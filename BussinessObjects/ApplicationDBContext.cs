using BussinessObjects.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser,ApplicationRole,int>
    {
        public ApplicationDBContext()
        {
            
        }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true);
            IConfigurationRoot configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
            // Bỏ tiền tố AspNet của các bảng: mặc định các bảng trong IdentityDbContext có
            // tên với tiền tố AspNet như: AspNetUserRoles, AspNetUser ...
            // Đoạn mã sau chạy khi khởi tạo DbContext, tạo database sẽ loại bỏ tiền tố đó
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));

                }
            }















            //modelBuilder.Entity<Role>().HasData(new List<Role>()
            //{
            //    new Role {RoleId = 1, RoleName = "Admin"},
            //    new Role {RoleId = 2, RoleName = "Customer"},
            //    new Role {RoleId = 3, RoleName = "Guest"}
            //});
            //modelBuilder.Entity<User>().HasData(new List<User>()
            //{
            //    new User {UserId = 1, Password="111102",RoleId=1,Email="luongfelix14@gmail.com", Name="Đức Lương Admin"},
            //});

            //// Cấu hình cho bảng Role
            //modelBuilder.Entity<Role>()
            //    .HasKey(r => r.RoleId);

            //// Cấu hình cho bảng User
            //modelBuilder.Entity<User>()
            //    .HasKey(u => u.UserId);

            //modelBuilder.Entity<User>()
            //    .HasOne(u => u.Role)
            //    .WithMany()
            //    .HasForeignKey(u => u.RoleId);

            //// Cấu hình cho bảng UserDetail
            //modelBuilder.Entity<UserDetail>()
            //    .HasKey(ud => ud.Id);

            //modelBuilder.Entity<UserDetail>()
            //    .HasOne(ud => ud.User)
            //    .WithOne(u => u.UserDetail)
            //    .HasForeignKey<UserDetail>(ud => ud.UserId);

            //// Cấu hình cho bảng Product
            //modelBuilder.Entity<Product>()
            //    .HasKey(p => p.ProductId);

            //modelBuilder.Entity<Product>()
            //    .HasOne(p => p.Category)
            //    .WithMany(c => c.Products)
            //    .HasForeignKey(p => p.CategoryId);

            //// Cấu hình cho bảng ProductImage
            //modelBuilder.Entity<ProductImage>()
            //    .HasKey(pi => pi.ProductImageId);

            //modelBuilder.Entity<ProductImage>()
            //    .HasOne(pi => pi.Product)
            //    .WithMany(p => p.ProductImages)
            //    .HasForeignKey(pi => pi.ProductId);

            //// Cấu hình cho bảng Comment
            //modelBuilder.Entity<Comment>()
            //    .HasKey(cm => cm.CommentId);

            //modelBuilder.Entity<Comment>()
            //    .HasOne(cm => cm.User)
            //    .WithMany(u => u.Comments)
            //    .HasForeignKey(cm => cm.UserId);

            //modelBuilder.Entity<Comment>()
            //    .HasOne(cm => cm.Product)
            //    .WithMany(p => p.Comments)
            //    .HasForeignKey(cm => cm.ProductId);

            //// Cấu hình cho bảng Rating
            //modelBuilder.Entity<Rating>()
            //    .HasKey(rt => rt.RatingId);

            //modelBuilder.Entity<Rating>()
            //    .HasOne(rt => rt.User)
            //    .WithMany(u => u.Ratings)
            //    .HasForeignKey(rt => rt.UserId);

            //modelBuilder.Entity<Rating>()
            //    .HasOne(rt => rt.Product)
            //    .WithMany(p => p.Ratings)
            //    .HasForeignKey(rt => rt.ProductId);

            //// Cấu hình cho bảng Category
            //modelBuilder.Entity<Category>()
            //    .HasKey(c => c.CategoryId);

            //// Cấu hình cho bảng Order
            //modelBuilder.Entity<Order>()
            //    .HasKey(o => o.OrderId);

            //modelBuilder.Entity<Order>()
            //    .HasOne(o => o.User)
            //    .WithMany(u => u.Orders)
            //    .HasForeignKey(o => o.UserId);

            //modelBuilder.Entity<Order>()
            //    .HasOne(o => o.Discount)
            //    .WithMany(d => d.Orders)
            //    .HasForeignKey(o => o.DiscountId);

            //// Cấu hình cho bảng OrderDetail
            //modelBuilder.Entity<OrderDetail>()
            //    .HasKey(od => od.OrderDetailId);

            //modelBuilder.Entity<OrderDetail>()
            //    .HasOne(od => od.Order)
            //    .WithMany(o => o.OrderDetails)
            //    .HasForeignKey(od => od.OrderId);

            //modelBuilder.Entity<OrderDetail>()
            //    .HasOne(od => od.Product)
            //    .WithMany(p => p.OrderDetails)
            //    .HasForeignKey(od => od.ProductId);

            //// Cấu hình cho bảng Favorite
            //modelBuilder.Entity<Favorite>()
            //    .HasKey(f => f.FavoriteId);

            //modelBuilder.Entity<Favorite>()
            //    .HasOne(f => f.User)
            //    .WithMany(u => u.Favorites)
            //    .HasForeignKey(f => f.UserId);

            //modelBuilder.Entity<Favorite>()
            //    .HasOne(f => f.Product)
            //    .WithMany(p => p.Favorites)
            //    .HasForeignKey(f => f.ProductId);

            //// Cấu hình cho bảng Blog
            //modelBuilder.Entity<Blog>()
            //    .HasKey(b => b.BlogId);

            //modelBuilder.Entity<Blog>()
            //    .HasOne(b => b.User)
            //    .WithMany(u => u.Blogs)
            //    .HasForeignKey(b => b.UserId);

            //// Cấu hình cho bảng Discount
            //modelBuilder.Entity<Discount>()
            //    .HasKey(d => d.DiscountId);


        }
    }
}
