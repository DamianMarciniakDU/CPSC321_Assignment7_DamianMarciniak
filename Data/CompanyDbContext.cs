using CPSC321_Assignment7_DamianMarciniak.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CPSC321_Assignment7_DamianMarciniak.Data
{
    public class CompanyDbContext : DbContext
    {
        public CompanyDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<UserModel> Users { get; set; }
    }
}
