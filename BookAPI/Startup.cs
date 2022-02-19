using BookAPI.Apps.AdminApi.DTOs.BookDtos;
using BookAPI.Apps.AdminApi.Profiles;
using BookAPI.Apps.UserApi.Profiles;
using BookAPI.Data.DAL;
using BookAPI.Data.Entity;
using BookAPI.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookAPI
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
            services.AddControllers().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<BookPostDto>());
            services.AddDbContext<BookDbContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("Default"));
            });
            services.AddAutoMapper(opt =>
            {
                opt.AddProfile(new MapProfile());
               

            });
            services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 8;
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<BookDbContext>();

            services.AddAutoMapper(opt =>
            {
                opt.AddProfile(new MapProfile());
                opt.AddProfile(new UserApiProfile());

            });

            services.AddScoped<IJwtService, JwtService>();

            services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidIssuer = Configuration.GetSection("JWT:issuer").Value,
                        ValidAudience = Configuration.GetSection("JWT:audience").Value,
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration.GetSection("JWT:secret").Value))
                    };
                });


            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("admin_v1", new OpenApiInfo
            //    {
            //        Title = "Employee API",
            //        Version = "admin_v1",
            //        Description = "An API to perform Employee operations",
            //        TermsOfService = new Uri("https://example.com/terms"),
            //        Contact = new OpenApiContact
            //        {
            //            Name = "John Walkner",
            //            Email = "John.Walkner@gmail.com",
            //            Url = new Uri("https://twitter.com/jwalkner"),
            //        },
            //        License = new OpenApiLicense
            //        {
            //            Name = "Employee API LICX",
            //            Url = new Uri("https://example.com/license"),
            //        }
            //    });

            //    c.SwaggerDoc("user_v1", new OpenApiInfo
            //    {
            //        Title = "Shop User API",
            //        Version = "user_v1",
            //        Description = "An API to perform Employee operations",
            //        TermsOfService = new Uri("https://example.com/terms"),
            //        Contact = new OpenApiContact
            //        {
            //            Name = "John Walkner",
            //            Email = "John.Walkner@gmail.com",
            //            Url = new Uri("https://twitter.com/jwalkner"),
            //        },
            //        License = new OpenApiLicense
            //        {
            //            Name = "Employee API LICX",
            //            Url = new Uri("https://example.com/license"),
            //        }
            //    });


            //    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            //    {
            //        In = ParameterLocation.Header,
            //        Description = "Please insert JWT with Bearer into field",
            //        Name = "Authorization",
            //        Type = SecuritySchemeType.ApiKey
            //    });
            //    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
            //    {
            //     new OpenApiSecurityScheme
            //     {
            //       Reference = new OpenApiReference
            //       {
            //         Type = ReferenceType.SecurityScheme,
            //         Id = "Bearer"
            //       }
            //      },
            //      new string[] { }
            //    }
            //    });
            //    c.AddFluentValidationRulesScoped();

            //    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            //    c.IncludeXmlComments(xmlPath);
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            //app.UseSwagger(c =>
            //{
            //    c.RouteTemplate = "api/swagger/{documentName}/swagger.json";
            //});

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //app.UseSwaggerUI(x =>
            //{
            //    x.SwaggerEndpoint("/api/swagger/user_v1/swagger.json", "User API  V1");
            //    x.SwaggerEndpoint("/api/swagger/admin_v1/swagger.json", "Admin API V1");
            //    x.RoutePrefix = "api/swagger";
            //});
        }
    }
}
