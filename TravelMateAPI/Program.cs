using AutoMapper;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using BussinessObjects;
using BussinessObjects.Configuration;
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
using TravelMateAPI.Services.Email;
using TravelMateAPI.Services.FindLocal;
using TravelMateAPI.Services.Notification;

namespace TravelMateAPI
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Lấy giá trị từ Key Vault
            var keyVaultUrl = new Uri("https://travelmatekeyvault.vault.azure.net/");
            var client = new SecretClient(vaultUri: keyVaultUrl, credential: new DefaultAzureCredential());

            // Lấy JWT settings từ Key Vault
            var jwtSecretKey = (await client.GetSecretAsync("JwtSecretKey")).Value.Value;
            var jwtIssuer = (await client.GetSecretAsync("JwtIssuer")).Value.Value;
            var jwtAudience = (await client.GetSecretAsync("JwtAudience")).Value.Value;
            var jwtDurationInMinutes = (await client.GetSecretAsync("JwtDurationInMinutes")).Value.Value;

            // Lấy Mail settings từ Key Vault
            var mailAddress = (await client.GetSecretAsync("MailAddress")).Value.Value;
            var mailDisplayName = (await client.GetSecretAsync("MailDisplayName")).Value.Value;
            var mailPassword = (await client.GetSecretAsync("MailPassword")).Value.Value;
            var mailHost = (await client.GetSecretAsync("MailHost")).Value.Value;
            var mailPort = (await client.GetSecretAsync("MailPort")).Value.Value;

            // Lấy Google Auth settings từ Key Vault
            var googleClientId = (await client.GetSecretAsync("GoogleClientID")).Value.Value;
            var googleClientSecret = (await client.GetSecretAsync("GoogleClientSecret")).Value.Value;

            // Lấy Firebase settings từ Key Vault
            var firebaseApiKey = (await client.GetSecretAsync("FirebaseAPIKey")).Value.Value;
            var firebaseAuthDomain = (await client.GetSecretAsync("FirebaseAuthDomain")).Value.Value;
            var firebaseProjectId = (await client.GetSecretAsync("FirebaseProjectID")).Value.Value;
            var firebaseStorageBucket = (await client.GetSecretAsync("FirebaseStorageBucket")).Value.Value;
            var firebaseMessagingSenderId = (await client.GetSecretAsync("FirebaseMessagingSenderID")).Value.Value;
            var firebaseAppId = (await client.GetSecretAsync("FirebaseAppID")).Value.Value;
            var firebaseMeasurementId = (await client.GetSecretAsync("FirebaseMeasurementID")).Value.Value;
            var firebaseAdminSdkJsonPath = (await client.GetSecretAsync("FirebaseAdminSdkJsonPath")).Value.Value;

            // Tạo đối tượng AppSettings
            var appSettings = new AppSettings
            {
                JwtSettings = new JwtSettings
                {
                    SecretKey = jwtSecretKey,
                    Issuer = jwtIssuer,
                    Audience = jwtAudience,
                    DurationInMinutes = int.Parse(jwtDurationInMinutes)
                },
                MailSettings = new MailSettings
                {
                    Mail = mailAddress,
                    DisplayName = mailDisplayName,
                    Password = mailPassword,
                    Host = mailHost,
                    Port = int.Parse(mailPort)
                },
                FirebaseConfig = new FirebaseConfig
                {
                    ApiKey = firebaseApiKey,
                    AuthDomain = firebaseAuthDomain,
                    ProjectId = firebaseProjectId,
                    StorageBucket = firebaseStorageBucket,
                    MessagingSenderId = firebaseMessagingSenderId,
                    AppId = firebaseAppId,
                    MeasurementId = firebaseMeasurementId,
                    FirebaseAdminSdkJsonPath = firebaseAdminSdkJsonPath
                },
                GoogleAuthSettings = new GoogleAuthSettings
                {
                    ClientId = googleClientId,
                    ClientSecret = googleClientSecret
                }
            };

            builder.Services.AddSingleton(appSettings);

            // Cấu hình Identity
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 1;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                //shorter constraint
                //options.User.AllowedUserNameCharacters = "a-zA-Z0-9-._@+";

                options.User.RequireUniqueEmail = true;

                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            })
            .AddEntityFrameworkStores<ApplicationDBContext>()
            .AddDefaultTokenProviders();

            // Cấu hình Authentication
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
                    ValidIssuer = appSettings.JwtSettings.Issuer,
                    ValidAudience = appSettings.JwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.JwtSettings.SecretKey))
                };
            })
            .AddGoogle(options =>
            {
                options.ClientId = appSettings.GoogleAuthSettings.ClientId;
                options.ClientSecret = appSettings.GoogleAuthSettings.ClientSecret;
            });

            builder.Services.AddScoped<TokenService>();

            // OData configuration
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
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

            builder.Services.AddControllers().AddOData(opt => opt.Select().Expand().Filter().OrderBy().Count().SetMaxTop(null)
                            .AddRouteComponents("odata", modelBuilder.GetEdmModel()));

            // Cấu hình Mail và Firebase
            builder.Services.AddScoped<IMailServiceSystem, SendMailService>();
            //builder.Services.AddSingleton<FirebaseService>();

            // Đăng ký các repository
            builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
            builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
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

            // Cấu hình Swagger
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            // Cấu hình CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
