using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BackEnd.Data;
using Swashbuckle.AspNetCore.Swagger;
using Npgsql;
using System.IO;

namespace BackEnd
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                }
                else
                {
                    //options.UseSqlite("Data Source=conferences.db");
                    var builder = new NpgsqlConnectionStringBuilder();
                    builder.Database = Configuration["DATABASE:NAME"];
                    builder.Host = Configuration["DATABASE:HOST"];
                    //TODO: If we ship the `file-per-value config` provider this could be made to 
                    //work like the others.
                    builder.Password = Configuration["DATABASE:PASSWORD"] ?? File.ReadAllText(Configuration["DATABASE:PASSWORDFILE"]);
                    builder.Port = int.Parse(Configuration["DATABASE:PORT"]);
                    builder.Username = Configuration["DATABASE:USER"];
                    options.UseNpgsql(builder.ToString());
                }
            });

            services.AddMvcCore()
                .AddJsonFormatters()
                .AddApiExplorer();

            services.AddSwaggerGen(options =>
                options.SwaggerDoc("v1", new Info { Title = "Conference Planner API", Version = "v1" })
            );
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(options =>
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Conference Planner API v1")
            );

            app.UseMvc();

            app.Run(context =>
            {
                context.Response.Redirect("/swagger");
                return Task.CompletedTask;
            });

            // Comment out the following line to avoid resetting the database each time
            NDCSydneyData.Seed(app.ApplicationServices);
        }
    }
}
