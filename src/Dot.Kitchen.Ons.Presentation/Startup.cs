using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dot.Kitchen.Ons.Application;
using Dot.Kitchen.Ons.Application.Commands;
using Dot.Kitchen.Ons.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Dot.Kitchen.Ons.Application.Interfaces;
using Dot.Kitchen.Ons.Application.Queries;

namespace Dot.Kitchen.Ons.Presentation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // retrieve from secrets.json (aka secret manager tool)
            var connString = Configuration["OnsConnectionString"];
            var groUsername = Configuration["GroUsername"];
            var groPassword = Configuration["GroPassword"];

            services.AddEntityFrameworkSqlServer().AddDbContext<DatabaseContext>(options => options.UseSqlServer(connString));
            services.AddScoped<IDatabaseContext>(provider => provider.GetService<DatabaseContext>());

            services.AddScoped<ISourceRepository, SourceRepository>();
            services.AddTransient<IGetSourceListQuery, GetSourceListQuery>();

            services.AddScoped<IScrapeRepository, ScrapeRepository>();
            services.AddTransient<IGetScrapeListQuery, GetScrapeListQuery>();
            services.AddTransient<IGetScrapeDetailQuery, GetScrapeDetailQuery>();
            services.AddTransient<ICreateSourceCommand, CreateSourceCommand>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
