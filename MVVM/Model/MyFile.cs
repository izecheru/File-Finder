using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find_Dupes.MVVM.Model
{
    internal class MyFile
    {
        private string _name;
        private string _type;
        private string _path;
        public string Name { get { return _name; } set { _name = value; } }
        public string Type { get { return _type; } set { _type = value; } }
        public string Path { get { return _path; } set { _path = value; } }

        public MyFile(string path)
        {
            _name = FileNameStrip(path);
            _type = FileTypeStrip(path);
            _path = path;
        }
        public static string FileTypeStrip(string fileName)
        {
            int index = fileName.Length - 1;
            while (index != 0)
            {
                if (fileName[index] == '.')
                {
                    index++;
                    break;
                }
                index--;
            }
            return fileName.Substring(index);
        }
        public static string FileNameStrip(string fileName)
        {
            int index = fileName.Length - 1;
            while (index != 0)
            {
                if (fileName[index] == '\\')
                {
                    index++;
                    break;
                }
                index--;
            }
            return fileName.Substring(index);
        }
        public override string ToString()
        {
            return _path;
        }

    }
}
