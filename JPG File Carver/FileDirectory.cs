using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPG_File_Carver
{
    class FileDirectory
    {
        private byte[] _Hexadecimal;
        public byte[] Hexadecimal
        {
            get { return _Hexadecimal; }
        }

        public void setHexadecimal(byte[] value)
        {
            _Hexadecimal = new byte[value.Length];

            for (int i = 0; i < value.Length; i++)
            {
                _Hexadecimal[i] = value[i];
            }
        }

        public int IndexFileTable;
        public int SizeFile;
        public int SizeName;
        public int NameFile;
    }
}
