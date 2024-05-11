using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Search_for_RiPD.Model
{
    public class KMPAnalizModel
    {
        private ListBox reportListBox;

        ApplicationContext bd = new ApplicationContext();
        private string _login;
        private string filepath;
        public KMPAnalizModel() { }

        public KMPAnalizModel(ListBox reportListBox,string login, string filepath)
        {
            this.reportListBox = reportListBox;
            this._login = login;
            this.filepath = filepath;
        }

        public void KMPSearch(List<string> codeLines)
        {
            reportListBox.Items.Add(@"{report}:");

            List<string> duplicates = new List<string>();

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

                for (int j = i + 1; j < codeLines.Count; j++)
                {
                    string nextLine = codeLines[j];
                    if (string.IsNullOrWhiteSpace(nextLine) || nextLine.Trim() == "{" || nextLine.Trim() == "}")
                    {
                        continue;
                    }

                    if (KMPSearchHelper(line, nextLine))
                    {
                        string formattedDuplicate = $"Схожі рядки знайдено: \"{line}\" (рядок {i + 1}) и \"{nextLine}\" (рядок {j + 1})";
                        duplicates.Add(formattedDuplicate);
                    }
                }
            }

            // Виводимо знайдені дублікати
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

        private bool KMPSearchHelper(string pattern, string text)
        {
            // Реалізація алгоритму Кнута-Морріса-Пратта
            int m = pattern.Length;
            int n = text.Length;

            int[] lps = ComputeLPSArray(pattern);

            int i = 0; // Індекс для text
            int j = 0; // Індекс для pattern

            while (i < n)
            {
                if (pattern[j] == text[i])
                {
                    j++;
                    i++;
                }

                if (j == m)
                {
                    // Підрядок знайдено в тексті
                    return true;
                }
                else if (i < n && pattern[j] != text[i])
                {
                    if (j != 0)
                    {
                        j = lps[j - 1];
                    }
                    else
                    {
                        i++;
                    }
                }
            }
            return false;
        }

        private int[] ComputeLPSArray(string pattern)
        {
            // Реалізація функції для обчислення масиву найбільших префіксів і суфіксів (LPS) для алгоритму KMP
            int m = pattern.Length;
            int[] lps = new int[m];
            int len = 0; // Довжина попереднього найдовшого префікса-суфікса

            lps[0] = 0;
            int i = 1;

            while (i < m)
            {
                if (pattern[i] == pattern[len])
                {
                    len++;
                    lps[i] = len;
                    i++;
                }
                else
                {
                    if (len != 0)
                    {
                        len = lps[len - 1];
                    }
                    else
                    {
                        lps[i] = 0;
                        i++;
                    }
                }
            }
            return lps;
        }

    }
}
