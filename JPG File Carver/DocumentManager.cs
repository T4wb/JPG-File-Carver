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

                    // verdeel bestand in blocks van 4096 = 580 blocks?
                    List<string> verdelingBestandBlocks = new List<string>();
                    string tempString = "";
                    br.BaseStream.Position = 0;

                    int j;
                    for (j = 0; j < stream.Length; j++)
                    {

                        byte[] xo = br.ReadBytes(_blockSize - 1);

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

                    // set third block: file table
                     _fileBinary._fileTable.verdelingBestandBlocks.Add(verdelingBestandBlocks[2]);

                    // set other blocks: file data
                    for (int k = 3; k < verdelingBestandBlocks.Count; k++)
                    {
                        _fileBinary._fileData.fileData.Add(verdelingBestandBlocks[k]);
                    }

                    ////getSize of Metadata block in Blocks
                    //hexvalue = null;
                    //for (int i = 0x00004; i < 0x00008; i++)
                    //{
                    //    br.BaseStream.Position = i;
                    //    string temp = br.ReadByte().ToString("X2");

                    //    hexvalue += temp;
                    //}

                    ////getSize of File directory block in Blocks
                    //hexvalue = null;
                    //for (int i = 0x00009; i < 0x0000D; i++)
                    //{
                    //    br.BaseStream.Position = i;
                    //    string temp = br.ReadByte().ToString("X2");

                    //    hexvalue += temp;
                    //}

                    ////getSize of File table in Blocks
                    //br.Close();
                }

                // To Do:

                // set first block
                // _fileBinary._fileMetaData.Hexadecimal = readFile(stream, 0);

                // set second block
                // _fileBinary._fileMetaData.Hexadecimal = readFile(dlg, 0);

                // set third block
                // _fileBinary._fileMetaData.Hexadecimal = readFile(dlg, 0);

                // set other blocks
                // ??

                return true;
                }

            return false;
        }

        //private string readFile(Stream stream, int pos)
        //{
        //    BinaryReader br = new BinaryReader(stream);
        //    br.BaseStream.Position = pos;
        //    byte[] xo = br.ReadBytes(_blockSize-1);
        //    string testingBytes = null;

        //    foreach(byte x in xo)
        //    {
        //        testingBytes += x.ToString("X2");
        //    }

        //    br.Close();
        //    return testingBytes;
        //}
    }
}
