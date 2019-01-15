#region --Using--
using System.Text;
using System.Net;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using DAL.Context;
using Service.Interfaces;
using Service;
using Core;
using DAL;
using WebAPI.Helpers;
using AutoMapper;
#endregion

namespace WebAPI
{
    public class Startup
    {
        private const string ConnectionString = "DefaultConnection";
        private const string SecretJWTSalt = "AppSettings:Token";

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(o => { o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; });
            services.AddDbContext<DatingAppContext>(_ => _.UseSqlServer(Configuration.GetConnectionString(ConnectionString)));
            services.AddCors();
            services.AddAutoMapper();
            services.AddTransient<Seed>();
            services.AddScoped<IUnityOfWork, UnityOfWork>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => 
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true, 
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection(SecretJWTSalt).Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Seed seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder => {
                     builder.Run(async context => {
                         context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                         var exception = context.Features.Get<IExceptionHandlerFeature>();
                         
                         if( exception != null)
                         {
                            context.Response.AddAplicationError(exception.Error.Message);
                            await context.Response.WriteAsync(exception.Error.Message);
                         }
                     });
                });
                //app.UseHsts();
            }


            //seeder.SeedUsers();
            //app.UseHttpsRedirection();
            app.UseCors(
                x => x.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
