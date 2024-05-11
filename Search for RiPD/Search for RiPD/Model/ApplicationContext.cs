using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Search_for_RiPD.Model;

namespace Search_for_RiPD
{
    class ApplicationContext : DbContext
    {

        public ApplicationContext() : base("DefaultConnection") { }

        public DbSet<User> Users { get; set; }

        public DbSet<Report> Reports { get; set; }//

        


    }
}
