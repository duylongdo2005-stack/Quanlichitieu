using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Quan_Li_Chi_Tieu.Models;

public partial class QuanlichitieuContext : DbContext
{
    public QuanlichitieuContext()
    {
    }

    public QuanlichitieuContext(DbContextOptions<QuanlichitieuContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attachment> Attachments { get; set; }

    public virtual DbSet<Budget> Budgets { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(config.GetConnectionString("value"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.HasKey(e => e.AttachmentId).HasName("PK__Attachme__442C64BE75A105A9");

            entity.HasIndex(e => e.TransactionId, "IX_Attachments_TransactionId");

            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.MimeType).HasMaxLength(50);
            entity.Property(e => e.UploadedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Transaction).WithMany(p => p.Attachments)
                .HasForeignKey(d => d.TransactionId)
                .HasConstraintName("FK__Attachmen__Trans__5EBF139D");
        });

        modelBuilder.Entity<Budget>(entity =>
        {
            entity.HasKey(e => e.BudgetId).HasName("PK__Budgets__E38E7924B3C3250B");

            entity.HasIndex(e => e.MonthYear, "IX_Budgets_MonthYear");

            entity.HasIndex(e => e.UserId, "IX_Budgets_UserId");

            entity.HasIndex(e => new { e.UserId, e.CategoryId, e.MonthYear }, "UQ__Budgets__FCF1B0706AF1D80D").IsUnique();

            entity.Property(e => e.AlertThreshold).HasDefaultValue(80);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LimitAmount).HasColumnType("decimal(12, 2)");

            entity.HasOne(d => d.Category).WithMany(p => p.Budgets)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Budgets__Categor__4AB81AF0");

            entity.HasOne(d => d.User).WithMany(p => p.Budgets)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Budgets__UserId__49C3F6B7");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A0BACDCBAD2");

            entity.HasIndex(e => e.UserId, "IX_Categories_UserId");

            entity.HasIndex(e => new { e.UserId, e.CategoryName, e.CategoryType }, "UQ__Categori__47D26B797101BE39").IsUnique();

            entity.Property(e => e.CategoryName).HasMaxLength(50);
            entity.Property(e => e.CategoryType).HasMaxLength(20);
            entity.Property(e => e.Color).HasMaxLength(10);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Categories)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Categorie__UserI__3F466844");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.PaymentMethodId).HasName("PK__PaymentM__DC31C1D33FDA589F");

            entity.HasIndex(e => e.UserId, "IX_PaymentMethods_UserId");

            entity.HasIndex(e => new { e.UserId, e.MethodName }, "UQ__PaymentM__659003FC9390EB4D").IsUnique();

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MethodName).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.PaymentMethods)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__PaymentMe__UserI__440B1D61");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagId).HasName("PK__Tags__657CF9AC0CE65849");

            entity.HasIndex(e => e.UserId, "IX_Tags_UserId");

            entity.HasIndex(e => new { e.UserId, e.TagName }, "UQ__Tags__CC56C39CD9446FA6").IsUnique();

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TagName).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.Tags)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Tags__UserId__4F7CD00D");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A6B534C62F1");

            entity.HasIndex(e => e.CategoryId, "IX_Transactions_CategoryId");

            entity.HasIndex(e => e.TransactionDate, "IX_Transactions_TransactionDate");

            entity.HasIndex(e => e.UserId, "IX_Transactions_UserId");

            entity.Property(e => e.Amount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsRecurring).HasDefaultValue(false);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RecurrencePattern).HasMaxLength(50);
            entity.Property(e => e.TransactionType).HasMaxLength(20);

            entity.HasOne(d => d.Category).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Categ__5629CD9C");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.PaymentMethodId)
                .HasConstraintName("FK__Transacti__Payme__571DF1D5");

            entity.HasOne(d => d.User).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Transacti__UserI__5535A963");

            entity.HasMany(d => d.Tags).WithMany(p => p.Transactions)
                .UsingEntity<Dictionary<string, object>>(
                    "TransactionTag",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Transacti__TagId__5AEE82B9"),
                    l => l.HasOne<Transaction>().WithMany()
                        .HasForeignKey("TransactionId")
                        .HasConstraintName("FK__Transacti__Trans__59FA5E80"),
                    j =>
                    {
                        j.HasKey("TransactionId", "TagId").HasName("PK__Transact__8314F5F160D10D1B");
                        j.ToTable("TransactionTags");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CCEA768E7");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E40BFE9E36").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534BA1EF4E7").IsUnique();

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastLoginDate).HasColumnType("datetime");
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
