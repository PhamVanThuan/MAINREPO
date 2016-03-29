using System;
using System.Data;
using System.IO;

namespace SAHL.Common.Utils
{
    /// <summary>
    /// Reading and writing to files and data streams,
    /// and types that provide basic file and directory support
    /// </summary>
    public static class IOUtils
    {
        /// <summary>
        /// Write content to the file system
        /// </summary>
        /// <param name="fileContent"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        public static void Save(byte[] fileContent, string path, string fileName)
        {
            if (!DirectoryExists(path))
                CreateDirectory(path);

            // Write the contents to a file.
            try
            {
                FileStream stream = File.Create(path + fileName, fileContent.Length);
                //Console.WriteLine("File created.");
                stream.Write(fileContent, 0, fileContent.Length);
                //Console.WriteLine("Result written to the file.");
                stream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        private static void CreateDirectory(string url)
        {
            if (!Directory.Exists(url))
                Directory.CreateDirectory(url);
        }

        public static bool DirectoryExists(string url)
        {
            if (Directory.Exists(url))
                return true;

            return false;
        }

        public static bool FileExists(string url)
        {
            if (File.Exists(url))
                return true;

            return false;
        }

        /// <summary>
        /// Gets the list of files in a directory
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string[] DirectoryGetFiles(string url)
        {
            if (Directory.Exists(url))
                return Directory.GetFiles(url); ;

            return null;
        }

        /// <summary>
        /// Deletes a file
        /// </summary>
        /// <param name="url"></param>
        public static void FileDelete(string url)
        {
            if (File.Exists(url))
                File.Delete(url);
        }

        /// <summary>
        /// Reads a CSV file into a Data Table
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="splitChar"></param>
        /// <returns></returns>
        public static DataTable GetCsvFile(string fileName, char splitChar)
        {
            int rowCounter = 1;

            try
            {
                if (!FileExists(fileName))
                    return null;

                DataTable DT = new DataTable();
                string importLine = "";
                char[] splitCharArr = { splitChar };
                StreamReader importFile = GetStreamFromFile(fileName);

                if (importFile != null)
                {
                    //Header
                    if (!(importFile.EndOfStream))
                    {
                        importLine = importFile.ReadLine();
                        string[] splitLine = importLine.Split(splitCharArr);
                        for (int i = 0; i < splitLine.Length; i++)
                        {
                            DT.Columns.Add(splitLine[i]);
                        }
                    }
                    //Data
                    while (!(importFile.EndOfStream))
                    {
                        importLine = importFile.ReadLine();
                        if (importLine.Length > 0)
                        {
                            string[] splitLine = importLine.Split(splitCharArr);
                            DataRow newRow = DT.NewRow();
                            for (int i = 0; i < splitLine.Length; i++)
                            {
                                newRow[i] = splitLine[i];
                            }
                            DT.Rows.Add(newRow);
                            rowCounter++;
                        }
                        else
                        {
                            //TODO: throw an exception
                        }
                    }
                }
                else
                {
                    //TODO: throw an exception
                }
                return DT;
            }
            catch
            {
                rowCounter++;
                // TODO throw an exception col & row causing error
                return null;
            }
        }

        /// <summary>
        /// Reads a file into a data stream
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static StreamReader GetStreamFromFile(string url)
        {
            try
            {
                StreamReader file = new StreamReader(url);
                return file;
            }
            catch
            {
                return null;
            }
        }
    }
}