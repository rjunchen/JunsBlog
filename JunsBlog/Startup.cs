using JunsBlog.Helpers;
using JunsBlog.Interfaces;
using JunsBlog.Interfaces.Services;
using JunsBlog.Interfaces.Settings;
using JunsBlog.Models.Services;
using JunsBlog.Models.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JunsBlog
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
            services.AddCors();
            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.Configure<JunsBlogDatabaseSettings>(Configuration.GetSection(nameof(JunsBlogDatabaseSettings)));
            services.AddSingleton<IJunsBlogDatabaseSettings>(sp => sp.GetRequiredService<IOptions<JunsBlogDatabaseSettings>>().Value);

            var jwtSettingSection = Configuration.GetSection(nameof(JwtSettings));
            services.Configure<JwtSettings>(jwtSettingSection);
            services.AddSingleton<IJwtSettings>(sp => sp.GetRequiredService<IOptions<JwtSettings>>().Value);

            services.Configure<EmailSettings>(Configuration.GetSection(nameof(EmailSettings)));
            services.AddSingleton<IEmailSettings>(sp => sp.GetRequiredService<IOptions<EmailSettings>>().Value);

            services.Configure<AuthenticationSettings>(Configuration.GetSection(nameof(AuthenticationSettings)));
            services.AddSingleton<IAuthenticationSettings>(sp => sp.GetRequiredService<IOptions<AuthenticationSettings>>().Value);

            services.AddScoped<IJwtTokenHelper, JwtTokenHelper>();
            services.AddScoped<INotificationService, EmailNotificationService>();
            services.AddScoped<IDatabaseService, MongoDBService>();

            services.AddHttpContextAccessor();

            var jwtSettings = jwtSettingSection.Get<JwtSettings>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(Options =>
            {
                Options.RequireHttpsMetadata = false;
                Options.SaveToken = true;
                Options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.TokenSecret))
                };
            });
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseAuthentication();

            app.UseRouting();

            // Global CORS policy
            app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                     // spa.UseAngularCliServer(npmScript: "start");

                    // Use the Proxy when starting the angular ClientApp externally
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
        }
    }
}
