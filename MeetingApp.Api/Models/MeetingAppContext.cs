using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MeetingApp.Api.Models
{
    public partial class MeetingAppContext : DbContext
    {
        public MeetingAppContext()
        {
        }

        public MeetingAppContext(DbContextOptions<MeetingAppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Meeting> Meeting { get; set; }
        public virtual DbSet<MeetingLabel> MeetingLabel { get; set; }
        public virtual DbSet<MeetingLabelItem> MeetingLabelItem { get; set; }
        public virtual DbSet<Token> Token { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:meeting-app.database.windows.net,1433;Initial Catalog=MeetingApp;Persist Security Info=False;User ID=matsumoto_ts;Password='!283kb4;';MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Meeting>(entity =>
            {
                entity.ToTable("meeting");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EndDatetime)
                    .HasColumnName("end_datetime")
                    .HasColumnType("datetime");

                entity.Property(e => e.Isvisible).HasColumnName("isvisible");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasColumnName("location")
                    .HasMaxLength(50);

                entity.Property(e => e.Owner).HasColumnName("owner");

                entity.Property(e => e.Regular).HasColumnName("regular");

                entity.Property(e => e.StartDatetime)
                    .HasColumnName("start_datetime")
                    .HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MeetingLabel>(entity =>
            {
                entity.ToTable("meetingLabel");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LabelName)
                    .IsRequired()
                    .HasColumnName("label_name")
                    .HasMaxLength(50);

                entity.Property(e => e.Mid).HasColumnName("mid");
            });

            modelBuilder.Entity<MeetingLabelItem>(entity =>
            {
                entity.ToTable("meetingLabelItem");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ItemName)
                    .IsRequired()
                    .HasColumnName("item_name")
                    .HasMaxLength(50);

                entity.Property(e => e.Lid).HasColumnName("lid");

                entity.Property(e => e.Uid).HasColumnName("uid");
            });

            modelBuilder.Entity<Token>(entity =>
            {
                entity.ToTable("token");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EndTime)
                    .HasColumnName("endTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.StartTime)
                    .HasColumnName("startTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.TokenText)
                    .IsRequired()
                    .HasColumnName("tokenText")
                    .HasMaxLength(64)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasIndex(e => e.UserId)
                    .HasName("Unique_userId")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("userId")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
