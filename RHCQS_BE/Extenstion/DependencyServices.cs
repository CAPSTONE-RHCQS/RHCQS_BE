using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using RHCQS_BusinessObjects;
using RHCQS_Repositories.UnitOfWork;
using RHCQS_Services.Interface;
using RHCQS_Services.Implement;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using CloudinaryDotNet;
using DinkToPdf;
using DinkToPdf.Contracts;
using RHCQS_DataAccessObjects.Context;
using System.Runtime.InteropServices;


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

            return strConn!;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IHouseTemplateService, HouseTemplateService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IPackageTypeService, PackageTypeService>();
            services.AddScoped<IPackageService, PackageService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IConstructionItemService, ConstructionItemService>();
            services.AddScoped<IUtilitiesService, UtilitiesService>();
            services.AddScoped<IHouseDesignDrawingService, HouseDesignDrawingService>();
            services.AddScoped<IAssignTaskService, AssignTaskService>();
            services.AddScoped<IHouseDesignVersionService, HouseDesignVersionService>();
            services.AddScoped<IInitialQuotationService, InitialQuotationService>();
            services.AddScoped<IFinalQuotationService, FinalQuotationService>();
            services.AddScoped<IContractService, ContractService>();
            services.AddScoped<IUploadImgService, UploadImgService>();
            services.AddScoped<IPromotionService, PromotionService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IMaterialService, MaterialService>();
            services.AddApiBehavior();
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddSignalR();
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
                            Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
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

        public static IServiceCollection AddCloudinary(this IServiceCollection services, IConfiguration configuration)
        {
            var account = new CloudinaryDotNet.Account(
                configuration["Cloudinary:Cloudname"],
                configuration["Cloudinary:ApiKey"],
                configuration["Cloudinary:ApiSecret"]
            );

            Cloudinary cloudinary = new Cloudinary(account);
            cloudinary.Api.Secure = true;

            services.AddSingleton(cloudinary);
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
        public static IServiceCollection AddApiBehavior(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    var errorMessage = errors.FirstOrDefault() ?? "Validation error";

                    // Create the response object
                    var errorResponse = new ErrorResponse
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Error = errorMessage,
                        TimeStamp = DateTime.UtcNow
                    };

                    return new ObjectResult(JsonConvert.SerializeObject(errorResponse, Formatting.Indented))
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                    };
                };
            });

            return services;
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(IConverter),new SynchronizedConverter(new PdfTools()));
            services.AddControllers();
        }

    }
}

