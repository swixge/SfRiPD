using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace Search_for_RiPD
{
    /// <summary>
    /// Логика взаимодействия для UserPageWindow.xaml
    /// </summary>
    public partial class UserPageWindow : Window
    {
        string auth_user_login;
        string auth_user_login_view;
        public UserPageWindow(string login)
        {
            InitializeComponent();
            this.auth_user_login = login;
            ApplicationContext bd = new ApplicationContext();
            auth_user_login_view = "User: " + auth_user_login;
            User_Acc_View.Text = auth_user_login_view;

        }

        private void Button_LoadFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    lstFiles.Items.Add(filename);
                }
            }
        }
    }
}
