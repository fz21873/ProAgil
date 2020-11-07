
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ProAgil.Repository;
using AutoMapper;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;

namespace ProAgil.WebAPI
{
    public class Startup
    {
         
        public Startup(IConfiguration configuration) 
        {
            this.Configuration = configuration;
               
        }
        
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

           /* var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("SecretKey"));
            services.AddAuthentication(x=>{
                x.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x=>{
                x.RequireHttpsMetadata=false;
                x.SaveToken=true;
                x.TokenValidationParameters=new TokenValidationParameters
                {
                    ValidateIssuerSigningKey=true,
                    IssuerSigningKey=new SymmetricSecurityKey(key),
                    ValidateIssuer=false,
                    ValidateAudience=false,


                };
            });*/
    
            services.AddDbContext <ProAgilContext>(
                            x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            services.AddAutoMapper();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddScoped<IProAgilRepository,ProAgilRepository>();
            services.AddCors();
            
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

           // app.UseHttpsRedirection();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions(){
                 FileProvider = new  PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),@"Resources")),
                 RequestPath = new PathString("/Resources")
             });
           // app.UseAuthentication();
            app.UseMvc();
        }
    }
}
