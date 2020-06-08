using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackErp.DB;
using StackErp.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackErp.Model.Utils;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using StackErp.UI;
using StackErp.Model;

namespace StackErp.App
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
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                    {
                        // Use the default property (Pascal) casing
                        options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                        
                        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                        options.SerializerSettings.ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace;
                        options.SerializerSettings.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore;
                        options.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include;
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize;
                        options.SerializerSettings.Error = (serializer, args) => args.ErrorContext.Handled = true;
                        options.SerializerSettings.StringEscapeHandling = Newtonsoft.Json.StringEscapeHandling.EscapeHtml;
                        
                    });

             services.Configure<Microsoft.AspNetCore.Server.Kestrel.Core.KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            var appSettingsSection = Configuration.GetSection("AppKeySettings");
            services.Configure<AppKeySetting>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppKeySetting>();
            var key = Encoding.ASCII.GetBytes(appSettings.JWTSecret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {  
                endpoints.MapControllers();
                              
                // endpoints.MapControllerRoute(
                //     name: "web",
                //     pattern: "web/{c?}/{a?}",
                //     defaults: new { controller = "App", action = "Index" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Entity}/{action=Index}/{id?}");
            });
            
            //app.InitStackApp(this.Configuration);
            StackErp.Core.App.ConfigureDB(this.Configuration.GetValue<string>("DBInfo:ConnectionString"));

            EntityMetaData.Build();
        }
    }
}
