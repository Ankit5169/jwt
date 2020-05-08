using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OrgDAL;

namespace jwt
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
            //services.AddMvc(x => x.Filters.Add(new AuthorizeFilter())).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            //services.AddDbContext<OrganizationDbContext>();
            //services.AddSwaggerDocument();
            //services.AddIdentity<IdentityUser, IdentityRole>()
            //    .AddEntityFrameworkStores<OrganizationDbContext>()
            //    .AddDefaultTokenProviders();

            //services.ConfigureApplicationCookie(opt =>
            //{
            //    opt.Events = new CookieAuthenticationEvents
            //    {
            //        OnRedirectToLogin = redirectContext =>
            //        {
            //            redirectContext.HttpContext.Response.StatusCode = 401;
            //            return Task.CompletedTask;
            //        }
            //    };
            //});
            services.AddMvc().AddXmlSerializerFormatters()
                              .AddXmlDataContractSerializerFormatters();
            services.AddDbContext<OrganizationDbContext>();

            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<OrganizationDbContext>()
                    .AddDefaultTokenProviders();

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this-is-my-secret-key"));
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = signingKey
            };

            services.AddAuthentication(x => x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(jwt => { jwt.TokenValidationParameters = tokenValidationParameters; });

            //services.ConfigureApplicationCookie(opt =>
            //{
            //    opt.Events = new CookieAuthenticationEvents
            //    {
            //        OnRedirectToLogin = redirectContext =>
            //        {
            //            redirectContext.HttpContext.Response.StatusCode = 401;
            //            return Task.CompletedTask;
            //        },
            //        OnRedirectToAccessDenied = redirectContext =>
            //        {
            //            redirectContext.HttpContext.Response.StatusCode = 401;
            //            return Task.CompletedTask;
            //        }
            //    };
            //});
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseAuthentication();
        }
    }
}
