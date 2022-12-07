using System;
using System.IO;
using System.Collections.Generic;

namespace FileManager
{

    /// <summary>
    /// Static methods thet perform basic file operations: create, read, write files
    /// </summary>
    public class FileFunctional
    {
        //private static string FilePath; //path to the file 

        /// <summary>
        /// Creates file in a folder
        /// </summary>
        /// <param name="FolderPath">Path to the nesseccary folder.</param>
        /// <returns>The path to the file.</returns>
        public static string CreateFile (string FolderPath)
        {
            string extension = ".txt"; //extension
            string FilePath = FolderPath + "/file1" + extension; 
            int i = 1;

            //here we pick up suitable name for the file as "fileN", where n є N
            while (File.Exists(FilePath)) // check if file does exist
            {
                i++;
                FilePath = FolderPath + $"/file{i}" + extension; //change file name to check it in the next iteration
            }

            using (File.Create(FilePath)); //create file with the guarantee that it does not exist
            return FilePath;
        }

        /// <summary>
        /// Writes nesseccary text to the file
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="lines">List of lines to write to the file</param>
        /// <returns>True if the text was written successfully and false if not.</returns>
        public static bool WriteToFile (string FilePath, List<string> lines)
        {
            using (var streamWriter = new StreamWriter(FilePath))
            {
                if (streamWriter.BaseStream == null) return false;
                foreach (string line in lines)
                {
                    streamWriter.WriteLine(line);
                }
                return true;
            }
        }

        /// <summary>
        /// Reads file and set list with lines.
        /// </summary>
        /// <param name="path">The path to the file</param>
        /// <returns>List with lines from the file. If the file was not read correctly, it returns emplty list.</returns>
        public static List<string> ReadFile(string path)
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(path))
            {
                if (reader.BaseStream == null) return lines;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            };
            return lines;
        }

        /// <summary>
        /// Appands nesseccary text to the file
        /// </summary>
        /// <param name="FilePath">Path to the file.</param>
        /// <param name="lines">Lines of text to appand.</param>
        /// <returns>True if mentioned file exists and appsnding was made and false if not.</returns>
        public static bool AppandToFile(string FilePath, List<string> lines)
        {
            var workingDirectory = Environment.CurrentDirectory;
            if (File.Exists(FilePath)) //check if the file does exist, if not we return false
            {
                using (var streamWriter = File.AppendText(FilePath))
                {
                    foreach (string line in lines)
                    {
                        streamWriter.WriteLine(line);
                    }
                    return true;
                }

            }
            else
            {
                return false;
            }
        }
    }
}
