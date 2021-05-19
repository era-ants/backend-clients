using System;
using System.IO;
using Clients.DataAccess;
using Clients.DatabaseMigrations;
using Clients.Model;
using Clients.Services;
using FluentMigrator.Runner;
using LinqToDB;
using LinqToDB.AspNet;
using LinqToDB.AspNet.Logging;
using LinqToDB.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Clients
{
    public class Startup
    {
        private readonly string _version = $"v{typeof(Startup).Assembly.GetName().Version!.ToString(2)}";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(_version, new OpenApiInfo {Title = "Clients API", Version = _version});
                var filePath = Path.Combine(AppContext.BaseDirectory, "Clients.xml");
                options.IncludeXmlComments(filePath);
            });

            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("DB_CONNECTION_STRING environment variable is not set");
            services
                .AddLinqToDbContext<AppDataConnection>((provider, builder) =>
                {
                    builder
                        .UsePostgreSQL(connectionString)
                        .UseDefaultLogging(provider);
                })
                ;

            services
                .AddFluentMigratorCore()
                .ConfigureRunner(builder => builder
                    .AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(InitialMigration).Assembly).For.Migrations())
                .AddLogging(builder => builder.AddFluentMigratorConsole())
                .BuildServiceProvider()
                ;

            services
                .AddScoped<IClientsService, DbClientsService>()
                .AddScoped<IStatisticsService, DbClientsService>()
                ;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"/swagger/{_version}/swagger.json", $"Clients API {_version}");
                options.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            using var scope = app.ApplicationServices.CreateScope();
            UpdateDatabase(app.ApplicationServices);
        }

        private void UpdateDatabase(IServiceProvider serviceProvider)
        {
            serviceProvider.GetRequiredService<IMigrationRunner>().MigrateUp();
        }
    }
}