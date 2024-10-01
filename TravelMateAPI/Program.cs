using BussinessObjects;
using BussinessObjects.Entities;
using BussinessObjects.Utils.Request;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Repositories.Interface;
using Repositories;
using System.Text;
using TravelMateAPI.Services.Email;
using Microsoft.Extensions.Configuration;
using TravelMateAPI.Services.Auth;
using Microsoft.OpenApi.Models;
using DotNetEnv;
using DataAccess;

namespace TravelMateAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            Env.Load();
            //odata 

            ODataConventionModelBuilder modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<ApplicationUser>("ApplicationUsers");
            modelBuilder.EntitySet<Profile>("Profiles");
            modelBuilder.EntitySet<Friend>("Friends");
            modelBuilder.EntitySet<Event>("Events");
            modelBuilder.EntitySet<EventParticipants>("EventParticipants");
            modelBuilder.EntitySet<Location>("Locations");
            modelBuilder.EntitySet<Activity>("Activities");
            modelBuilder.EntitySet<UserLocation>("UserLocations");
            modelBuilder.EntitySet<UserActivity>("UserActivities");

            builder.Services.AddScoped(typeof(ApplicationDBContext));
            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddControllers().AddOData(opt => opt.Select().Expand().Filter().OrderBy().Count().SetMaxTop(null)
                            .AddRouteComponents("odata", modelBuilder.GetEdmModel()));

            var jwtSettings = new JwtSettings
            {
                SecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY"),
                Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
                DurationInMinutes = int.Parse(Environment.GetEnvironmentVariable("JWT_DURATION_IN_MINUTES"))
            };



            builder.Services.AddSingleton(jwtSettings);
            
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                // Cấu hình tùy chỉnh cho Identity nếu cần
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = false;
                // Thiết lập về Password
                options.Password.RequireDigit = false; // Không bắt phải có số
                options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
                options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
                options.Password.RequireUppercase = false; // Không bắt buộc chữ in
                options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
                options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

                // Cấu hình Lockout - khóa user
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
                options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lần thì khóa
                options.Lockout.AllowedForNewUsers = true;

                // Cấu hình về User.
                options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;  // Email là duy nhất

                // Cấu hình đăng nhập.
                options.SignIn.RequireConfirmedEmail = false;       //đăng tắt xác thực     // Cấu hình xác thực địa chỉ email (email phải tồn tại)
                options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
            })
                .AddEntityFrameworkStores<ApplicationDBContext>()
                .AddDefaultTokenProviders();


            //Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    //ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                    //ValidAudience = builder.Configuration["JwtSettings:Audience"],
                    //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };
            })
            .AddGoogle(options =>
            {
                IConfigurationSection googleAuthSection = builder.Configuration.GetSection("Authentication:Google");
                //options.ClientId = googleAuthSection["ClientId"];
                //options.ClientSecret = googleAuthSection["ClientSecret"];
                options.ClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
                options.ClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET");
                //options.CallbackPath = "/signin-google";
            }); 

            builder.Services.AddScoped<TokenService>();
            //builder.Services.AddHostedService<AccountCleanupService>();

            //var mailSettings = builder.Configuration.GetSection("MailSettings");
            //builder.Services.Configure<MailSettings>(mailSettings);
            var mailSettings = new MailSettings
            {
                Mail = Environment.GetEnvironmentVariable("MAIL_ADDRESS"),
                DisplayName = Environment.GetEnvironmentVariable("MAIL_DISPLAY_NAME"),
                Password = Environment.GetEnvironmentVariable("MAIL_PASSWORD"),
                Host = Environment.GetEnvironmentVariable("MAIL_HOST"),
                Port = int.Parse(Environment.GetEnvironmentVariable("MAIL_PORT"))
            };

            builder.Services.AddSingleton(mailSettings);
            builder.Services.AddScoped<IMailServiceSystem, SendMailService>();

            //firebase 
            var firebaseConfig = new
            {
                ApiKey = Env.GetString("FIREBASE_API_KEY"),
                AuthDomain = Env.GetString("FIREBASE_AUTH_DOMAIN"),
                ProjectId = Env.GetString("FIREBASE_PROJECT_ID"),
                StorageBucket = Env.GetString("FIREBASE_STORAGE_BUCKET"),
                MessagingSenderId = Env.GetString("FIREBASE_MESSAGING_SENDER_ID"),
                AppId = Env.GetString("FIREBASE_APP_ID"),
                MeasurementId = Env.GetString("FIREBASE_MEASUREMENT_ID")
            };
            builder.Services.AddSingleton(firebaseConfig);

            // Register your repositories
            builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
            builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            // Đăng ký FindLocalDAO
            builder.Services.AddScoped<FindLocalDAO>();
            builder.Services.AddScoped<IFindLocalRepository, FindLocalRepository>();
            builder.Services.AddScoped<IEventRepository, EventRepository>();
            builder.Services.AddScoped<IEventParticipantsRepository, EventParticipantsRepository>();
            builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
            builder.Services.AddScoped<ILocationRepository, LocationRepository>();
            builder.Services.AddScoped<IUserActivitiesRepository, UserActivitiesRepository>();
            builder.Services.AddScoped<IUserLocationsRepository, UserLocationsRepository>();


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(
                c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                    // Thêm scheme mặc định là HTTPS nếu API chạy HTTPS
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please insert JWT with Bearer into field",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }});
                }
            );
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder
                        .AllowAnyOrigin()    // Allows all origins
                        .AllowAnyMethod()    // Allows all HTTP methods
                        .AllowAnyHeader());  // Allows all headers
            });
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            app.UseHttpsRedirection();
            // Use CORS policy
            app.UseCors("AllowAll");

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
