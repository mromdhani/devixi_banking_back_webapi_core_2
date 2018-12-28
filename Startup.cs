using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MonProjetBanking_Back.DataAccess;

namespace MonProjetBanking_Back
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
 
           services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)  
    .AddJwtBearer(options =>  
    {  
        options.TokenValidationParameters = new TokenValidationParameters  
        {  
            ValidateIssuer = true,  
            ValidateAudience = true,  
            ValidateLifetime = true,  
            ValidateIssuerSigningKey = true,  
            ValidIssuer = Configuration["Jwt:Issuer"],  
            ValidAudience = Configuration["Jwt:Issuer"],  
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))  
        };  
    });  
            services.AddCors();
            services.AddDbContext<ComptesContext>(opt =>
                 opt.UseInMemoryDatabase("ComptesList"));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

         services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                                policy.RequireClaim("Pouvoir", "admin"));
                options.AddPolicy("UserOnly", policy =>
                                policy.RequireClaim("Pouvoir", "user"));
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

        app.UseCors(builder =>
           builder.WithOrigins("http://localhost:4201"));

            //app.UseHttpsRedirection();
            app.UseAuthentication();  
            app.UseMvc();
            
        }
    }
}
