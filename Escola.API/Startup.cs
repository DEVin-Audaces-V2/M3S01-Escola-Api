using Escola.API.Config;
using Escola.API.DataBase;
using Escola.API.DataBase.Repositories;
using Escola.API.Interfaces.Repositories;
using Escola.API.Interfaces.Services;
using Escola.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Escola.API
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
            services.AddMvc(config =>
            {
                config.ReturnHttpNotAcceptable = true;
                config.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                config.InputFormatters.Add(new XmlDataContractSerializerInputFormatter(config));
            });


            services.AddControllers();

            //Services
            services.AddScoped<IAlunoService,AlunoService>();
            services.AddScoped<IBoletimService,BoletimService>();
            services.AddScoped<IAutenticacaoServices, AutenticacaoServices>();

            //Repositorios
            services.AddDbContext<EscolaDbContexto>();
            services.AddScoped<IAlunoRepository, AlunoRepository>();
            services.AddScoped<ITurmaRepository, TurmaRepository>();
            services.AddScoped<IBoletimRepository, BoletimRepository>();


            //Cache
            services.AddMemoryCache();

            var jwtChave = Configuration.GetSection("jwtTokenChave").Get<string>();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtChave)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            }); ;

            //swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Escola.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Escola.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(config =>
            {
                //config.AllowAnyOrigin();
                config.WithOrigins("*");
            });

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseMiddleware<ErrorMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
