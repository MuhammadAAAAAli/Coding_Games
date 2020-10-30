using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NUglify;

namespace KeywordMe
{
    public static class StringExtension
    {
        public static string OnlyCharsWillBeReturned(this string s)
        {
            var sb = new StringBuilder();
            foreach (char c in s)
            {
                if (char.IsLetter(c))
                    sb.Append(c);
            }
            return sb.ToString();
        }
    }

    internal class Program
    {
        static List<string> allWebsites = new List<string>()
            {
                "IT_in_Culori"
            };


        private static void Main(string[] args)
        {


            // join, toLower, replace punctuation marks with spaces, split in to individual words, extra check that will leave only chars in each word
            foreach (var websiteName in allWebsites)
            {
                for (int words = 2; words <= 4; words++)
                {
                    DoShit(websiteName, words);
                }
            }
        }

        private static void DoShit(string websiteName, int words)
        {
            // variables 
            var pages = GetHtmlFiles(@"C:\My Web Sites\" + websiteName);
            var rawWords = new List<string>();
            var rawKeywords = new List<string>();
            var sanitizedStrings = new List<string>();

            // get all HTML text from files
            foreach (var page in pages)
            {
                rawWords.Add(Uglify.HtmlToText(File.ReadAllText(page)).Code);
            }

            // process the srings and sanitize them
            var oneString = string.Join(" ", rawWords).ToLower();
            oneString = Regex.Replace(oneString, @"\W|_", " ");
            rawWords = oneString.Split(' ').ToList();
            foreach (var r in rawWords)
            {
                // after sanitization word should have > 0 size
                if (r.Trim().OnlyCharsWillBeReturned().Any())
                    sanitizedStrings.Add(r.Trim().OnlyCharsWillBeReturned());
            }

            RawKeywords(words, sanitizedStrings, rawKeywords);

            //grup keywords
            var groupedKeywords = from word in rawKeywords.Cast<string>()
                                  group word by word
                into g
                                  select new { Word = g.Key, Count = g.Count() };

            groupedKeywords = groupedKeywords.OrderByDescending(x => x.Count);
            if (!Directory.Exists(Environment.CurrentDirectory + @"\" + websiteName))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + @"\" + websiteName);
            }
            using (
                FileStream fs = new FileStream(Environment.CurrentDirectory + @"\" + websiteName + @"\" + websiteName + "_" + words + "_words" + ".txt",
                    FileMode.OpenOrCreate, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                var counter = 0;
                foreach (var word in groupedKeywords)
                {
                    if (counter < 150)
                    {
                        var percent = ((word.Count * 100) % groupedKeywords.Count()).ToString();
                        sw.WriteLine(word.Word + " | " + word.Count + " | " +
                                     ((double)((word.Count * 100) / groupedKeywords.Count()) + "." +
                                      percent.Substring(0, percent.Length > 1 ? 2 : percent.Length) + "%"));
                    }
                        
                    else
                        break;
                    counter++;
                }
                sw.Flush();
                sw.Close();
            }
        }

        #region private logic
        private static IEnumerable<string> GetHtmlFiles(string directory)
        {
            var files = new List<string>();

            try
            {
                files.AddRange(Directory.GetFiles(directory, "*.html", SearchOption.AllDirectories));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return files;
        }
        private static void RawKeywords(int words, List<string> sanitizedStrings, List<string> rawKeywords)
        {
            if (words == 1)
            {
                // 1 word
                for (var i = 0; i < sanitizedStrings.Count() - 1; i++)
                {
                    rawKeywords.Add(sanitizedStrings[i]);
                }
            }
            else if (words == 2)
            {
                // 2 words
                for (var i = 0; i < sanitizedStrings.Count() - 2; i = i + 2)
                {
                    rawKeywords.Add(sanitizedStrings[i] + " " + sanitizedStrings[i + 1]);
                }
                for (var i = 1; i < sanitizedStrings.Count() - 2; i = i + 2)
                {
                    rawKeywords.Add(sanitizedStrings[i] + " " + sanitizedStrings[i + 1]);
                }
            }
            else if (words == 3)
            {
                // 3 words
                for (var i = 0; i < sanitizedStrings.Count() - 3; i = i + 3)
                {
                    rawKeywords.Add(sanitizedStrings[i] + " " + sanitizedStrings[i + 1] + " " + sanitizedStrings[i + 2]);
                }
                for (var i = 1; i < sanitizedStrings.Count() - 3; i = i + 3)
                {
                    rawKeywords.Add(sanitizedStrings[i] + " " + sanitizedStrings[i + 1] + " " + sanitizedStrings[i + 2]);
                }
                for (var i = 2; i < sanitizedStrings.Count() - 3; i = i + 3)
                {
                    rawKeywords.Add(sanitizedStrings[i] + " " + sanitizedStrings[i + 1] + " " + sanitizedStrings[i + 2]);
                }
            }
            else if (words == 4)
            {
                // 3 words
                for (var i = 0; i < sanitizedStrings.Count() - 4; i = i + 4)
                {
                    rawKeywords.Add(sanitizedStrings[i] + " " + sanitizedStrings[i + 1] + " " + sanitizedStrings[i + 2] + " " +
                                    sanitizedStrings[i + 3]);
                }
                for (var i = 1; i < sanitizedStrings.Count() - 4; i = i + 4)
                {
                    rawKeywords.Add(sanitizedStrings[i] + " " + sanitizedStrings[i + 1] + " " + sanitizedStrings[i + 2] + " " +
                                    sanitizedStrings[i + 3]);
                }
                for (var i = 2; i < sanitizedStrings.Count() - 4; i = i + 4)
                {
                    rawKeywords.Add(sanitizedStrings[i] + " " + sanitizedStrings[i + 1] + " " + sanitizedStrings[i + 2] + " " +
                                    sanitizedStrings[i + 3]);
                }
                for (var i = 3; i < sanitizedStrings.Count() - 4; i = i + 4)
                {
                    rawKeywords.Add(sanitizedStrings[i] + " " + sanitizedStrings[i + 1] + " " + sanitizedStrings[i + 2] + " " +
                                    sanitizedStrings[i + 3]);
                }
            }
        }
        #endregion

    }
}
