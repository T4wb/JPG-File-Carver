using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPG_File_Carver
{
    class FileTable
    {
        private List<byte[]> _verdelingFileTableBlocks = new List<byte[]>();
        public List<byte[]> VerdelingFileTableBlocks
        {
            get { return _verdelingFileTableBlocks; }
        }

        public void setVerdelingFileTableBlocks(byte[] value)
        {
            _verdelingFileTableBlocks.Add(value);
        }

        public List<byte[]> indexFileTable = new List<byte[]>();
    }
}
