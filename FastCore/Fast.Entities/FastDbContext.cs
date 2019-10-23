using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Fast.Entities
{
    public partial class FastDbContext : DbContext
    {
        public FastDbContext()
        {
        }

        public FastDbContext(DbContextOptions<FastDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bucket> Bucket { get; set; }
        public virtual DbSet<BucketCut> BucketCut { get; set; }
        public virtual DbSet<BucketImage> BucketImage { get; set; }
        public virtual DbSet<Sys_ActivityLog> Sys_ActivityLog { get; set; }
        public virtual DbSet<Sys_ActivityLogComment> Sys_ActivityLogComment { get; set; }
        public virtual DbSet<Sys_Category> Sys_Category { get; set; }
        public virtual DbSet<Sys_NLog> Sys_NLog { get; set; }
        public virtual DbSet<Sys_Permission> Sys_Permission { get; set; }
        public virtual DbSet<Sys_Role> Sys_Role { get; set; }
        public virtual DbSet<Sys_Setting> Sys_Setting { get; set; }
        public virtual DbSet<Sys_User> Sys_User { get; set; }
        public virtual DbSet<Sys_UserJwt> Sys_UserJwt { get; set; }
        public virtual DbSet<Sys_UserLogin> Sys_UserLogin { get; set; }
        public virtual DbSet<Sys_UserR> Sys_UserR { get; set; }
        public virtual DbSet<Sys_UserRole> Sys_UserRole { get; set; }
        public virtual DbSet<QuarztSchedule> QuarztSchedule { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bucket>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<BucketCut>(entity =>
            {
                entity.HasComment("图片剪彩配置");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Value).HasComment("w_xx,h_xx配置");
            });

            modelBuilder.Entity<BucketImage>(entity =>
            {
                entity.HasComment("图片存储");

                entity.HasIndex(e => e.SHA1);

                entity.HasIndex(e => e.VisitUrl);

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Sys_ActivityLog>(entity =>
            {
                entity.HasComment("操作日志记录");

                entity.HasIndex(e => e.CreationTime);

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Sys_ActivityLogComment>(entity =>
            {
                entity.HasComment("日志表务业说明表");
            });

            modelBuilder.Entity<Sys_Category>(entity =>
            {
                entity.HasIndex(e => e.UID)
                    .HasName("IX_Sys_Category")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.IsMenu)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Sys_NLog>(entity =>
            {
                entity.HasComment("日志记录表");

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Sys_Permission>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Sys_Role>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).HasComment("描述");
            });

            modelBuilder.Entity<Sys_Setting>(entity =>
            {
                entity.HasIndex(e => e.Name);

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Sys_User>(entity =>
            {
                entity.HasComment("系统用户表");

                entity.HasIndex(e => e.Account);

                entity.HasIndex(e => e.CreationTime);

                entity.HasIndex(e => e.Name);

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreationTime).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Sys_UserJwt>(entity =>
            {
                entity.HasKey(e => e.Jti)
                    .HasName("PK_SysUserJWTToken");

                entity.HasComment("用户登陆jwttoken");

                entity.HasIndex(e => e.Expiration);
            });

            modelBuilder.Entity<Sys_UserLogin>(entity =>
            {
                entity.HasIndex(e => e.LoginTime);

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Sys_UserR>(entity =>
            {
                entity.HasComment("登陆随机数");

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Sys_UserRole>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
