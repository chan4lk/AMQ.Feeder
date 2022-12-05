using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace AMQ.Generator
{
    public class FilesGenerator
    {
        public void GenerateFrom(string src, int fileCount)
        {
            var lines = File.ReadAllLines(src);
            var length = lines.Length;
            var newLines = new string[length];

            for (int i = 0; i < fileCount; i++)
            {
                var replacements = new Dictionary<string, object> {
                    { "Number3", Seed(963315, i) },
                    { "ReferenceId", GenerateReferenceId(i) },
                    { "Number1", Seed(9150822, i) },
                    { "Number2", Seed(636013073, i) },
                    { "Name", Seed(3, i, "MTEST") },
                    { "3digitdecimal", GenerateNumberWithDecimal(1699, i) },
                };
                var destFile = $"templates/replaced_{i}.txt";
                WriteFile(destFile, replacements, lines, length, newLines);
            }

            
        }

        private string GenerateNumberWithDecimal(double root, int i)
        {           
            return ((root + i) / 10).ToString("0.##");
        }

        private string GenerateReferenceId(int i)
        {
            var root = 1657 + i +"16536lvp4x";
            return root;
        }

        private string Seed(int root, int i, string prefix = "")
        {
            return prefix + (root + i).ToString();
        }

        private void WriteFile(string dest, Dictionary<string, object> replacements, string[] lines, int length, string[] newLines)
        {
            for (int i = 0; i < length; i++)
            {
                var newLine = ReplaceUsingDictionary(lines[i], replacements);
                newLines[i] = newLine;
            }

            File.WriteAllLines(dest, newLines);
        }

        private string ReplaceUsingDictionary(string src, IDictionary<string, object> replacements)
        {
            return Regex.Replace(src, @"#{(\w+)}", (m) => {
                object replacement;
                var key = m.Groups[1].Value;
                if (replacements.TryGetValue(key, out replacement))
                {
                    return Convert.ToString(replacement);
                }
                else
                {
                    return m.Groups[0].Value;
                }
            });
        }
    }
}