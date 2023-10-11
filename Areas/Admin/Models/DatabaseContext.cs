using System;
using Clothing_boutique_web.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Clothing_boutique_web.Models
{
    public partial class DatabaseContext : DbContext
    {
       
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
            
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RoleAccount> RoleAccounts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.Email)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FullName).HasMaxLength(250);

                entity.Property(e => e.Password)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RoleAccount>(entity =>
            {
                entity.HasKey(e => new { e.RoleId, e.AccountId });

                entity.ToTable("RoleAccount");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.RoleAccounts)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleAccount_Account");

                entity.HasOne(d => d.Roles)
                    .WithMany(p => p.RoleAccounts)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleAccount_Role");
            });

            //modelBuilder.Entity<Product>(entity =>
            //{
            //    entity.ToTable("Product");

            //    entity.Property(e => e.Name)
            //        .HasMaxLength(250)
            //        .IsUnicode(false);

            //    entity.Property(e => e.Description).HasMaxLength(500);

            //    entity.Property(e => e.Detail)
            //        .HasMaxLength(1000)
            //        .IsUnicode(false);
            //    entity.HasOne(d => d.Categories)
            //        .WithMany(p => p.Products)
            //        .HasForeignKey(d => d.CategoryId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_Product_Category");
            //});

            //modelBuilder.Entity<Category>(entity =>
            //{
            //    entity.ToTable("Category");

            //    entity.Property(e => e.Name)
            //        .HasMaxLength(250)
            //        .IsUnicode(false);
            //});
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
