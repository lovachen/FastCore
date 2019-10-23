using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cts.web.core;
using Fast.Framework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Targets;

namespace Fast.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment)
        {
            Hosting = webHostEnvironment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Hosting { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var engine = EngineContext.Create(new FastEngine(Configuration, Hosting));
            engine.Initialize(services);
            var serviceProvider = engine.ConfigureServices(services);

            return serviceProvider;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            //NLog 数据库连接
            LogManager.Configuration.FindTargetByName<DatabaseTarget>("connectionString").ConnectionString 
                = Configuration.GetConnectionString("DefaultConnection");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
