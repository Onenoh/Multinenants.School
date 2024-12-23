using Infrastructure;
using Infrastructure.Persistence;
using Application;

namespace WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddApplicationService();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddInfrastructureServices(builder.Configuration);

            var app = builder.Build();

            await app.Services.AddDatabaseInitializerAsync();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            //app.UseAuthorization();
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.MapControllers();

            app.UseInfrastructure();

            app.Run();
        }
    }
}
