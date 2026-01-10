using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PucharApi.Infrastructure;

public class PucharDbContextFactory : IDesignTimeDbContextFactory<PucharDbContext>
{
  public PucharDbContext CreateDbContext(string[] args)
  {
    var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true)
        .AddJsonFile("appsettings.Development.json", optional: true)
        .AddEnvironmentVariables()
        .Build();

    var cs = config.GetConnectionString("Default") ?? "Data Source=puchar.db";

    var options = new DbContextOptionsBuilder<PucharDbContext>()
        .UseSqlite(cs)
        .Options;

    return new PucharDbContext(options);
  }
}
