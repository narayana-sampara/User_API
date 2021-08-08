using System;
using Microsoft.EntityFrameworkCore;
using User_API.Models;

namespace User_API.Data
{
    public class SqliteContext : DbContext
    {
        public SqliteContext(DbContextOptions<SqliteContext> options)
            : base(options)
        {
        }
        public DbSet<UserModel> UserModel { get; set; }
    }
}

