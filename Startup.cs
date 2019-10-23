using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Supermarket.Models;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Supermarket.API
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
            services.AddDbContext<SupermarketContext>(opt =>
               opt.UseInMemoryDatabase("Products"));
            services.AddControllers();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Supermarket Api",
                    Version = "v1",
                    Description = "A simple supermarket ASP.NET Core Web API",
                    Contact = new OpenApiContact
                    {
                        Name = "Jakub Wielgus",
                        Url = new Uri("https://github.com/Wielqus"),
                    },
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme(){ 
                    In = ParameterLocation.Header, 
                    Description = "Please insert JWT with Bearer into field", 
                    Name = "Authorization", 
                    Type = SecuritySchemeType.ApiKey 
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement { { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, new string[] { } } });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddTransient((config) =>
            {
                var conf = new JWTConfiguration();
                Configuration.GetSection("JWTConfiguration").Bind(conf);
                return conf;
            });

            services.AddTransient((config) =>
            {
                var conf = new User();
                Configuration.GetSection("User").Bind(conf);
                return conf;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            ConfigureJwt(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication(); 

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureJwt(IServiceCollection services)
        {
            var config = services.BuildServiceProvider().GetService<JWTConfiguration>();
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.SecretKey));
            var tokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = signingKey,
                ValidIssuer = config.ValidIssuer,
                ValidAudience = config.ValidAudience
            };
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(c =>
            {
                c.RequireHttpsMetadata = false;
                c.SaveToken = true;
                c.TokenValidationParameters = tokenValidationParameters;
            });

        }
    }
}
