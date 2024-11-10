using BusinessObjects.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BusinessObjects
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public ApplicationDBContext()
        {

        }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }


        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<UserLocation> UserLocations { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<DetailForm> DetailForms { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventParticipants> EventParticipants { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupParticipant> GroupParticipants { get; set; }
        public DbSet<GroupPost> GroupPosts { get; set; }
        public DbSet<GroupPostPhoto> GroupPostPhotos { get; set; }
        public DbSet<HomePhoto> HomePhotos { get; set; }
        public DbSet<Languages> Languages { get; set; }
        public DbSet<Message> Messages { get; set; }

        public DbSet<OnTravelling> OnTravellings { get; set; }
        public DbSet<PastTripPost> PastTripPosts { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
        public DbSet<PostPhoto> PostPhotos { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<SpokenLanguages> SpokenLanguages { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<UserDescription> UserDescriptions { get; set; }
        public DbSet<UserEducation> UserEducations { get; set; }
        public DbSet<UserHome> UserHomes { get; set; }



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
            base.OnModelCreating(modelBuilder); // Ensure base configurations are applied first
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));

                }
            }

            // Thiết lập cascade delete cho quan hệ giữa AspNetUsers và Profiles
            modelBuilder.Entity<ApplicationUser>()
              .HasOne(u => u.Profiles) // Mối quan hệ giữa ApplicationUser và Profile
              .WithOne(p => p.User) // Mỗi Profile liên kết với một ApplicationUser
              .HasForeignKey<Profile>(p => p.UserId) // Khóa ngoại trong Profile
              .OnDelete(DeleteBehavior.Cascade); // Xóa theo chuỗi



            // Thiết lập quan hệ 1-1 giữa ApplicationUser và UserHome
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(a => a.UserHome) // Mỗi ApplicationUser có một UserHome
                .WithOne(uh => uh.ApplicationUser) // Mỗi UserHome có một ApplicationUser
                .HasForeignKey<UserHome>(uh => uh.UserId) // Khóa ngoại trong UserHome
                .OnDelete(DeleteBehavior.Cascade); // Xóa theo chuỗi


            //Cấu hình cho UserLocation/Activity
            modelBuilder.Entity<UserLocation>()
            .HasKey(ul => new { ul.UserId, ul.LocationId });
            modelBuilder.Entity<UserActivity>()
                .HasKey(ua => new { ua.UserId, ua.ActivityId });

            // Thiết lập quan hệ giữa ApplicationUser và Friendship
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.SentFriendRequests)
                .WithOne(f => f.User1)
                .HasForeignKey(f => f.UserId1)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.ReceivedFriendRequests)
                .WithOne(f => f.User2)
                .HasForeignKey(f => f.UserId2)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SpokenLanguages>()
            .HasKey(ul => new { ul.UserId, ul.LanguagesId });
            modelBuilder.Entity<UserEducation>()
                .HasKey(ua => new { ua.UserId, ua.UniversityId });
            modelBuilder.Entity<EventParticipants>()
                .HasKey(ua => new { ua.UserId, ua.EventId });

            // Cấu hình khóa chính và quan hệ cho bảng Friendship
            // modelBuilder.Entity<Friendship>()
            //.HasOne(f => f.User1)
            //.WithMany()
            //.HasForeignKey(f => f.UserId1)
            //.OnDelete(DeleteBehavior.Restrict); // Tránh vòng lặp xóa

            // modelBuilder.Entity<Friendship>()
            //     .HasOne(f => f.User2)
            //     .WithMany()
            //     .HasForeignKey(f => f.UserId2)
            //     .OnDelete(DeleteBehavior.Restrict); // Tránh vòng lặp xóa

            // Seed data cho các role
            modelBuilder.Entity<ApplicationRole>().HasData(
                new ApplicationRole { Id = 1, Name = "admin", NormalizedName = "ADMIN" },
                new ApplicationRole { Id = 2, Name = "user", NormalizedName = "USER" },
                new ApplicationRole { Id = 3, Name = "traveler", NormalizedName = "TRAVELER" },
                new ApplicationRole { Id = 4, Name = "local", NormalizedName = "LOCAL" }
            );
            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = 1,
                    UserName = "user1",
                    Email = "user1@example.com",
                    FullName = "User One",
                    EmailConfirmed = false,
                    RegistrationTime = DateTime.UtcNow
                },
                new ApplicationUser
                {
                    Id = 2,
                    UserName = "user2",
                    Email = "user2@example.com",
                    FullName = "User Two",
                    EmailConfirmed = false,
                    RegistrationTime = DateTime.UtcNow
                },
                new ApplicationUser
                {
                    Id = 3,
                    UserName = "user3",
                    Email = "user3@example.com",
                    FullName = "User Three",
                    EmailConfirmed = false,
                    RegistrationTime = DateTime.UtcNow
                },
                new ApplicationUser
                {
                    Id = 4,
                    UserName = "user4",
                    Email = "user4@example.com",
                    FullName = "User Four",
                    EmailConfirmed = false,
                    RegistrationTime = DateTime.UtcNow
                },
                new ApplicationUser
                {
                    Id = 5,
                    UserName = "user5",
                    Email = "user5@example.com",
                    FullName = "User Five",
                    EmailConfirmed = false,
                    RegistrationTime = DateTime.UtcNow
                },
                new ApplicationUser
                {
                    Id = 6,
                    UserName = "userSystem1",
                    Email = "userSystem1@example.com",
                    FullName = "User System",
                    EmailConfirmed = false,
                    RegistrationTime = DateTime.UtcNow
                },
                new ApplicationUser
                {
                    Id = 7,
                    UserName = "Admin1",
                    Email = "Admin1@example.com",
                    FullName = "Admin 1",
                    EmailConfirmed = false,
                    RegistrationTime = DateTime.UtcNow
                }
            );
            // Seed data cho user roles (mỗi user được gán một role)
            modelBuilder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int> { UserId = 6, RoleId = 2 },
                new IdentityUserRole<int> { UserId = 7, RoleId = 1 },
                new IdentityUserRole<int> { UserId = 1, RoleId = 3 }, // user1 là admin
                new IdentityUserRole<int> { UserId = 2, RoleId = 4 }, // user2 là user
                new IdentityUserRole<int> { UserId = 3, RoleId = 4 }, // user3 là user
                new IdentityUserRole<int> { UserId = 4, RoleId = 4 }, // user4 là user
                new IdentityUserRole<int> { UserId = 5, RoleId = 4 }  // user5 là user
            );
            // Seed data cho profiles
            modelBuilder.Entity<Profile>().HasData(
                new Profile
                {
                    ProfileId = 1,
                    UserId = 1,
                    FullName = "User One",
                    Address = "123 Main St, Hanoi",
                    Phone = "0123456789",
                    Gender = "Male",
                    Birthdate = DateTime.UtcNow,
                    ImageUser = "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e7/Instagram_logo_2016.svg/2048px-Instagram_logo_2016.svg.png"
                },
                new Profile
                {
                    ProfileId = 2,
                    UserId = 2,
                    FullName = "User Two",
                    Address = "456 Secondary St, Ho Chi Minh",
                    Phone = "0987654321",
                    Gender = "Male",
                    Birthdate = DateTime.UtcNow,
                    ImageUser = "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e7/Instagram_logo_2016.svg/2048px-Instagram_logo_2016.svg.png"
                },
                new Profile
                {
                    ProfileId = 3,
                    UserId = 3,
                    FullName = "User Three",
                    Address = "789 Tertiary St, Da Nang",
                    Phone = "0912345678",
                    Gender = "Male",
                    Birthdate = DateTime.UtcNow,
                    ImageUser = "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e7/Instagram_logo_2016.svg/2048px-Instagram_logo_2016.svg.png"
                },
                new Profile
                {
                    ProfileId = 4,
                    UserId = 4,
                    FullName = "User Four",
                    Address = "101 Eleventh St, Hue",
                    Phone = "0998765432",
                    Gender = "Male",
                    Birthdate = DateTime.UtcNow,
                    ImageUser = "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e7/Instagram_logo_2016.svg/2048px-Instagram_logo_2016.svg.png"
                },
                new Profile
                {
                    ProfileId = 5,
                    UserId = 5,
                    FullName = "User Five",
                    Address = "202 Twelfth St, Phu Quoc",
                    Phone = "0923456789",
                    Gender = "Male",
                    Birthdate = DateTime.UtcNow,
                    ImageUser = "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e7/Instagram_logo_2016.svg/2048px-Instagram_logo_2016.svg.png"
                }
            );
            // Seed data cho Locations (địa điểm trên lãnh thổ Việt Nam)
            modelBuilder.Entity<Location>().HasData(
                new Location { LocationId = 1, LocationName = "Hà Nội" },
                new Location { LocationId = 2, LocationName = "Hồ Chí Minh" },
                new Location { LocationId = 3, LocationName = "Đà Nẵng" },
                new Location { LocationId = 4, LocationName = "Huế" },
                new Location { LocationId = 5, LocationName = "Hội An" },
                new Location { LocationId = 6, LocationName = "Nha Trang" },
                new Location { LocationId = 7, LocationName = "Phú Quốc" },
                new Location { LocationId = 8, LocationName = "Vịnh Hạ Long" }
            );

            // Seed data cho Activities (các sở thích và hoạt động)
            modelBuilder.Entity<Activity>().HasData(
                new Activity { ActivityId = 1, ActivityName = "Đi bộ" },
                new Activity { ActivityId = 2, ActivityName = "Đi phượt" },
                new Activity { ActivityId = 3, ActivityName = "Chơi golf" },
                new Activity { ActivityId = 4, ActivityName = "Tắm biển" },
                new Activity { ActivityId = 5, ActivityName = "Leo núi" },
                new Activity { ActivityId = 6, ActivityName = "Câu cá" },
                new Activity { ActivityId = 7, ActivityName = "Đi xe đạp" },
                new Activity { ActivityId = 8, ActivityName = "Tham quan văn hóa" }
            );
            // Seed data cho bảng UserLocations
            modelBuilder.Entity<UserLocation>().HasData(
                new UserLocation { UserId = 1, LocationId = 1 },
                new UserLocation { UserId = 2, LocationId = 3 },
                new UserLocation { UserId = 3, LocationId = 3 },
                new UserLocation { UserId = 4, LocationId = 3 },
                new UserLocation { UserId = 5, LocationId = 3 },
                new UserLocation { UserId = 3, LocationId = 2 },
                new UserLocation { UserId = 4, LocationId = 2 }
            );

            // Seed data cho bảng UserActivities
            modelBuilder.Entity<UserActivity>().HasData(
                new UserActivity { UserId = 1, ActivityId = 1 },
                new UserActivity { UserId = 1, ActivityId = 2 },
                new UserActivity { UserId = 1, ActivityId = 3 },
                new UserActivity { UserId = 1, ActivityId = 4 },
                new UserActivity { UserId = 2, ActivityId = 1 },
                new UserActivity { UserId = 3, ActivityId = 3 },
                new UserActivity { UserId = 3, ActivityId = 2 },
                new UserActivity { UserId = 3, ActivityId = 4 },
                new UserActivity { UserId = 5, ActivityId = 1 },
                new UserActivity { UserId = 5, ActivityId = 2 }
            );


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


            modelBuilder.Entity<Contract>()
                .HasOne(c => c.CreatedByUser)
                .WithMany(u => u.CreatedContracts)
                .HasForeignKey(c => c.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Contract>()
                .HasOne(c => c.PaidByUser)
                .WithMany(u => u.PaidContracts)
                .HasForeignKey(c => c.PaidById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
       .HasOne(m => m.CreatedByUser)
       .WithMany(u => u.Messages)
       .HasForeignKey(m => m.CreatedById)
       .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.SendToUser)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.SendToId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PastTripPost>()
       .HasOne(m => m.Traveler)
       .WithMany(u => u.PastTripPosts)
       .HasForeignKey(m => m.TravelerId)
       .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PastTripPost>()
                .HasOne(m => m.Local)
                .WithMany(u => u.PastTripPostReviews)
                .HasForeignKey(m => m.LocalId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Report>()
      .HasOne(m => m.ReportToUser)
      .WithMany(u => u.ReceivedReports)
      .HasForeignKey(m => m.ReportToId)
      .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Report>()
                .HasOne(m => m.SentByUser)
                .WithMany(u => u.Reports)
                .HasForeignKey(m => m.SentById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Request>()
     .HasOne(m => m.RequestByUser)
     .WithMany(u => u.Requests)
     .HasForeignKey(m => m.RequestById)
     .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Request>()
                .HasOne(m => m.RequestToUser)
                .WithMany(u => u.ReceivedRequests)
                .HasForeignKey(m => m.RequestToId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OnTravelling>()
       .HasKey(ot => new { ot.UserId, ot.DestinationId });

            modelBuilder.Entity<OnTravelling>()
                .HasOne(ot => ot.User)
                .WithMany(e => e.OnTravel)
                .HasForeignKey(ep => ep.UserId);

            modelBuilder.Entity<OnTravelling>()
                .HasOne(ep => ep.Destination)
                .WithMany(u => u.Travellers)
                .HasForeignKey(ep => ep.DestinationId);

            //     modelBuilder.Entity<Reaction>()
            //.HasKey(ot => new { ot.ReactedById, ot.PostId });

            //modelBuilder.Entity<Reaction>()
            //    .HasOne(ot => ot.ReactedByUser)
            //    .WithMany(e => e.Reactions)
            //    .HasForeignKey(ep => ep.ReactedById);

            //modelBuilder.Entity<Reaction>()
            //    .HasOne(ep => ep.GroupPost)
            //    .WithMany(u => u.Reactions)
            //    .HasForeignKey(ep => ep.PostId);

            modelBuilder.Entity<UserDescription>()
                .HasKey(ud => ud.UserId);

            modelBuilder.Entity<GroupParticipant>()
               .HasOne(gp => gp.Group)
               .WithMany(g => g.GroupParticipants)
               .HasForeignKey(gp => gp.GroupId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GroupParticipant>()
               .HasOne(gp => gp.User)
               .WithMany(g => g.GroupParticipants)
               .HasForeignKey(gp => gp.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GroupPost>()
                .HasOne(p => p.Group)
                .WithMany(g => g.GroupPosts)
                .HasForeignKey(p => p.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GroupPost>()
                .HasOne(p => p.PostBy)
                .WithMany(g => g.GroupPosts)
                .HasForeignKey(p => p.PostById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PostComment>()
                .HasOne(pc => pc.Post)
                .WithMany(p => p.PostComments)
                .HasForeignKey(pc => pc.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PostComment>()
                .HasOne(pc => pc.CommentedBy)
                .WithMany(p => p.PostComments)
                .HasForeignKey(pc => pc.CommentedById)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<GroupPostPhoto>()
                .HasOne(pp => pp.Post)
                .WithMany(p => p.GroupPostPhotos)
                .HasForeignKey(pp => pp.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
