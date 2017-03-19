using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
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
                    // get blocksize in bytes and set to int
                    BinaryReader br = new BinaryReader(stream);
                    _blockSize = getBlockSize(br);

                    // split fileblocks into the the value of _blocksize
                    //List<string> formattedBlocks = splitBlocks(stream, br);
                    byte[][] formattedBlocks = splitBlocks(stream, br);
                    // bytes[] formattedBlocks = splitBlocks(stream, br);

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
        /// <returns>Returns the status of the process of carving the document as a boolean value.</returns>
        public bool CarveDocument()
        {
            // Carve File
            //int firstBlock = 234;
            //string byteAdress = _fileBinary._fileTable.indexFileTable[234];
            List<byte[]> carvedData = new List<byte[]>();

            int pointer = 234; // To do: get from FileMetaData
            
            while (pointer != 0)
            {
                carvedData.Add(_fileBinary._fileData.fileData[pointer-12]);
                pointer = BitConverter.ToInt32(
                    BitConverter.IsLittleEndian
                    ? _fileBinary._fileTable.indexFileTable[pointer].Reverse().ToArray()
                    : _fileBinary._fileTable.indexFileTable[pointer], 0);
                //pointer = int.Parse(_fileBinary._fileTable.indexFileTable[pointer], System.Globalization.NumberStyles.HexNumber);
            }

            // Save file
            //if (SaveDocumentAs(carvedData))
            //{
            //    return true;
            //}

            return false;
        }

        /// <summary>
        /// This method saves the document.
        /// </summary>
        /// <param name="carvedData">This is the carved data in binary.</param>
        /// <returns></returns>
        public bool SaveDocumentAs(byte[] carvedData)
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
                ;
                int hello = int.Parse(carvedData[3]+"", NumberStyles.HexNumber);

                //Convert.ToByte
                File.WriteAllText(@_currentFile, carvedData.ToString()); // doesn't work => export as binary not as text

                // To do: export offsets into .txt file with name of dlg.FileName
                return true;
            }

            return false;
        }

        private int getBlockSize(BinaryReader br)
        {
            byte[] blockSize = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                blockSize[i] = br.ReadByte();
            }

            int x = BitConverter.ToInt32(
                BitConverter.IsLittleEndian
                   ? blockSize.Reverse().ToArray()
                   : blockSize, 0);

            return x;
        }

        private byte[][] splitBlocks(Stream stream, BinaryReader br)
        {
            // 
            long totalBlocks = stream.Length / _blockSize;
            byte[][] verdelingBestandBlocks = new byte[totalBlocks][];

            for (int i = 0; i < totalBlocks; i++)
            {
                verdelingBestandBlocks[i] = new byte[_blockSize];
            }

            // 
            br.BaseStream.Position = 0;

            for (int i = 0; i < totalBlocks; i++)
            {
                for (int j = 0; j < _blockSize; j++)
                {
                    verdelingBestandBlocks[i][j] = br.ReadByte();
                }
            }

            return verdelingBestandBlocks;
        }

        private void setBlocks(byte[][] verdelingBestandBlocks)
        {
            // set first block
            _fileBinary._fileMetaData.setHexadecimal(verdelingBestandBlocks[0]);

            // set second block
            _fileBinary._fileDirectory.setHexadecimal(verdelingBestandBlocks[1]);

            // set third block to 12th block: file table // Refactor: shift to FileTable Class

            for (int k = 2; k < 12; k++) // shift to class? => looping part? pass jagged array
            {
                _fileBinary._fileTable.setVerdelingFileTableBlocks(verdelingBestandBlocks[k]); // 4096 bytes of i.e. [1,2,3,4] 
                //_fileBinary._fileTable.verdelingBestandBlocks.Add(verdelingBestandBlocks[k]);
            }

            // setIndexTabel // Refactor: shift to FileTable Class
            List<byte> tempBytes = new List<byte>();
            int count = 0;

            for (int i = 0; i < _fileBinary._fileTable.VerdelingFileTableBlocks[0].Length; i++)
            {
                if (i != 0 && i % 8 == 0)
                {
                    _fileBinary._fileTable.indexFileTable.Add(tempBytes.ToArray());
                    count = 0;
                    tempBytes.Clear();
                }

                tempBytes.Add(verdelingBestandBlocks[0][count]);
                count++;
            }

            //// set other blocks: file data // Refactor: shift to FileData Class
            for (int i = 12; i < verdelingBestandBlocks.Length; i++)
            {
                _fileBinary._fileData.fileData.Add(verdelingBestandBlocks[i]);
            }
        }
    }
}
