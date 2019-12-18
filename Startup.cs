using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;


namespace aspnetcore_demo
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration config)
        {
            if(env.IsDevelopment()){
                app.UseDeveloperExceptionPage();
            }else{
                app.UseExceptionHandler("/Error");
            }
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
               endpoints.MapGet("/ThrowException/", async context =>
                {
                    await Task.FromException(new InvalidOperationException("無效的操作"));
                });
                endpoints.MapGet("/Error/", async context =>
                {
                    await context.Response.WriteAsync("Error");
                });
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!" +config.GetValue<string>("Message"));
                });
            });        
        }
    }
}