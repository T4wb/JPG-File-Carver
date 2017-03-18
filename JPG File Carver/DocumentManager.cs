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
        // variables
        private string _currentFile;

        private FileBinary _fileBinary;

        private int _blockSize;

        public DocumentManager()
        {
            _fileBinary = new FileBinary();
        }

        /// <summary>
        /// This method opens the document.
        /// </summary>
        /// <returns>Returns the status of the process of opening the document as a boolean value.</returns>
        public bool OpenDocument()
        {
            OpenFileDialog dlg = new OpenFileDialog()
            {
                Filter = "Zuyd File System  | *.zufs"
            };

            if (dlg.ShowDialog() == true)
            {
                _currentFile = dlg.FileName;

                // read file in binary
                using (Stream stream = dlg.OpenFile())
                {
                    // gets blocksize in bytes
                    BinaryReader br = new BinaryReader(stream);
                    string hexvalue = getBlockSize(br);

                    // set the blocksize
                    _blockSize = int.Parse(hexvalue, System.Globalization.NumberStyles.HexNumber);

                    // split fileblocks into the the value of _blocksize 
                    List<string> formattedBlocks = splitBlocks(stream, br);

                    // set the splitted blocks into _fileBinary
                    setBlocks(formattedBlocks);
                }

                return true;
                }

            return false;
        }

        /// <summary>
        /// This method carves the document.
        /// </summary>
        /// <returns>Returns the status of the process of carving the document as a boolean value. </returns>
        public bool CarveDocument()
        {
            // Carve File
            //int firstBlock = 234;
            //string byteAdress = _fileBinary._fileTable.indexFileTable[234];
            string carvedData = "";
            int pointer = 234; // To do: get from FileMetaData
            
            while (pointer != 0)
            {
                carvedData += _fileBinary._fileData.fileData[pointer-12];
                pointer = int.Parse(_fileBinary._fileTable.indexFileTable[pointer], System.Globalization.NumberStyles.HexNumber);
            }

            if (SaveDocumentAs(carvedData))
            {
                return true;
            }

            return false;
            
        }

        public bool SaveDocumentAs(string carvedData)
        {
            // save data into .txt
            SaveFileDialog dlg = new SaveFileDialog()
            {
                Filter = "JPEG | *.jpg"
            };

            // To Do: file checking = begins with FF D8 && ENDS WITH FF D9? true+save document + open Document(folder) : false
            if (dlg.ShowDialog() == true)
            {
                _currentFile = dlg.FileName;
                //byte[] data = System.Text.Encoding.ASCII.GetBytes(carvedData);
                File.WriteAllText(@_currentFile, carvedData); // doesn't work => export as binary not as tex

                
                // To do: export offsets into .txt file with name of dlg.FileName
            }

            return false;


            
        }

        private static string getBlockSize(BinaryReader br)
        {
            string hexvalue = null;
            for (int i = 0x00000; i < 0x00004; i++)
            {
                br.BaseStream.Position = i;
                string temp = br.ReadByte().ToString("X2");

                hexvalue += temp;
            }

            return hexvalue;
        }

        private List<string> splitBlocks(Stream stream, BinaryReader br)
        {
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

            return verdelingBestandBlocks;
        }

        private void setBlocks(List<string> verdelingBestandBlocks)
        {
            string tempString;
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

            // setIndexTabel
            tempString = "";
            for (int x = 0; x < _fileBinary._fileTable.verdelingBestandBlocks[0].Length; x++)
            {

                if (x != 0 && x % 8 == 0)
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
    }
}
