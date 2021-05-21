using System;
using System.IO;
using Clients.DatabaseMigrations;
using Clients.Hubs;
using Clients.Services;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Npgsql;

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
            services.AddSignalR();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(_version, new OpenApiInfo {Title = "Clients API", Version = _version});
                var filePath = Path.Combine(AppContext.BaseDirectory, "Clients.xml");
                options.IncludeXmlComments(filePath);
            });

            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("DB_CONNECTION_STRING environment variable is not set");
            services.AddScoped(_ => new NpgsqlConnection(connectionString));

            services
                .AddFluentMigratorCore()
                .ConfigureRunner(builder => builder
                        .AddPostgres11_0()
                        .WithGlobalConnectionString(connectionString)
                        .WithMigrationsIn(typeof(InitialMigration).Assembly)
                    // .ScanIn(typeof(InitialMigration).Assembly).For.Migrations()
                )
                .AddLogging(builder => builder.AddFluentMigratorConsole())
                .BuildServiceProvider()
                ;

            services
                .AddScoped<IClientsService, PostgresDbClientsService>()
                .AddScoped<IStatisticsService, PostgresDbClientsService>()
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
            app.UseWebSockets(new WebSocketOptions{KeepAliveInterval = TimeSpan.FromMinutes(5)});

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ClientsHub>("/clientshub");
            });

            using var scope = app.ApplicationServices.CreateScope();
            UpdateDatabase(app.ApplicationServices);
        }

        private void UpdateDatabase(IServiceProvider serviceProvider)
        {
            serviceProvider.GetRequiredService<IMigrationRunner>().MigrateUp();
        }
    }
}