using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Agems
{
    public partial class AgemsSoundsContext : DbContext
    {
        public AgemsSoundsContext()
        {
        }

        public AgemsSoundsContext(DbContextOptions<AgemsSoundsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Sound> Sounds { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=agems.database.windows.net; Database=AgemsSounds; User ID=azureuser; Password=sdlifh!7e82");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.Text)
                    .IsFixedLength();

                entity.Property(e => e.Author)
                    .IsFixedLength();

                entity.HasOne(d => d.Sound)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.SoundId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Comments_Sounds");          
            });

            modelBuilder.Entity<Sound>(entity =>
            {
                entity.Property(e => e.About)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .IsFixedLength();

                entity.Property(e => e.Author)
                    .IsFixedLength();

                entity.Property(e => e.SoundPath)
                    .IsFixedLength();

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Sounds)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Sounds_Categories");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
