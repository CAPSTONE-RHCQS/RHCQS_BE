using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;
using RHCQS.BusinessObject.Constants;
using RHCQS_BE.Extenstion;
using RHCQS_Services.Implement;
using System.Text.Json.Serialization;

namespace RHCQS_BE
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.ClearProviders();

            var currentDirectory = System.AppDomain.CurrentDomain.BaseDirectory;

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: CorsConstant.PolicyName,
                   policy =>
                   {
                       policy.WithOrigins("http://localhost:3000")
                             .WithOrigins("http://localhost:5173")
                             .WithOrigins("http://localhost:8081")
                             .WithOrigins("https://rhcqs.vercel.app")
                             .AllowAnyHeader()
                             .AllowAnyMethod()
                             .AllowCredentials();
                   });
            });

            builder.Services.AddControllers().AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                x.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            });

            var configuration = builder.Configuration;

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddDatabase();
            builder.Services.AddDatabase();
            builder.Services.AddUnitOfWork();
            builder.Services.AddServices();
            builder.Services.AddCloudinary(builder.Configuration);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddConfigSwagger();
            builder.Services.AddSingletonJson();
            builder.Services.AddJwtValidation(configuration);
            builder.Services.AddSignalRServices();
            builder.Services.AddFirebase(builder.Configuration);

            var app = builder.Build();

            // Enable Swagger
            app.UseSwagger();
            app.UseSwaggerUI();

            // Middleware for Exception and Authorization Handling
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<AuthorizationHandlingMiddleware>();

            // Enable CORS with the defined policy
            app.UseCors(CorsConstant.PolicyName);
            //app.UseHttpsRedirection();

            app.UseRouting();
            // Authentication and Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatService>("/chatHub");
            });

            app.Run();
        }
    }
}
