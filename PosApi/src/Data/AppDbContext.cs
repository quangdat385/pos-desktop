using Microsoft.EntityFrameworkCore;
using PosApi.Entities;
namespace PosApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Table name
                entity.SetTableName(ToSnakeCase(entity.GetTableName() ?? string.Empty));

                // Column names
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(ToSnakeCase(property.Name ?? string.Empty));
                }
            }

            // Hàm chuyển camelCase/PascalCase sang snake_case
            static string ToSnakeCase(string input)
            {
                return string.Concat(
                    input.Select((x, i) =>
                        i > 0 && char.IsUpper(x) ? "_" + x : x.ToString()
                    )
                ).ToLower();
            }
        }
        public DbSet<Product> Products
        {
            get; set;
        } = null!;
        public DbSet<Order> Orders
        {
            get; set;
        } = null!;
        public DbSet<OrderItem> OrderItems
        {
            get; set;
        } = null!;
    }
}
