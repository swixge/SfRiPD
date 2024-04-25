using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Search_for_RiPD
{
    class ApplicationContext : DbContext
    {

        public DbSet<User> Users { get; set; }

        public ApplicationContext() : base("DefaultConnection") { } 


    }
}
