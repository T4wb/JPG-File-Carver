using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace JPG_File_Carver
{
    class DocumentManager
    {
        private string _currentFile;
        private List<Int32> _binaryStream;

        public DocumentManager()
        {
            _binaryStream = new List<Int32>();
        }

        public bool OpenDocument()
        {
            OpenFileDialog dlg = new OpenFileDialog()
            {
                Filter = "Zuyd File System  | *.zufs"
            };

            if (dlg.ShowDialog() == true)
            {
                _currentFile = dlg.FileName;

                using (Stream stream = dlg.OpenFile())
                {
                    byte[] temp_binaryStream = new BinaryReader(stream).ReadBytes((int)stream.Length);
                }

                return true;
            }

            return false;
        }
    }
}
