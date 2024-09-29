using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using RHCQS_BusinessObjects;
using RHCQS_DataAccessObjects;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;
using RHCQS_Services.Implement;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


namespace RHCQS_BE.Extenstion
{
    public static class DependencyServices
    {
        //public static IConfiguration Configuration { get; }


        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddLogging();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

        public static IServiceCollection AddSingletonJson(this IServiceCollection services)
        {
            services.AddSingleton<JsonSerializerSettings>(JsonSerializationHelper.GetNewtonsoftJsonSerializerSettings());
            return services;
        }
        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services.AddDbContext<RhcqsContext>(options =>
                options.UseSqlServer(GetConnectionString()));
            return services;
        }

        public static IServiceCollection AddSignalRServices(this IServiceCollection services)
        {
            services.AddSignalR();
            return services;
        }


        private static string GetConnectionString()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", true, true)
                        .Build();
            var strConn = config["ConnectionStrings:DefaultConnection"];

            return strConn;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IConstructionItemService, ConstructionItemService>();
            services.AddScoped<IUtilitiesService, UtilitiesService>();
            return services;
        }

        public static IServiceCollection AddJwtValidation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                        if (!string.IsNullOrEmpty(token))
                        {
                            context.Token = token;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            return services;
        }

        public static IServiceCollection AddConfigSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "V1",
                    Title = "Residential Housing Construction Quotation System Documentation",
                    Description = "Residential Housing Construction Quotation System"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                options.IncludeXmlComments(xmlPath);

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                options.MapType<TimeOnly>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "time",
                    Example = OpenApiAnyFactory.CreateFromJson("\"13:45:42.0000000\"")
                });
            });
            return services;
        }
    }
}

