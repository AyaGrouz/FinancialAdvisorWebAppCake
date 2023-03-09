using FinancialAdvisorWebApp.Hubs;
using FinancialAdvisorWebApp.Options;
using FinancialAdvisorWebApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Text;
using FinancialAdvisorWebApp.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;

namespace FinancialAdvisorWebApp
{
    public class Startup
    {
        readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) => _configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("IdentityConnection")));
            //services.AddDbContext<ApplicationContext>();
            services.AddControllersWithViews();

            services.Configure<TwilioSettings>(settings =>
            {
                settings.AccountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
                settings.ApiSecret = Environment.GetEnvironmentVariable("TWILIO_API_SECRET");
                settings.ApiKey = Environment.GetEnvironmentVariable("TWILIO_API_KEY");
                settings.AuthToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");
            })
                    .AddTransient<IVideoService, VideoService>()
                    .AddSpaStaticFiles(config => config.RootPath = "ClientApp/dist");

            services.AddSignalR();

            services.AddDbContext<AuthenticationContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("IdentityConnection")));

            services.AddDefaultIdentity<ApplicationUser>()
                .AddEntityFrameworkStores<AuthenticationContext>();
            services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
            });
            //JWT Authentication

            var key = Encoding.UTF8.GetBytes(_configuration["ApplicationSettings:JWT_Secret"].ToString());

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            

            app.UseHttpsRedirection()
               .UseStaticFiles()
               .UseSpaStaticFiles();

            app.UseRouting();
            app.UseCors(builder =>
                builder.AllowAnyHeader()
                    .AllowCredentials()
                    .AllowAnyMethod());
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");

                endpoints.MapHub<NotificationHub>("/notificationHub");
            })
               .UseSpa(spa =>
               {
                   spa.Options.SourcePath = "ClientApp";
                   spa.Options.StartupTimeout = new TimeSpan(0, 5, 0);

                   if (env.IsDevelopment())
                   {
                       spa.UseAngularCliServer(npmScript: "start");
                   }
               });


        }
    }
}