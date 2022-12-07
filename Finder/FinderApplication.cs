using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileManager;
using ListManager;

namespace FinderApplication
{
    /// <summary>
    /// Console programe to operate files.
    /// </summary>
    public class Finder
    {
        #region Fields
        private string FolderPath = string.Empty; //path to folder where we will perform operations
        private List<string> option_names = new List<string>(); //this list will safe text of options to show them
        private List<Action> options = new List<Action>();
        #endregion

        #region Constructors
        public Finder ()
        {
            SetFolderPath_Console() ;//set the path of folder vie Console where all neccessary operations will be performed
            CreateOptions(); //implementation of the options
            Start(); //start program
        }

        public Finder(string FolderPath)
        {
            SetFolderPath(FolderPath); //set the path of folder where all neccessary operations will be performed
            CreateOptions(); //implementation of the options
            Start(); //start program
        }
        #endregion

        #region Set Folder Path
        private void SetFolderPath_Console()
        {
            Console.WriteLine("Enter path of necessary folder");
            string path;
            if (!Directory.Exists(path = Console.ReadLine()))
            {
                Console.WriteLine("FOLDER PATH ERROR!");
                Console.WriteLine();
                SetFolderPath_Console();
            }
            else
            {
                this.FolderPath = path;
            }
            Console.WriteLine();
        }
        
        private void SetFolderPath(string FolderPath)
        {
            if(!Directory.Exists(FolderPath))
            {
                Console.WriteLine("FOLDER PATH ERROR!");
                Console.WriteLine();
                SetFolderPath_Console();
            }
            else
            {
                this.FolderPath = FolderPath;
            }

        }
        #endregion

        #region Options Control
        /// <summary>
        /// Shows all operations for the user
        /// </summary>
        private void ShowOperations(string massagge, List<string> options)
        {
            Console.WriteLine(massagge);
            int numb = 1; // number of operation
            foreach (string line in options) // to show all possible operations for the user
            {
                Console.WriteLine("{0} - {1}", numb, line); //numb is used to show the order of operations
                numb++;
            }
            Console.WriteLine();
        }

        private void Menu()
        {
            ShowOperations("Choose operation!", option_names);
            Console.WriteLine("0 - End");
            Console.WriteLine();
        }

        /// <summary>
        /// Adds an option to the menu
        /// </summary>
        /// <param name="name"></param>
        /// <param name="func"></param>
        private void AddFunction(string name, Action func)
        {
            option_names.Add(name);
            options.Add(func);
        }

        /// <summary>
        /// Creates all possible options. Fills private list options and option_name with methods and their name for the menu. 
        /// </summary>
        private void CreateOptions()
        {
            AddFunction("Create txt file", CreateTxtFile);
            AddFunction("Create txt file and fill it", CreateAndFillTxtFile);
            AddFunction("Appand txt file", AppandTxtFile);
            AddFunction("Read txt file", ReadTxtFile);
            AddFunction("Count words in txt file", CountWordsTxtFile);
            AddFunction("Uppercase txt file", UpperCaseTxtFile);
        }
        #endregion

        #region Options
        private void CreateTxtFile ()
        {
            string pth1 = FileFunctional.CreateFile(FolderPath);
            Console.WriteLine("File is created.\n Path: {0}", pth1);
        }

        private void CreateAndFillTxtFile ()
        {
            Console.WriteLine("Write text to the new file. Type 0 in a new line and ENTER to stop!");
            Console.WriteLine();

            List<string> consoleLines = ReadConsole();
            string pth = FileFunctional.CreateFile(FolderPath);
            FileFunctional.WriteToFile(pth, consoleLines);
            Console.WriteLine("File is created and filled.\n Path: {0}", pth);
        }

        private void AppandTxtFile ()
        {
            string path = ChooseTxtFile();
            if (path == string.Empty) return;

            Console.WriteLine("Write text to the new file. Type 0 in a new line and ENTER to stop!");
            Console.WriteLine();

            var consoleLines = ReadConsole();
            FileFunctional.AppandToFile(path, consoleLines);
            Console.WriteLine();

            Console.WriteLine("File is appanded.\n Path: {0}", path);
        }

        private void ReadTxtFile ()
        {
            string path = ChooseTxtFile();
            if (path == string.Empty) return;

            List<string> lines = FileFunctional.ReadFile(path);
            if (lines.Count == 0)
            {
                Console.WriteLine("File is empty.\n Path: {0}", path);
                return;
            }
            ShowLines(lines);

            Console.WriteLine("File is read.\n Path: {0}", path);
        }

        private void CountWordsTxtFile ()
        {
            string path = ChooseTxtFile();
            if (path == string.Empty) return;

            List<string> lines = FileFunctional.ReadFile(path);
            Console.WriteLine("Words: {0}.\n Path: {1}",ListFunctional.CountWords(lines), path);
        }

        private void UpperCaseTxtFile ()
        {
            string path = ChooseTxtFile();
            if (path == string.Empty) return;

            List<string> lines = FileFunctional.ReadFile(path);
            ListFunctional.ConvertToUpperCase(lines);
            string massage = (FileFunctional.WriteToFile(path, lines)) ? string.Format("File is converted to uppercase.\n Path: {1}", ListFunctional.CountWords(lines), path) : "Error!";
            Console.WriteLine(massage);
        }
        #endregion

        #region Additions
        /// <summary>
        /// Checks if imput entered in a correct form
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private int CheckInput(string str)
        {
            if(Int32.TryParse(str,out int numb) && numb >= 0)
            {
                return numb;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Returns the list of context of directory that are txt
        /// </summary>
        /// <returns>List of context of directory that are txt</returns>
        private List<string> GetTxtFiles()
        {
            List<string> files = (Directory.GetFiles(FolderPath)).ToList();
            return files.Where(file => file.EndsWith(".txt")).ToList();
        }

        /// <summary>
        /// Reads lines from console until 0 will not be typed in one line. 
        /// </summary>
        /// <returns>List of lines from the console</returns>
        private List<string> ReadConsole()
        {
            var lines = new List<string>();
            string line;
            while ((line = Console.ReadLine()) != "0")
            {
                lines.Add(line);
            }

            return lines;
        }

        /// <summary>
        /// Chooses file in the folder.
        /// </summary>
        /// <returns>Returns choosen file path if there are files in folder. Returns empty line if there are no files.</returns>
        private string ChooseTxtFile()
        {
            List<string> context = GetTxtFiles();
            if (context.Count == 0) Console.WriteLine("NO FILES IN THE FOLDER");
            else
            {
                string massage = "Choose file!";
                ShowOperations(massage, context);
                Console.WriteLine();
                string input = Console.ReadLine();
                while (CheckInput(input) == -1 || Int32.Parse(input) < 0 || Int32.Parse(input) > context.Count())
                {
                    Console.WriteLine("WRONG FORMAT!");
                    Console.WriteLine();
                    ShowOperations(massage, context);
                    input = Console.ReadLine();
                }

                Console.WriteLine();
                return context[Int32.Parse(input) - 1];
            }
            Console.WriteLine();
            return string.Empty;
        }

        /// <summary>
        /// Show lines in a console from the list.
        /// </summary>
        /// <param name="lines">List of lines.</param>
        private void ShowLines (List<string> lines)
        {
            foreach(string str in lines)
            {
                Console.WriteLine(str);
            }
            Console.WriteLine();
        }
        #endregion

        public void Start()
        {
            Menu();
            string input;
            while((input = Console.ReadLine()) != "0") {

                if (CheckInput(input) == -1)
                {
                    Console.WriteLine();
                    Console.WriteLine("WRONG FORMAT!");
                    Console.WriteLine();
                    Menu();
                }
                else if (CheckInput(input) >= 1 && CheckInput(input) <= options.Count)
                {
                    Console.WriteLine();
                    options[Int32.Parse(input) - 1].Invoke();
                    Console.WriteLine();
                    Menu();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("WRONG FORMAT!");
                    Console.WriteLine();
                    Menu();
                }
            }
        }
    }
}
