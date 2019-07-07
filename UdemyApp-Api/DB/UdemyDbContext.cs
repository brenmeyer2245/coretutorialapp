using Microsoft.EntityFrameworkCore;
using UdemyApp.Models;
namespace UdemyApp.DB {
public class UdemyDbContext : DbContext
  {
    public UdemyDbContext(DbContextOptions<UdemyDbContext> options) : base(options)
{
}
    public DbSet<Values> Values { get; set; }
    public DbSet<User> Users {get; set;}
  }
}
