using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace Search_for_RiPD.Model
{
    public class SemiLocalLCSModel
    {
        public SemiLocalLCSModel() { }

        private List<string> codeLines;
        private int threshold;
        private ListBox reportListBox;
        private string filepath;

        ApplicationContext bd = new ApplicationContext();//
        private string _login;//
        public SemiLocalLCSModel(List<string> codeLines, int threshold, ListBox reportListBox, string login, string filepath )
        {
            this.codeLines = codeLines;
            this.threshold = threshold;
            this.reportListBox = reportListBox;
            this._login = login;
            this.filepath = filepath;
        }

        public void FindAndReportDuplicates()
        {



            Dictionary<string, int> lineCounts = new Dictionary<string, int>();

            // Рахуємо, скільки разів кожен рядок зустрічається в коді
            foreach (string line in codeLines)
            {
                // Ігноруємо пробіли та дужки під час підрахунку повторень
                string cleanedLine = line.Replace(" ", "").Replace("{", "").Replace("}", "");
                if (!lineCounts.ContainsKey(cleanedLine))
                    lineCounts[cleanedLine] = 1;
                else
                    lineCounts[cleanedLine]++;
            }

            StringBuilder reportBuilder = new StringBuilder();

            // Формуємо звіт про знайдені повтори
            foreach (var pair in lineCounts)
            {
                if (pair.Value > 1)
                {
                    reportBuilder.AppendLine($"Рядок \"{pair.Key}\" зустрічається {pair.Value} рази.");
                }
            }

            // Виводимо звіт
            if (reportBuilder.Length > 0)
            {
                reportListBox.Items.Clear();
                foreach (var pair in lineCounts)
                {
                    if (pair.Value > 1)
                    {
                        reportListBox.Items.Add($"Рядок \"{pair.Key}\" зустрічається {pair.Value} рази.");
                    }
                }
            }
            else
            {
                reportListBox.Items.Add("Рядків, що повторюються, не знайдено.");
            }
        }

        private int SemiLocalLCS(string s1, string s2, int m, int n, int[,] dp)
        {
            if (m == 0 || n == 0)
                return 0;

            if (dp[m, n] != -1)
                return dp[m, n];

            if (s1[m - 1] == s2[n - 1])
                return dp[m, n] = 1 + SemiLocalLCS(s1, s2, m - 1, n - 1, dp);

            return dp[m, n] = Math.Max(SemiLocalLCS(s1, s2, m, n - 1, dp), SemiLocalLCS(s1, s2, m - 1, n, dp));
        }

        private int FindLongestCommonSubsequence(string s1, string s2)
        {
            int[,] dp = new int[s1.Length + 1, s2.Length + 1];
            for (int i = 0; i <= s1.Length; i++)
            {
                for (int j = 0; j <= s2.Length; j++)
                {
                    dp[i, j] = -1;
                }
            }

            return SemiLocalLCS(s1, s2, s1.Length, s2.Length, dp);
        }

        public void  FindAndReportDuplicatesAdvanced()
        {

            StringBuilder reportBuilder = new StringBuilder();

            for (int i = 0; i < codeLines.Count; i++)
            {
                for (int j = i + 1; j < codeLines.Count; j++)
                {
                    int similarity = FindLongestCommonSubsequence(codeLines[i], codeLines[j]);

                    if (similarity > threshold) // Встановлюємо поріг схожості
                    {
                        reportBuilder.AppendLine($"Схожі рядки знайдено: \"{codeLines[i]}\" (рядок {i + 1}) и \"{codeLines[j]}\" (рядок {j + 1}). Довжина загальної підпослідовності: {similarity}");
                    }
                }
            }

            if (reportBuilder.Length > 0)
            {
                
                reportListBox.Items.Add("\n" + "Файл: " + filepath + "\n{report}:");
                for (int i = 0; i < codeLines.Count; i++)
                {
                    for (int j = i + 1; j < codeLines.Count; j++)
                    {
                        int similarity = FindLongestCommonSubsequence(codeLines[i], codeLines[j]);

                        if (similarity > threshold)
                        {
                            reportListBox.Items.Add($"Схожі рядки знайдено: \"{codeLines[i]}\" (рядок {i + 1}) и \"{codeLines[j]}\" (рядок {j + 1}). Довжина загальної підпослідовності: {similarity}");
                        }
                    }
                }
            }
            else
            {
                reportListBox.Items.Add("\n" + "Файл: " + filepath + "\n{report}: \nРядків, що повторюються, не знайдено.");
            }


            string reportText = $"{filepath}: \n {reportBuilder.ToString()}";
            Report report = new Report(_login, reportText);//
            bd.Reports.Add(report);//
            bd.SaveChanges();//
        }
    }
}
