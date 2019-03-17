using AutoMapper;
using BalticAmadeusTask.Data;
using BalticAmadeusTask.Helpers;
using BalticAmadeusTask.Models;
using BalticAmadeusTask.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BalticAmadeusTask
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddFluentValidation();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddAutoMapper(); 

            //TODO: HttpFactory
            //services.AddHttpClient( /* etc */);

            services.AddOptions();
            services.Configure<CRUDApiConfig>(Configuration.GetSection("CRUDApiConfig"));
            services.AddSingleton<IPasswordPolicyService, PasswordPolicyService>();
            services.AddSingleton<IHashingService, HashingService>();

            // Validators
            services.AddScoped<IValidator<RegisteredUser>, RegisteredUserValidator>();

            //services.AddDbContext<BalticAmadeusTaskContext>(context => { context.UseInMemoryDatabase("inMemoryDB"); });
            services.AddDbContext<BalticAmadeusTaskContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("BalticAmadeusTaskContext")));

            services.AddHttpClient<IRegisteredUserCRUDApiService, RegisteredUserCrudApiService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "ActionApi",
                    template: "api/{controller}/{action}/{id}");
            });
        }
    }
}
