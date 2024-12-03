using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using BusinessObjects;
using BusinessObjects.Configuration;
using BusinessObjects.Entities;
using BusinessObjects.Utils.Request;
using BusinessObjects.Utils.Response;
using DataAccess;
using Google.Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Repositories;
using Repositories.Interface;
using Repository.Interfaces;
using System.Text;
using TravelMateAPI.Hubs;
using TravelMateAPI.Middleware;
using TravelMateAPI.Models;
using TravelMateAPI.Services.CCCDValid;
using TravelMateAPI.Services.Contract;
using TravelMateAPI.Services.Email;
using TravelMateAPI.Services.FilterLocal;
using TravelMateAPI.Services.FindLocal;
using TravelMateAPI.Services.Hubs;
using TravelMateAPI.Services.Notification;
using TravelMateAPI.Services.Notification.Event;

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

            builder.Services.AddSingleton<MongoDbContext>();

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
            //var userSet = modelBuilder.EntitySet<ApplicationUserDTO>("ApplicationUsers");
            //userSet.EntityType.HasKey(u => u.UserId);
            //userSet.EntityType.Property(u => u.FullName);
            //userSet.EntityType.Property(u => u.Email);
            //userSet.EntityType.CollectionProperty(u => u.Roles);
            //userSet.EntityType.ComplexProperty(u => u.Profile);
            var userSet = modelBuilder.EntitySet<UserWithDetailsDTO>("ApplicationUsers");
            userSet.EntityType.HasKey(u => u.UserId);
            userSet.EntityType.Property(u => u.FullName);
            userSet.EntityType.Property(u => u.Email);
            userSet.EntityType.Property(u => u.Star);
            userSet.EntityType.CollectionProperty(u => u.LocationIds);
            userSet.EntityType.ComplexProperty(u => u.Profile);
            userSet.EntityType.CollectionProperty(u => u.Roles);
            userSet.EntityType.ComplexProperty(u => u.CCCD);
            //userSet.EntityType.ComplexProperty(u => u.UserActivities);
            userSet.EntityType.CollectionProperty(u => u.ActivityIds);
            //userSet.EntityType.Property(u => u.SimilarityScore);

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
            //real time
            builder.Services.AddSignalR();
            builder.Services.AddHttpClient();
            builder.Services.AddMemoryCache();
            // Đăng ký các repository
            builder.Services.AddScoped<ProfileDAO>();
            builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
            builder.Services.AddScoped<ApplicationUserDAO>();
            builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            builder.Services.AddScoped<CCCDDAO>();
            builder.Services.AddScoped<ICCCDRepository, CCCDRepository>();
            builder.Services.AddScoped<ICCCDService, CCCDService>();
            builder.Services.AddScoped<FilterUserService>();
            builder.Services.AddScoped<IContractService, ContractService>();
            builder.Services.AddScoped<LocationService>();
            builder.Services.AddScoped<IFindLocalService, FindLocalService>();
            builder.Services.AddScoped<ISearchLocationService, SearchLocationService>();
            builder.Services.AddScoped<SearchLocationFuzzyService>();
            builder.Services.AddScoped<IFindLocalByFeedbackService, FindLocalByFeedbackService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<EventDAO>();
            builder.Services.AddScoped<IEventRepository, EventRepository>();
            builder.Services.AddScoped<EventParticipantsDAO>();
            builder.Services.AddScoped<IEventParticipantsRepository, EventParticipantsRepository>();
            builder.Services.AddScoped<EventNotificationService>();
            builder.Services.AddHostedService<BackgroundNotificationWorker>();
            builder.Services.AddScoped<ActivitiesDAO>();
            builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
            builder.Services.AddScoped<LocationsDAO>();
            builder.Services.AddScoped<ILocationRepository, LocationRepository>();
            builder.Services.AddScoped<UserActivitiesDAO>();
            builder.Services.AddScoped<IUserActivitiesRepository, UserActivitiesRepository>();
            builder.Services.AddScoped<UserLocationsDAO>();
            builder.Services.AddScoped<IUserLocationsRepository, UserLocationsRepository>();
            builder.Services.AddScoped<LanguagesDAO>();
            builder.Services.AddScoped<ILanguagesRepository, LanguagesRepository>();
            builder.Services.AddScoped<SpokenLanguagesDAO>();
            builder.Services.AddScoped<ISpokenLanguagesRepository, SpokenLanguagesRepository>();
            builder.Services.AddScoped<UniversityDAO>();
            builder.Services.AddScoped<IUniversityRepository, UniversityRepository>();
            builder.Services.AddScoped<UserEducationDAO>();
            builder.Services.AddScoped<IUserEducationRepository, UserEducationRepository>();
            builder.Services.AddScoped<UserHomeDAO>();
            builder.Services.AddScoped<IUserHomeRepository, UserHomeRepository>();
            builder.Services.AddScoped<HomePhotoDAO>();
            builder.Services.AddScoped<IHomePhotoRepository, HomePhotoRepository>();
            builder.Services.AddScoped<GroupDAO>();
            builder.Services.AddScoped<IGroupRepository, GroupRepository>();
            builder.Services.AddScoped<GroupPostDAO>();
            builder.Services.AddScoped<IGroupPostRepository, GroupPostRepository>();
            builder.Services.AddScoped<PostCommentDAO>();
            builder.Services.AddScoped<IPostCommentRepository, PostCommentRepository>();
            builder.Services.AddScoped<PastTripPostDAO>();
            builder.Services.AddScoped<IPastTripPostRepository, PastTripPostRepository>();
            builder.Services.AddScoped<ExtraFormDetailDAO>();
            builder.Services.AddScoped<ILocalExtraDetailFormRepository, LocalExtraDetailFormRepository>();
            builder.Services.AddScoped<ITravelerFormRepository, TravelerFormRepository>();
            builder.Services.AddScoped<TourDAO>();
            builder.Services.AddScoped<ITourRepository, TourRepository>();

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            });


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
                //options.AddPolicy("AllowAll",
                //    builder => builder
                //        .AllowAnyOrigin()
                //        .AllowAnyMethod()
                //        .AllowAnyHeader());
                options.AddPolicy("AllowSpecificOrigins",
                policyBuilder =>
                {
                    policyBuilder.WithOrigins("https://travelmatefe.netlify.app/", "http://localhost:5173", "http://localhost:5174", "http://localhost:5500/") // Địa chỉ của ứng dụng React của bạn
                                 .AllowAnyMethod()
                                 .AllowAnyHeader()
                                 .AllowCredentials(); // Quan trọng khi sử dụng cookies hoặc thông tin xác thực
                });
            });

            //builder.Services.AddSignalR();

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


            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowSpecificOrigins");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            // real time
            app.MapHub<ServiceHub>("/serviceHub");
            app.MapHub<ChatHub>("/chatHub");
            app.Run();
        }
    }
}