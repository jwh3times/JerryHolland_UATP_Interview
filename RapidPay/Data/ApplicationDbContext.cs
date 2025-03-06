using Microsoft.EntityFrameworkCore;
using RapidPay.Models;

namespace RapidPay.Data
{
    /// <summary>
    /// Represents the database context for the application, 
    /// providing access to the Cards, Transactions, AuthorizationLogs, and FeeHistories tables.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options to be used by the DbContext.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Card> Cards { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<AuthorizationLog> AuthorizationLogs { get; set; }
        public DbSet<FeeHistory> FeeHistories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CardUpdateLog> CardUpdateLogs { get; set; }

        /// <summary>
        /// Configures the schema needed for the RapidPay application.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                            .SelectMany(t => t.GetProperties())
                            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }

            base.OnModelCreating(modelBuilder);

            // Configure schema for each entity
            modelBuilder.Entity<Card>().ToTable("Cards", "RapidPay");
            modelBuilder.Entity<Transaction>().ToTable("Transactions", "RapidPay");
            modelBuilder.Entity<AuthorizationLog>().ToTable("AuthorizationLogs", "RapidPay");
            modelBuilder.Entity<FeeHistory>().ToTable("FeeHistories", "RapidPay");
            modelBuilder.Entity<User>().ToTable("Users", "RapidPay");
            modelBuilder.Entity<CardUpdateLog>().ToTable("CardUpdateLogs", "RapidPay");
        }
    }
}