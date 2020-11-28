using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using toDoList.Models;

namespace toDoList
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddMvc(options => options.EnableEndpointRouting = false).AddXmlSerializerFormatters();
            services.AddSingleton<IUserRepository,MockUserRepository>();
            services.AddSingleton<IUserRoleRepository, MockUserRoleRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else if (env.IsStaging() || env.IsProduction() || env.IsEnvironment("UAT"))
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            //app.UseMvcWithDefaultRoute();

            app.UseMvc(routes => {
            routes.MapRoute("default","{controller=Home}/{action=Index}/{id?}");
            });

            //app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name:"default",
            //        pattern:"{controller=Home}/{action=Index}/{id?}");


            //    //endpoints.MapGet("/", async context =>
            //    //{
            //    //    //await context.Response.WriteAsync("hosting environment: " + env.EnvironmentName);
            //    //    await context.Response.WriteAsync("Hello World!");
            //    //});
            //});

        }
    }
}
