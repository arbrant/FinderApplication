using System;
using System.Collections.Generic;
using FileManager;

namespace ListManager
{
    public class ListFunctional
    {
        #region Signs Functions
        /// <summary>
        /// Fills an array with the signs we want to avoide while searching. We avoid punctuation marks, space, another signs like $, #, %, ets.
        /// </summary>
        /// <returns>Array with the signs.</returns>
        private static List<char> SignsForCounting()
        {
            var temp = new List<char>();
            for (int ascii = 32; ascii < 35; ascii++)
            {
                temp.Add((char)ascii);
            }

            for (int ascii = 40; ascii < 48; ascii++)
            {
                temp.Add((char)ascii);
            }

            return temp;
        }

        /// <summary>
        /// We use this method to check if the input char is equal to the one of signs we have to avoid. They are located at the signs array.
        /// </summary>
        /// <param name="inputChar"></param>
        /// <returns>True if the input char is the sign we want to evoid and false if it is not.</returns>
        private static bool CheckSign(char inputChar, List<char> signs)
        {
            return signs.Exists(element => element == inputChar);
        }
        #endregion

        /// <summary>
        /// Counts the amount of words in the list.
        /// </summary>
        /// <param name="text">List of lines where to count words.</param>
        /// <returns>An amount of words</returns>
        public static int CountWords(List<string> text)
        {
            List<char> signs = SignsForCounting();

            int words = 0;

            for (int i = 0; i < text.Count; i++)
            {
                bool prevIsWord = false;
                for (int j = 0; j < text[i].Length; j++)
                {
                    if (CheckSign(text[i][j], signs))
                    {
                        if (prevIsWord)
                        {
                            words++;
                            prevIsWord = false;
                        }
                        continue;
                    }

                    if (!prevIsWord)
                    {

                        prevIsWord = true;
                    }

                    if (prevIsWord && j == text[i].Length - 1)
                    {
                        words++;
                    }
                }
            }

            return words;
        }

        /// <summary>
		/// Converts lines to uppercase. It converts list immediately in the list from args.
		/// </summary>
		/// <param name="text">List of lines to convert.</param>
        public static void ConvertToUpperCase(List<string> text)
        {
            for (int i = 0; i < text.Count; i++)
            {
                text[i] = text[i].ToUpper();
            }
        }
    }
}
