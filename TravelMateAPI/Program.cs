using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using BussinessObjects;
using BussinessObjects.Entities;
using BussinessObjects.Utils.Request;
using DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Repositories;
using Repositories.Interface;
using System.Text;
using TravelMateAPI.Models;
using TravelMateAPI.Services.Email;
using TravelMateAPI.Services.FindLocal;
using TravelMateAPI.Services.Firebase;
using TravelMateAPI.Services.Notification;

namespace TravelMateAPI
{

    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.

            //Env.Load();

            var keyVaultUrl = new Uri("https://travelmatekeyvault.vault.azure.net/");
            var client = new SecretClient(vaultUri: keyVaultUrl, credential: new DefaultAzureCredential());

            KeyVaultSecret jwtSecretKey = (await client.GetSecretAsync("JwtSecretKey"));
            KeyVaultSecret jwtIssuer = (await client.GetSecretAsync("JwtIssuer"));
            KeyVaultSecret jwtAudience = (await client.GetSecretAsync("JwtAudience"));
            KeyVaultSecret jwtDurationInMinutes = (await client.GetSecretAsync("JwtDurationInMinutes"));

            var googleClientId = (await client.GetSecretAsync("GoogleClientID")).Value.Value;
            var googleClientSecret = (await client.GetSecretAsync("GoogleClientSecret")).Value.Value;

            var mailAddress = (await client.GetSecretAsync("MailAddress")).Value.Value;
            var mailDisplayName = (await client.GetSecretAsync("MailDisplayName")).Value.Value;
            var mailPassword = (await client.GetSecretAsync("MailPassword")).Value.Value;
            var mailHost = (await client.GetSecretAsync("MailHost")).Value.Value;
            var mailPort = (await client.GetSecretAsync("MailPort")).Value.Value;

            var firebaseApiKey = (await client.GetSecretAsync("FirebaseAPIKey")).Value.Value;
            var firebaseAuthDomain = (await client.GetSecretAsync("FirebaseAuthDomain")).Value.Value;
            var firebaseProjectId = (await client.GetSecretAsync("FirebaseProjectID")).Value.Value;
            var firebaseStorageBucket = (await client.GetSecretAsync("FirebaseStorageBucket")).Value.Value;
            var firebaseMessagingSenderId = (await client.GetSecretAsync("FirebaseMessagingSenderID")).Value.Value;
            var firebaseAppId = (await client.GetSecretAsync("FirebaseAppID")).Value.Value;
            var firebaseMeasurementId = (await client.GetSecretAsync("FirebaseMeasurementID")).Value.Value;
            var firebaseAdminSdkJsonPath = (await client.GetSecretAsync("FirebaseAdminSdkJsonPath")).Value.Value;

            //var jwtSettings = new JwtSettings
            //{
            //    SecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY"),
            //    Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
            //    Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
            //    DurationInMinutes = int.Parse(Environment.GetEnvironmentVariable("JWT_DURATION_IN_MINUTES"))
            //};

            var jwtSettings = new JwtSettings
            {
                SecretKey = jwtSecretKey.Value,
                Issuer = jwtIssuer.Value,
                Audience = jwtAudience.Value,
                DurationInMinutes = int.Parse(jwtDurationInMinutes.Value)
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
                options.ClientId = googleClientId;
                options.ClientSecret = googleClientSecret;
                //options.CallbackPath = "/signin-google";
            });

            builder.Services.AddScoped<TokenService>();


            //odata 

            ODataConventionModelBuilder modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<ApplicationUser>("ApplicationUsers");
            modelBuilder.EntitySet<Profile>("Profiles");
            modelBuilder.EntitySet<Friendship>("Friends");
            modelBuilder.EntitySet<Event>("Events");
            modelBuilder.EntitySet<EventParticipants>("EventParticipants");
            modelBuilder.EntitySet<Location>("Locations");
            modelBuilder.EntitySet<Activity>("Activities");
            modelBuilder.EntitySet<UserLocation>("UserLocations");
            modelBuilder.EntitySet<UserActivity>("UserActivities");

            builder.Services.AddScoped(typeof(ApplicationDBContext));
            //builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

            builder.Services.AddControllers().AddOData(opt => opt.Select().Expand().Filter().OrderBy().Count().SetMaxTop(null)
                            .AddRouteComponents("odata", modelBuilder.GetEdmModel()));

            //builder.Services.AddHostedService<AccountCleanupService>();

            //var mailSettings = builder.Configuration.GetSection("MailSettings");
            //builder.Services.Configure<MailSettings>(mailSettings);


            var mailSettings = new MailSettings
            {
                Mail = mailAddress,
                DisplayName = mailDisplayName,
                Password = mailPassword,
                Host = mailHost,
                Port = int.Parse(mailPort)
            };

            builder.Services.AddSingleton(mailSettings);
            builder.Services.AddScoped<IMailServiceSystem, SendMailService>();

            //firebase 
            var firebaseConfig = new FirebaseConfig
            {

                ApiKey = firebaseApiKey,
                AuthDomain = firebaseAuthDomain,
                ProjectId = firebaseProjectId,
                StorageBucket = firebaseStorageBucket,
                MessagingSenderId = firebaseMessagingSenderId,
                AppId = firebaseAppId,
                MeasurementId = firebaseMeasurementId,
                FirebaseAdminSdkJsonPath = firebaseAdminSdkJsonPath
            };
            builder.Services.AddSingleton(firebaseConfig);
            builder.Services.AddSingleton<FirebaseService>();

            // Register your repositories
            builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
            builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();

            //builder.Services.AddScoped<IFindLocalRepository, FindLocalRepository>();
            builder.Services.AddScoped<IFindLocalService, FindLocalService>();
            builder.Services.AddScoped<IFindLocalByFeedbackService, FindLocalByFeedbackService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IEventRepository, EventRepository>();
            builder.Services.AddScoped<IEventParticipantsRepository, EventParticipantsRepository>();
            builder.Services.AddScoped<ActivitiesDAO>();
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
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    // Nếu bạn sử dụng OData

                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            // Use CORS policy
            app.UseCors("AllowAll");

            // Use CORS policy
            //app.UseCors("AllowAllOrigins");

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
