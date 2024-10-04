using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ECR.Domain.Data {
    public interface IDBContextFactory {
        MDRContext CreateDbContext(string[]? args = null);
    }

    public sealed class DbContextFactory : IDesignTimeDbContextFactory<MDRContext>, IDBContextFactory {

        private static DbContextOptionsBuilder<MDRContext> optionsBuilder = null!;

        public DbContextFactory() {
            optionsBuilder = new();
            string machineName = System.Environment.MachineName;
            var connectionString = $"Server={machineName}\\SQLEXPRESS;Database=MDR;TrustServerCertificate=True;Trusted_Connection=True;";
            optionsBuilder.UseSqlServer(connectionString);
        }

        public MDRContext CreateDbContext(string[]? args = null) {
            return new MDRContext(optionsBuilder.Options);
        }
    }
}
