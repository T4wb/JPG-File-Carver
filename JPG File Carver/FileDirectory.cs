using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPG_File_Carver
{
    class FileDirectory
    {
        private string _Hexadecimal;
        public string Hexadecimal
        {
            get { return _Hexadecimal; }

            set
            {
                _Hexadecimal = value;

                // set other variabels

            }
        }
        public int IndexFileTable;
        public int SizeFile;
        public int SizeName;
        public int NameFile;
    }
}
