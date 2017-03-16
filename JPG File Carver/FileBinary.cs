using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPG_File_Carver
{
    class FileBinary
    {
        public FileMetaData _fileMetaData;
        public FileDirectory _fileDirectory;
        public FileTable _fileTable;
        public FileData _fileData;
        
        public FileBinary()
        {
            _fileMetaData = new FileMetaData();
            _fileDirectory = new FileDirectory();
            _fileTable = new FileTable();
            _fileData = new FileData();
        }
    }
}
