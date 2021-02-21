using Filed.Services.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Filed.Services
{
    public class PaymentContextFactory : IDesignTimeDbContextFactory<PaymentContext>
    {
        public PaymentContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PaymentContext>();

            IConfigurationBuilder builder = new ConfigurationBuilder();
            var configuration = builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json")).Build();

            var sql = configuration.GetConnectionString("PaymentsConnString");

            optionsBuilder.UseSqlServer(sql);

            return new PaymentContext(optionsBuilder.Options);
        }
    }
}
