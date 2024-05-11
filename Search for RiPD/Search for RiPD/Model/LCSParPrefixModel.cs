using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Search_for_RiPD.Model
{
    public class LCSParPrefixModel
    {
        private ListBox reportListBox;
        public LCSParPrefixModel() { }

        ApplicationContext bd = new ApplicationContext();
        private string _login;
        private string filepath;

        public LCSParPrefixModel(ListBox reportListBox, string login, string filepath)
        {
            this.reportListBox = reportListBox;
            this._login = login;
            this.filepath = filepath;
        }

        public void PrefixSALCS(List<string> codeLines)
        {
            reportListBox.Items.Add(@"{report}:");

            List<string> duplicates = new List<string>();

            int[,] lcsLengths = new int[codeLines.Count, codeLines.Count];

            for (int i = 0; i < codeLines.Count; i++)
            {
                string line1 = codeLines[i];
                if (string.IsNullOrWhiteSpace(line1) || line1.Trim() == "{" || line1.Trim() == "}")
                {
                    continue;
                }

                for (int j = i + 1; j < codeLines.Count; j++)
                {
                    string line2 = codeLines[j];
                    if (string.IsNullOrWhiteSpace(line2) || line2.Trim() == "{" || line2.Trim() == "}")
                    {
                        continue;
                    }

                    int lcsLength = ComputeLCSLength(line1, line2);
                    lcsLengths[i, j] = lcsLength;
                    lcsLengths[j, i] = lcsLength;
                }
            }

            int[] maxLCSLengths = new int[codeLines.Count];
            for (int i = 0; i < codeLines.Count; i++)
            {
                int maxLCS = 0;
                for (int j = 0; j < codeLines.Count; j++)
                {
                    if (lcsLengths[i, j] > maxLCS)
                    {
                        maxLCS = lcsLengths[i, j];
                    }
                }
                maxLCSLengths[i] = maxLCS;
            }

            for (int i = 0; i < codeLines.Count; i++)
            {
                if (maxLCSLengths[i] > 0)
                {
                    for (int j = i + 1; j < codeLines.Count; j++)
                    {
                        if (lcsLengths[i, j] == maxLCSLengths[i])
                        {
                            string duplicate = $"Схожі рядки знайдено: \"{codeLines[i]}\" (рядок {i + 1}) и \"{codeLines[j]}\" (рядок {j + 1})";
                            duplicates.Add(duplicate);
                        }
                    }
                }
            }

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

        private int ComputeLCSLength(string s1, string s2)
        {
            int m = s1.Length;
            int n = s2.Length;

            //Створюємо двовимірний масив для зберігання довжин LCS для кожного префікса
            int[,] dp = new int[m + 1, n + 1];

            //Заповнюємо масив значеннями довжин LCS
            for (int i = 0; i <= m; i++)
            {
                for (int j = 0; j <= n; j++)
                {
                    if (i == 0 || j == 0)
                    {
                        dp[i, j] = 0;
                    }
                    else if (s1[i - 1] == s2[j - 1])
                    {
                        dp[i, j] = dp[i - 1, j - 1] + 1;
                    }
                    else
                    {
                        dp[i, j] = Math.Max(dp[i - 1, j], dp[i, j - 1]);
                    }
                }
            }

            return dp[m, n];
        }

    }
}
