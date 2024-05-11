using Search_for_RiPD.Model;
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

namespace Search_for_RiPD.View
{
    /// <summary>
    /// Логика взаимодействия для ReportsWindow.xaml
    /// </summary>
    public partial class ReportsWindow : Window
    {

        private readonly Window _userpageWindow;

        public ReportsWindow()
        {
            InitializeComponent();
        }
        
        public ReportsWindow( Window userpageWindow, string login )
        {
            InitializeComponent();
            _userpageWindow = userpageWindow;
            ApplicationContext bd = new ApplicationContext();
            List<Report> reports = bd.Reports.Where(r => r.Login == login).ToList();
            listOfReports.ItemsSource = reports;
        }

        private void BackNavigation_Button_Click(object sender, RoutedEventArgs e)
        {
            _userpageWindow.Show();
            Hide();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
