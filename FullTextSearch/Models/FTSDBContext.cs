using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FullTextSearch.Models
{
    public class FTSDBContext : DbContext
    {
        public DbSet<Image> Images { get; set; }
        public FTSDBContext(DbContextOptions<FTSDBContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Image>().ToTable("images");
            builder.Entity<Image>().Property(i => i.Id).HasColumnName("images_id").ValueGeneratedOnAdd();
            builder.Entity<Image>().Property(i => i.Title).HasColumnName("title");
            builder.Entity<Image>().Property(i => i.KeyWords).HasColumnName("key_words");
            builder.Entity<Image>().Property(i => i.Description).HasColumnName("description");
            builder.Entity<Image>().Property(i => i.ImageVector).HasColumnName("image_vector");

            base.OnModelCreating(builder);
        }
    }
}
