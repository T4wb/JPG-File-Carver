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

        private FileBinary _fileBinary;

        private int _blockSize;

        public DocumentManager()
        {
            _fileBinary = new FileBinary();
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
                    // read file in binary

                    // getBlockSize in Bytes
                    BinaryReader br = new BinaryReader(stream);
                    string hexvalue = null;
                    for (int i = 0x00000; i < 0x00004; i++)
                    {
                        br.BaseStream.Position = i;
                        string temp = br.ReadByte().ToString("X2");

                        hexvalue += temp;
                    }

                    _blockSize = int.Parse(hexvalue, System.Globalization.NumberStyles.HexNumber);

                    // verdeel bestand in blocks van 4096 = 579 blocks?
                    List<string> verdelingBestandBlocks = new List<string>();
                    string tempString = "";
                    br.BaseStream.Position = 0;

                    int j;
                    for (j = 0; j < stream.Length; j++)
                    {
                        byte[] xo = br.ReadBytes(_blockSize);

                        foreach (byte x in xo)
                        {
                            tempString += x.ToString("X2");
                        }

                        verdelingBestandBlocks.Add(tempString);
                        j += _blockSize;
                        tempString = "";
                    }

                    // set first block
                     _fileBinary._fileMetaData.Hexadecimal = verdelingBestandBlocks[0];

                    // set second block
                    // _fileBinary._fileMetaData.Hexadecimal = readFile(dlg, 0);
                    _fileBinary._fileDirectory.Hexadecimal = verdelingBestandBlocks[1];

                    // set third block to 12th block: file table
                    for (int k = 2; k < 12; k++)
                    {
                        _fileBinary._fileTable.verdelingBestandBlocks.Add(verdelingBestandBlocks[k]);
                    }

                    // IndexTabel
                    tempString = "";
                    for (int x = 0; x < _fileBinary._fileTable.verdelingBestandBlocks[0].Length; x++)
                    {
                        
                        if (x !=0 && x%8 == 0)
                        {
                            _fileBinary._fileTable.indexFileTable.Add(tempString);
                            tempString = "";
                        }

                        tempString += _fileBinary._fileTable.verdelingBestandBlocks[0][x];
                    }

                    // set other blocks: file data
                    for (int k = 12; k < verdelingBestandBlocks.Count; k++)
                    {
                        _fileBinary._fileData.fileData.Add(verdelingBestandBlocks[k]);
                    }
                }

                return true;
                }

            return false;
        }

        public bool CarveDocument()
        {
            // Carve File
            //int firstBlock = 234;
            //string byteAdress = _fileBinary._fileTable.indexFileTable[234];
            string fileCarved = "";
            int pointer = 234; // To do: get from FileMetaData
            
            while (pointer != 0)
            {
                //
                fileCarved += _fileBinary._fileData.fileData[pointer-12];

                // ga terug naar fileTable en ga daar vanuit index 0 naar de pointer
                pointer = int.Parse(_fileBinary._fileTable.indexFileTable[pointer], System.Globalization.NumberStyles.HexNumber);
            }

            return true;
        }

        // Save document
    }
}
