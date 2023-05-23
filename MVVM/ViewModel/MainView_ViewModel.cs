using Find_Dupes.MVVM.Core;
using Find_Dupes.MVVM.Model;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Data;
using System.Windows.Input;

namespace Find_Dupes.MVVM.ViewModel
{
    internal class MainView_ViewModel : ObservableObject
    {
        private int _currentNumberOfFiles;
        private CollectionViewSource _fileCollection;
        private string _fileTypeFilter;
        private string _filterText;
        private ObservableCollection<MyFile> _myFiles;
        private string _selectedItemListView;

        public MainView_ViewModel()
        {
            GetAllFilesCommand = new RelayCommand(o => ExecuteGetAllFiles());
            _myFiles = new ObservableCollection<MyFile>();
            _fileCollection = new CollectionViewSource();
            _fileCollection.Source = _myFiles;
            _fileCollection.Filter += FileCollectionFilter;
        }

        public int CurrentNumberOfFiles
        {
            get
            {
                return _currentNumberOfFiles;
            }
            set
            {
                _currentNumberOfFiles = value;
                OnPropertyChanged();
            }
        }

        public string FileTypeFilter
        {
            get
            {
                return _fileTypeFilter;
            }
            set
            {
                _fileTypeFilter = value;
                OnPropertyChanged();
            }
        }

        public string FilterText
        {
            get
            {
                return _filterText;
            }
            set
            {
                _filterText = value;
                _fileCollection.View.Refresh();
                OnPropertyChanged();
            }
        }

        public ICommand GetAllFilesCommand
        {
            get;
        }

        public string SelectedItemListView
        {
            get
            {
                return _selectedItemListView;
            }
            set
            {
                _selectedItemListView = value;
                OnPropertyChanged();
                OnSelectedItemListViewChanged();
            }
        }

        public ICollectionView SourceCollection
        {
            get
            {
                return _fileCollection.View;
            }
        }

        private void ExecuteGetAllFiles()
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            dialog.SelectedPath = "";
            dialog.UseDescriptionForTitle = true;
            dialog.ShowNewFolderButton = true;
            dialog.Description = @"Select target folder";
            dialog.ShowDialog();
            GetAllFiles(dialog.SelectedPath);
        }

        void FileCollectionFilter(object sender, FilterEventArgs e)
        {
            if (string.IsNullOrEmpty(FilterText))
            {
                e.Accepted = true;
                return;
            }

            MyFile file = e.Item as MyFile;

            if (file.Name.ToUpper().Contains(FilterText.ToUpper()) || file.Type.ToUpper().Contains(FilterText) || file.Path.ToUpper().Contains(FilterText.ToUpper()))
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = false;
            }
        }

        private void GetAllFiles(string folderPath)
        {
            try
            {
                string[] allfiles = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);
                _myFiles.Clear();
                foreach (string file in allfiles)
                {
                    _myFiles.Add(new MyFile(file));
                }
                CurrentNumberOfFiles = _myFiles.Count;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void OnSelectedItemListViewChanged()
        {
            MyFile file = new MyFile(_selectedItemListView);
            string temp;
            int index = 0;
            for (int i = file.Path.Length - 1; i >= 0; i--)
            {
                if (file.Path[i] == '\\')
                {
                    index = i;
                    break;
                }
            }
            Process.Start("explorer.exe", file.Path.Substring(0, index));
            //Clipboard.SetText(file.Path.Substring(0,index));
        }
    }
}
