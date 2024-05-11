using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;
using Search_for_RiPD.Model;
using System.Windows.Controls;
using System.Threading.Tasks;
using Search_for_RiPD.View;

namespace Search_for_RiPD
{
    /// <summary>
    /// Логика взаимодействия для UserPageWindow.xaml
    /// </summary>
    public partial class UserPageWindow : Window
    {
        string auth_user_login;
        string auth_user_login_view;
        public ObservableCollection<string> Files { get; set; }
        public UserPageWindow()
        {
            InitializeComponent();
        }
        public UserPageWindow(string login)
        {
            InitializeComponent();
            this.auth_user_login = login;
            ApplicationContext bd = new ApplicationContext();
            auth_user_login_view = "User: " + auth_user_login;
            User_Acc_View.Text = auth_user_login_view;
            Files = new ObservableCollection<string>();
            DataContext = this;

        }

        private void Button_LoadFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    Files.Add(filename);
                }
            }
        }

        private void MenuItem_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (lstFiles.SelectedItems.Count > 0)
            {
                var selectedItems = lstFiles.SelectedItems.Cast<string>().ToList();
                foreach (var item in selectedItems)
                {
                    Files.Remove(item);
                }
            }
        }

        private void Button_Analiz_files_Click(object sender, RoutedEventArgs e)
        {

            lstReport.Items.Clear();

            if (Files.Count == 0)
            {
                MessageBox.Show("Немає файлів для аналізу.");
                return;
            }
            lstReport.Items.Clear();


            try
            {
                foreach (string filePath in Files)
                {
                    var codeLines = new List<string>(File.ReadAllLines(filePath, Encoding.UTF8));
                    string selectedMethod = ((ComboBoxItem)cmbProcessingMethod.SelectedItem)?.Content.ToString();
                    if (!string.IsNullOrEmpty(selectedMethod))
                    {
                        switch (selectedMethod)
                        {
                            case "Хеш метод":
                                lstReport.Items.Add("\n" + "Файл: " +filePath);
                                var analyzerHash = new HashAnalizModel(lstReport, auth_user_login, filePath);
                                analyzerHash.HashMethod(codeLines);
                                break;
                            case "Метод Кнута-Морриса-Пратта":
                                lstReport.Items.Add("\n" + "Файл: " + filePath);
                                var analyzerKMP = new KMPAnalizModel(lstReport, auth_user_login, filePath);
                                analyzerKMP.KMPSearch(codeLines);
                                break;
                            case "LCS для всіх пар префіксів":
                                lstReport.Items.Add("\n" + "Файл: " + filePath);
                                var analyzerKMPPrefixSALCS = new LCSParPrefixModel(lstReport, auth_user_login, filePath);
                                analyzerKMPPrefixSALCS.PrefixSALCS(codeLines);
                                break;
                            case "Semi-local lcs з використанням липкого множення":
                                var analyzerSemiLocalLCS = new SemiLocalLCSModel(codeLines, Convert.ToInt32(ThresholdTextBox.Text), lstReport, auth_user_login, filePath);
                                analyzerSemiLocalLCS.FindAndReportDuplicatesAdvanced();
                                break;
                            default:
                                MessageBox.Show("Виберіть метод обробки в комбо-боксі.");
                                break;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка під час читання файлу: {ex.Message}");
            }
              
        }

        private void cmbProcessingMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ThresholdTextBox != null && cmbProcessingMethod.SelectedItem != null)
            {
                string selectedItem = ((ComboBoxItem)cmbProcessingMethod.SelectedItem).Content.ToString();
                ThresholdTextBox.IsEnabled = selectedItem == "Semi-local lcs з використанням липкого множення";
            }
        }

        private void Reports_Navigation_Button_Click(object sender, RoutedEventArgs e)
        {
            ReportsWindow reportsWindow = new ReportsWindow(this, auth_user_login);
            reportsWindow.Show();
            Hide();
            
            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }

}
