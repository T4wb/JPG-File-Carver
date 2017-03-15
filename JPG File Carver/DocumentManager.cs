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
        private RichTextBox _textBox;

        public DocumentManager()
        {

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
                    //TextRange range = new TextRange(
                    //    _textBox.Document.ContentStart,
                    //    _textBox.Document.ContentEnd
                    //    );

                    //range.Load(stream, DataFormats.Text);

                }

                return true;
            }

            return false;
        }
    }
}
