using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PilzGPTArge_V0.Models.Database;

public partial class PilzGptContext : DbContext
{
    public PilzGptContext()
    {
    }

    public PilzGptContext(DbContextOptions<PilzGptContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Model> Models { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-TI0POED;Database=PilzGPT;Trusted_Connection=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>(entity =>
        {
            entity.Property(e => e.ChangedDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.Property(e => e.ImageFormat)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.MessageImage).HasColumnType("image");
            entity.Property(e => e.MessageType)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.SendDate).HasColumnType("datetime");

            entity.HasOne(d => d.Chat).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ChatId)
                .HasConstraintName("FK_Messages_Chats");

            entity.HasOne(d => d.Model).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ModelId)
                .HasConstraintName("FK_Messages_Models");

            entity.HasOne(d => d.Role).WithMany(p => p.Messages)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_Messages_Roles");
        });

        modelBuilder.Entity<Model>(entity =>
        {
            entity.Property(e => e.ModelName).HasMaxLength(30);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Role1)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
