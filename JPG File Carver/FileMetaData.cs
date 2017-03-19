using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPG_File_Carver
{
    class FileMetaData
    {
        //private string _Hexadecimal;
        //public string Hexadecimal
        //{
        //    get { return _Hexadecimal; }

        //    set
        //    {
        //        _Hexadecimal = value;

        //        // set other variabels

        //    }
        //}
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

        //00: Block size in bytes	
        //04: Size of Metadata block(in blocks)
        //08: Size of File directory block(in blocks)
        //12: Size of File table(in blocks)
    }
}
