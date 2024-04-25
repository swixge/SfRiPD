using ControlzEx.Standard;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Web.UI.WebControls;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows;
using System.Xml.Linq;

namespace Search_for_RiPD
{
    class User
    {
        public int id { get; set; }

        private string login, pass, email;

        public string Login
        {
            get { return login; }
            set { login = value; }
        }

        public string Pass
        {
            get { return pass; }
            set { pass = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public User() { }

        public User(string login, string email, string pass)
        {
            this.login = login;
            this.pass = pass;
            this.email = email;
        }


    }
}
