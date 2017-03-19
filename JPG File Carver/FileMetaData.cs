using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPG_File_Carver
{
    class FileMetaData
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

        public int BlockSize;
        public int BlockMetaData;
        public int SizeFileDirectory;
        public int BlockFileTabel;
    }
}
