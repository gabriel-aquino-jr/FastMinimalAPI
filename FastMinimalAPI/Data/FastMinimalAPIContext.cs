namespace FastMinimalAPI.Data
{
    public class FastMinimalAPIContext : DbContext
    {
        public FastMinimalAPIContext (DbContextOptions<FastMinimalAPIContext> options)
            : base(options)
        {
        }

        public DbSet<FastMinimalAPI.Models.Hero> Hero { get; set; } = default!;
    }
}
