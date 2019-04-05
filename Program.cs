using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Net;

namespace ConsoleApp1_RingbaChallenge
{

    class Program
    {
        static void Main(string[] args)
        {
            // Validate argument for file location exists
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Please provide a valid remote file location.");

            }
                // Hard coded location for testing - be sure to comment out arg based remoteLocation below if using hard code
                //string remoteLocation = "insert remote .txt url location here";
                //NOTE: This program was written to accept .txt remote file compressed with no spaces or symbols, each word separated by camel casing

                // Access text file
                string remoteLocation = args[0];
                WebClient client = new WebClient();

                Stream data = client.OpenRead(remoteLocation);
                StreamReader reader = new StreamReader(data);

                // Read entire text file
                string text = reader.ReadToEnd();
                data.Close();
                reader.Close();

                // Array to store frequencies
                int[] countArr = new int[(int)char.MaxValue];
                // Int to store uppercase count
                int upper = 0;


                // Iterate over each letter
                foreach (char k in text)
                {
                    // Increment countArr
                    countArr[(int)Char.ToLower(k)]++;
                    // Count uppercase
                    if (Char.IsUpper(k))
                        upper++;
                }
                // Write letters with frequencies
                //Test: opperating as expected
                for (int i = 0; i < (int)char.MaxValue; i++)
                {
                    if(countArr[i] > 0 && char.IsLetterOrDigit((char)i))
                    {
                        Console.WriteLine("Letter: {0} Frequency: {1}",
                        (char)i,
                        countArr[i]);
                    }
                }

                // Print uppercase count
                //Test: Opperating as expected
                Console.WriteLine($"Uppercase Count: {upper}");

                // Split camelCase
                string splitText = Regex.Replace(Regex.Replace(text, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");

                // Array of words in text
                string[] wordArr = splitText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                // Dictionary to store words and their counts
                Dictionary<string, int> dic1 = new Dictionary<string, int>();

                // Dictionary to store prefixes and their counts
                Dictionary<string, int> dic2 = new Dictionary<string, int>();

                // Iterate over each word
                foreach (string s in wordArr)
                {
                    // Increment dic1
                    if (dic1.ContainsKey(s))
                        dic1[s] += 1;
                    if (!dic1.ContainsKey(s))
                        dic1.Add(s, 1);
                    // Increment dic2
                    if (s.Length > 2)
                    {
                        if (dic2.ContainsKey(s.Substring(0, 2)))
                            dic2[s.Substring(0, 2)] += 1;
                        else
                            dic2.Add(s.Substring(0, 2), 1);
                    }


                }

                // Print most common word and count
                //Test: Operating as expected
                int max = dic1.Values.Max();
                foreach (KeyValuePair<string, int> kvp in dic1)
                {
                  if (kvp.Value == max)
                    {
                      Console.WriteLine($"Most Common Word: {kvp.Key}  Frequency: {max}");
                      break;
                    }
                }

                // String to store most common Prefix
                string maxPrefix = null;

                // Print most common prefix and count
                //Test: Operating as expected
                int maxP = dic2.Values.Max();
                foreach (KeyValuePair<string, int> kvp in dic2)
                {
                    if (kvp.Value == maxP)
                    {
                        maxPrefix = kvp.Key;
                        Console.WriteLine($"Most Common Prefix: {maxPrefix}  Frequency: {maxP}");
                        break;
                    }
                }

                // List to store words with identified max prefix
                List<string> prefixWords = new List<string>();

                //Find words with max prefix
                foreach (string s in wordArr)
                {
                    if (s.Length > 2 && s.Substring(0, 2) == maxPrefix)
                    {
                        prefixWords.Add(s);
                    }
                }

                // Print words with max prefix comma separated
                //Test: Operating as expected
                Console.WriteLine($"The prefix '{maxPrefix}' appears in the following words: { String.Join(", ", prefixWords.ToArray())}");


            }
        }


}
