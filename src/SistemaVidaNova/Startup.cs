using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SistemaVidaNova.Services;
using SistemaVidaNova.Models;
using Microsoft.AspNetCore.Identity;

namespace SistemaVidaNova
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            /* if (env.IsDevelopment())
             {
                 // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                 builder.AddUserSecrets();
             }*/
            if (env.IsDevelopment())
            {

            }

                builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            //var connection = @"Server=GUERRA-PC\SQLEXPRESS;Database=VidaNovaDB;Trusted_Connection=True;";
            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<VidaNovaContext>(options => options.UseSqlServer(connection));

            services.AddIdentity<Usuario, IdentityRole>()
                .AddEntityFrameworkStores<VidaNovaContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddDeveloperIdentityServer()
                .AddInMemoryScopes(Config.GetScopes())
                .AddInMemoryClients(Config.GetClients())
                .AddAspNetIdentity<Usuario>();

            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            InitializeDatabase(app);

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();
            app.UseIdentityServer();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });




        }


        private async void InitializeDatabase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<VidaNovaContext>().Database.Migrate();

                var context = scope.ServiceProvider.GetRequiredService<VidaNovaContext>();
                UserManager<Usuario> _userManager = scope.ServiceProvider.GetRequiredService<UserManager<Usuario>>();                
                context.Database.Migrate();
                if (!context.Users.Where(q => q.Email == "admin@admin.com").Any())
                {
                    var user = new Usuario
                    {
                        Nome = "Admin",
                        UserName = "admin@admin.com",
                        Email = "admin@admin.com",
                        Cpf = "83577880171"
                    };
                    var result = await _userManager.CreateAsync(user, "Admin1@");
                    //context.SaveChanges();
                }

                if (!context.Roles.Any()) {
                    IdentityRole ir = new IdentityRole("Administrator");
                    context.Roles.Add(ir);
                    ir = new IdentityRole("User");
                    context.Roles.Add(ir);
                    context.SaveChanges();
                }

                try
                {
                    Usuario admin = context.Users.Single(q => q.UserName == "admin@admin.com");
                    IdentityRole role = context.Roles.Single(q => q.Name == "Administrator");
                    admin.Roles.Add(new IdentityUserRole<string>() { RoleId = role.Id, UserId = admin.Id });
                    context.SaveChanges();
                }
                catch { }


            }
        }

    }
}
