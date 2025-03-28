using ConferencesManagementAPI.DAO.Repositories;
using ConferencesManagementDAO.Data.Entities;
using ConferencesManagementDAO.IRepositories;
using ConferencesManagementDAO.Repositories;
using ConferencesManagementService;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

namespace ConferencesManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                builder.WebHost.ConfigureKestrel(options =>
                {
                    options.ListenAnyIP(7171); // Chỉ chạy HTTP
                });

                // Lấy ConnectionString từ biến môi trường hoặc `appsettings.json`
                var connectionString = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"))
                                            ? Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
                                                : builder.Configuration.GetConnectionString("DefaultConnection");

                // Add services to the container.
                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

                    // Cấu hình để thêm nút "Authorize" trong Swagger UI
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Nhập token vào trường này: Bearer {your token}"
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

                // Đăng ký ILogger
                builder.Services.AddLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.AddDebug();
                });

                builder.Services.AddAutoMapper(typeof(Program));

                builder.Services.AddDbContext<ConferenceManagementDbContext>(options =>
                    options.UseSqlServer(connectionString));

                // Đăng ký Repository và Service
                builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
                builder.Services.AddScoped<DelegateService>();
                builder.Services.AddScoped<ConferenceService>();
                builder.Services.AddScoped<ConferenceRoleService>();
                builder.Services.AddScoped<RegistrationService>();
                builder.Services.AddScoped<ConferenceHostingRegistrationService>();
                builder.Services.AddScoped<DelegateConferenceRoleService>();


                builder.Services.AddScoped<IAuthService, AuthService>();
                builder.Services.AddScoped<DelegatesRepositories>();
                builder.Services.AddScoped<ConferenceRepositories>();
                builder.Services.AddScoped<ConferenceRoleRepositories>();
                builder.Services.AddScoped<RegistrationRepositories>();
                builder.Services.AddScoped<DelegateConferenceRoleRepositories>();
                builder.Services.AddScoped<ConferenceHostingRegistrationRepositories>();


                // Cấu hình Authentication với JWT
                builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                        ValidAudience = builder.Configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!))
                    };
                });

                // Đăng ký DbContext
                builder.Services.AddDbContext<ConferenceManagementDbContext>(options =>
                    options.UseSqlServer(connectionString));

                // Đăng ký DatabaseMigrator
                builder.Services.AddScoped<DatabaseMigrator>();

                // Thêm CORS Policy
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowAll",
                        policy =>
                        {
                            policy.SetIsOriginAllowed(_ => true)
                                  .AllowAnyMethod()
                                  .AllowAnyHeader();
                        });
                });

                var app = builder.Build();

                // Gọi migrate database
                using (var scope = app.Services.CreateScope())
                {
                    var migrator = scope.ServiceProvider.GetRequiredService<DatabaseMigrator>();
                    migrator.MigrateDatabase();
                }

                app.UseCors("AllowAll"); // Áp dụng CORS Policy

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();
                app.UseAuthentication();
                app.UseAuthorization();

                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }
    }
}
