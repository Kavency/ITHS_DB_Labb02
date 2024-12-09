using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TheBookNook_WPF.DBModel
{
    class TheBookNookDB : DbContext
    {
        public TheBookNookDB(DbContextOptions<TheBookNookDB> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=TheBookNookDB;Trusted_Connection=True;");
            }
        }
    }
}
