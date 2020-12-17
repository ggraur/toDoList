using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using toDoList.Models;
using toDoList.Security;

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
            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(_config.GetConnectionString("toDoListDBConnection")));

            services.AddDbContextPool<AGesContext>(
             options => options.UseSqlServer(_config.GetConnectionString("aGesConnection")));


            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 10;
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.SignIn.RequireConfirmedEmail = true;

                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(1);

            }).AddEntityFrameworkStores<AppDbContext>()
              .AddDefaultTokenProviders();

       


            var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

            services.AddMvc(options =>
            {
                options.Filters.Add(new AuthorizeFilter(policy));
                options.EnableEndpointRouting = false;
            }).AddXmlSerializerFormatters();

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
            });

            services.AddAuthorization(options =>
            {
                //options.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role", "true")
                //                                                      .RequireRole("Administrator")
                //                                                      .RequireRole("Super Admin")
                //);

                //options.AddPolicy("EditRolePolicy", policy => policy.RequireClaim("Edit Role", "true")
                //                                                    .RequireRole("Administrator")
                //                                                    .RequireRole("Super Admin"));

                //options.AddPolicy("EditRolePolicy", policy => policy.RequireAssertion(context =>
                //              context.User.IsInRole("Administrator") &&
                //              context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") ||
                //              context.User.IsInRole("Super Admin")
                //    ));
               
                options.AddPolicy("DeleteRolePolicy", policy => policy.RequireAssertion(context =>
                             context.User.IsInRole("Administrator") &&
                             context.User.HasClaim(claim => claim.Type == "Delete Role" && claim.Value == "true") ||
                             context.User.IsInRole("Super Admin")
                  ));

                options.AddPolicy("EditRolePolicy", policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));

                options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("Administrator"));
                options.AddPolicy("SuperAdminRolePolicy", policy => policy.RequireRole("Super Admin"));
                  
                options.AddPolicy("ManagerRights",
                    policy => policy.RequireAssertion(context =>
                              context.User.IsInRole("Administrator") ||
                              context.User.IsInRole("Super Admin")));
                  
            });

           // services.AddScoped<IUserRepository, SQLUserRepository>();
            //services.AddScoped<ITaskRepository, SQLTaskRepository>();
            //services.AddScoped<IToDoListRepository, SQLToDoRepository>();
            services.AddScoped<IForgotPassword, SQL_IForgotPassword>();
            services.AddScoped<IConConfig, SQL_ConConfig>();
            services.AddScoped<IEmpresa, SQL_IEmpresa>();
            services.AddScoped<IEmpresaUtilizadores, SQL_IEmpresaUtilizadores>();
            services.AddScoped<IEmpresaAGes,SQL_IEmpresaAGes>();


            //services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();
            services.AddSingleton<IAuthorizationHandler, AdministratorHandler>();
            


            //services.AddSingleton<IUserRepository,MockUserRepository>();// memory repository used for test

            // services.AddSingleton<IUserRoleRepository, MockUserRoleRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else //if (env.IsStaging() || env.IsProduction() || env.IsEnvironment("UAT"))
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }

            app.UseStaticFiles();
            //app.UseMvcWithDefaultRoute();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
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
