using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Data.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using DatingApp.API.Helpers;

namespace DatingApp.API
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // A connection String usada neste contexto.
            services.AddDbContext<DataContext>(_ => _.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            
            // Para poder fazer requests de Contextos diferentes.
            services.AddCors();

            // Injeção de dependência.
            services.AddScoped<IAuthRepository, AuthRepository>();

            // Faz com que o atributo Authorize faça validação de Jwt
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => 
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true, 
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
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
                // Para lidar com erros sem a developer page e sem o try catch em cada action.
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

            //app.UseHttpsRedirection();
            app.UseCors(
                x => x.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );
            app.UseMvc();
        }
    }
}
