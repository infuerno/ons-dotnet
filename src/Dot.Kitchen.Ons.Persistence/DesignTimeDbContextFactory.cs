using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace Dot.Kitchen.Ons.Persistence
{
    //// This class is only needed for the migrations while the web root doesn't have the necessary stuff
    //public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    //{

    //    public DatabaseContext CreateDbContext(string[] args)
    //    {
    //        var builder = new ConfigurationBuilder()
    //            .SetBasePath(Directory.GetCurrentDirectory())
    //            .AddJsonFile("appsettings.Development.json");

    //        var configuration = builder.Build();

    //        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
    //        optionsBuilder.UseSqlServer(configuration.GetConnectionString("OnsConnectionString"));

    //        return new DatabaseContext(optionsBuilder.Options);
    //    }
    //}
}