using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEngine;

namespace Voxelated.Utilities {
    /// <summary>
    /// Helper class for various file input / output tasks.
    /// </summary>
    public static class FileUtils {
        #region File IO
        /// <summary>
        /// Checks if a file with the specified name exists
        /// in the folder.
        /// </summary>
        /// <param name="directory">The folder to check in.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <returns>True if a file exists with that name</returns>
        public static bool DoesFileExist(string directory, string fileName) {
            string fullFileName = directory + "/" + fileName;
            return File.Exists(fullFileName);
        }

        /// <summary>
        /// Saves a byte array to file.
        /// </summary>
        /// <param name="directory">The folder to save it in.</param>
        /// <param name="fileName">The name of the file to save as.</param>
        /// <param name="fileContent">The content of the file.</param>
        /// <param name="zipped">If the file should be gzipped.</param>
        /// <param name="overwrite">If a file with the same name already exists. Overwrite it?</param>
        /// <returns>True if the operation was a success.</returns>
        public static bool SaveFile(string directory, string fileName, byte[] fileContent, bool zipped, bool overwrite = false) {
            string fullFileName = directory + "/" + fileName;

            //If the folder doesn't exist. Make it.
            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }

            //File already exists. Check if overwrite is enabled.
            if (File.Exists(fullFileName)) {
                if (overwrite) {
                    File.Delete(fullFileName);
                }
                else {
                    return false;
                }
            }

            //Create the new filestream.
            FileStream fileStream = new FileStream(fullFileName, FileMode.Create, FileAccess.Write, FileShare.None);

            //Write the content to file.
            using (BinaryWriter binWriter = new BinaryWriter(fileStream)) {
                if (zipped) {
                    byte[] compressedFile = FileUtils.Compress(fileContent);
                    binWriter.Write(compressedFile);
                }
                else {
                    binWriter.Write(fileContent);
                }
            }

            fileStream.Close();
            return true;
        }

        /// <summary>
        /// Delets the file at the specified index of the folder.
        /// </summary>
        /// <param name="directory">The folder to work in.</param>
        /// <param name="index">The file index to delete.</param>
        /// <returns>True if the file was deleted.</returns>
        public static bool DeleteFileAtIndex(string directory, int index) {
            if(index >= 0 && index < Directory.GetFiles(directory).Length) {
                string fileName = Directory.GetFiles(directory)[index];
                File.Delete(fileName);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the number of files in the folder.
        /// </summary>
        /// <param name="directory">The folder to count files in.</param>
        /// <returns>How many files there are. 0 if none.</returns>
        public static int GetFileCount(string directory) {
            if (Directory.Exists(directory)) {
                return Directory.GetFiles(directory).Length;
            }
            else {
                return 0;
            }
        }

        /// <summary>
        /// Deletes the file with the following name.
        /// </summary>
        /// <param name="directory">The folder to look in.</param>
        /// <param name="fileName">The file name to delete.</param>
        /// <returns>True if the file was found and deleted.</returns>
        public static bool DeleteFile(string directory, string fileName) {
            string fullFileName = directory + "/" + fileName;

            if (File.Exists(fullFileName)) {
                File.Delete(fullFileName);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Load a file from memory as a byte array.
        /// </summary>
        /// <param name="directory">The folder it resides in.</param>
        /// <param name="fileName">The file name...</param>
        /// <param name="dezipp">If it shouded be passed through gzip.</param>
        /// <returns>The file if it was found.</returns>
        public static byte[] LoadFile(string directory, string fileName, bool dezipp) {
            string fullFileName = directory + "/" + fileName;

            //Ensure the file exists.
            if (!File.Exists(fullFileName)) {
                LoggerUtils.LogError("File: " + fullFileName + " does not exist.");
                return null;
            }

            FileStream fileStream = new FileStream(fullFileName, FileMode.Open);
            byte[] fileBytes;

            //Read in the file bytes to an array
            using (BinaryReader binReader = new BinaryReader(fileStream)) {
                binReader.BaseStream.Position = 0;
                fileBytes = binReader.ReadBytes((int)fileStream.Length);
            }

            //Close the stream and decompress the bytes.
            fileStream.Close();

            return dezipp ? Decompress(fileBytes) : fileBytes;
        }
        #endregion

        #region Compression
        /// <summary>
        /// Compress the byte array using c#'s built in gzip.
        /// Don't bother calling this on byte arrays shorter than 30 bytes.
        /// </summary>
        /// <param name="raw">The byte array to zip.</param>
        /// <returns>The compressed byte array.</returns>
        public static byte[] Compress(byte[] raw) {
            using (MemoryStream memory = new MemoryStream()) {
                using (GZipStream gzip = new GZipStream(memory, CompressionMode.Compress, true)) {
                    gzip.Write(raw, 0, raw.Length);
                }
                return memory.ToArray();
            }
        }

        /// <summary>
        /// Decompress a gzipped byte array.
        /// </summary>
        /// <param name="gzip">The byte array to decompress.</param>
        /// <returns>The decompressed byte array.</returns>
        public static byte[] Decompress(byte[] gzip) {
            // Create a GZIP stream with decompression mode.
            // ... Then create a buffer and write into while reading from the GZIP stream.
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip),
                CompressionMode.Decompress)) {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream()) {
                    int count = 0;
                    do {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0) {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }
        #endregion
    }
}