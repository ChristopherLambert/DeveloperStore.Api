using Microsoft.EntityFrameworkCore;
using DeveloperStore.Domain.Entities;

public class DeveloperStoreContext : DbContext
{
    public DeveloperStoreContext(DbContextOptions<DeveloperStoreContext> options)
        : base(options) { }

    // DbSets representando as tabelas do banco de dados
    public DbSet<Sale> Sales { get; set; }
    public DbSet<SaleItem> SaleItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração da relação one-to-many: Sale -> SaleItems
        modelBuilder.Entity<SaleItem>()
            .HasOne(si => si.Sale)           
            .WithMany(s => s.Items)          // Cada Sale tem muitos SaleItems
            .HasForeignKey(si => si.SaleId)  // Chave estrangeira em SaleItem
            .OnDelete(DeleteBehavior.Cascade);

        // Outras configurações de mapeamento (se necessário):
        // - Por exemplo, configurar propriedades de desconto ou índices únicos, etc.
        // modelBuilder.Entity<Sale>().Property(s => s.TotalComDesconto).HasPrecision(18,2);
    }
}
