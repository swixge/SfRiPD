using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Search_for_RiPD.Model
{
    public class Report
    {

        public Report() { }
        public Report(string login, string report_text)
        {
            this.login = login;
            this.report_text = report_text;         
        }

        [Key]
        public int report_id { get; set; }

        private string login;

        private string report_text;

        public string Login
        {
            get { return login; }
            set { login = value; }
        }

        public string Report_text
        {
            get { return report_text; }
            set { report_text = value; }
        }

        public override string ToString()
        {
                return "Звіт:  \n" + report_text;

        }
    }
}
