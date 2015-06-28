using System.Data.Entity.ModelConfiguration.Conventions;
using FindTheWay.Data.DbModels;
using System.Data.Entity;

namespace FindTheWay.Data
{
    public class FindTheWayDbContext : DbContext
    {
        public DbSet<Node> Nodes { get; set; }

        public DbSet<Edge> Edges { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Edge>()
                .HasRequired<Node>(p => p.NodeFrom)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Edge>()
                .HasRequired<Node>(p => p.NodeTo)
                .WithMany()
                .WillCascadeOnDelete(false);
        }
    }
}
