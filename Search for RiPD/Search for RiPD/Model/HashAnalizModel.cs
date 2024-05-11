using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;


namespace Search_for_RiPD.Model
{
    public class HashAnalizModel
    {
        ApplicationContext bd = new ApplicationContext();//
        private string _login;//
        private ListBox reportListBox;
        private string filepath; 
        public HashAnalizModel() { }

        public HashAnalizModel(ListBox reportListBox, string login, string filepath) //
        {
            this.reportListBox = reportListBox;
            this._login = login;//
            this.filepath = filepath;
        }

        public void HashMethod(List<string> codeLines)
        {

            reportListBox.Items.Add(@"{report}:");

            Dictionary<string, List<int>> hashToLinesMap = GenerateHashToLinesMap(codeLines);
            List<string> duplicates = FindDuplicates(hashToLinesMap, codeLines);

            if (duplicates.Count > 0)
            {
                foreach (string duplicate in duplicates)
                {
                    reportListBox.Items.Add(duplicate);
                }
            }
            else
            {
                reportListBox.Items.Add("Дублікатів коду не знайдено.");
            }


            StringBuilder reportTextBuilder = new StringBuilder();

            foreach (var duplicate in duplicates)
            {
                reportTextBuilder.AppendLine(duplicate);
            }

            string reportText = $"{filepath}: \n {reportTextBuilder.ToString()}";
            Report report = new Report(_login, reportText);//
            bd.Reports.Add(report);//
            bd.SaveChanges();// 

        }

        private Dictionary<string, List<int>> GenerateHashToLinesMap(List<string> codeLines)
        {
            var hashToLinesMap = new Dictionary<string, List<int>>();

            for (int i = 0; i < codeLines.Count; i++)
            {
                string line = codeLines[i];

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (line.Trim() == "{" || line.Trim() == "}")
                {
                    continue;
                }

                string hash = ComputeHash(NormalizeLine(line));

                if (!hashToLinesMap.ContainsKey(hash))
                {
                    hashToLinesMap[hash] = new List<int>();
                }

                hashToLinesMap[hash].Add(i + 1);
            }

            return hashToLinesMap;
        }

        private string ComputeHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder stringBuilder = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    stringBuilder.Append(hashBytes[i].ToString("X2"));
                }

                return stringBuilder.ToString();
            }
        }

        private List<string> FindDuplicates(Dictionary<string, List<int>> hashToLinesMap, List<string> codeLines)
        {
            var duplicates = new List<string>();

            foreach (var kvp in hashToLinesMap)
            {
                if (kvp.Value.Count > 1)
                {
                    string hash = kvp.Key;
                    List<int> lineNumbers = kvp.Value;

                    StringBuilder duplicateInfo = new StringBuilder();
                    duplicateInfo.Append($"Схожі рядки знайдено: Хеш: {hash}");

                    for (int i = 0; i < lineNumbers.Count; i++)
                    {
                        int lineNumber = lineNumbers[i];
                        string line = codeLines[lineNumber - 1];

                        duplicateInfo.Append($" {line} (рядок{lineNumber})");
                        if (i < lineNumbers.Count - 1)
                        {
                            duplicateInfo.Append(" і ");
                        }
                    }

                    duplicates.Add(duplicateInfo.ToString());
                }
            }

            return duplicates;
        }

        private string NormalizeLine(string line)
        {
            line = line.Trim();
            line = Regex.Replace(line, @"\s+", " ");
            line = line.ToLower();

            return line;
        }
    }
}
