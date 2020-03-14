using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace aspnetcore_demo {
    public class Startup {
        public void ConfigureServices (IServiceCollection services) {
            services.AddDbContext<ProductContext>(options => options.UseSqlite("Data Source=product.db"));

            var configBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("test.json", optional: true);
            var config = configBuilder.Build();
            services.Configure<TestOptions>(config);
        }

        public void Configure (IApplicationBuilder app, IWebHostEnvironment env, IConfiguration config,ILogger<Startup> logger, ProductContext productContext, IOptions<TestOptions> testOptions) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            } else {
                app.UseExceptionHandler ("/Error");
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles (); // For the wwwroot folder

            //利用PhysicalFileProvider讀取靜態檔案
            app.UseStaticFiles (new StaticFileOptions {
                FileProvider = new PhysicalFileProvider (
                        Path.Combine (Directory.GetCurrentDirectory (), "MyStaticFiles")),
                    RequestPath = "/StaticFiles"
            });
            //目錄瀏覽
            app.UseDirectoryBrowser (new DirectoryBrowserOptions {
                FileProvider = new PhysicalFileProvider (
                        Path.Combine (Directory.GetCurrentDirectory (), "wwwroot", "images")),
                    RequestPath = "/MyImages"
            });
            app.UseStaticFiles (new StaticFileOptions {
                FileProvider = new PhysicalFileProvider (
                        Path.Combine (Directory.GetCurrentDirectory (), "wwwroot", "images")),
                    RequestPath = "/MyImages"
            });

            var productCount=productContext.Products.Count();
            app.UseRouting ();
            app.UseEndpoints (endpoints => {
                endpoints.MapGet ("/ThrowException/", async context => {
                    await Task.FromException (new InvalidOperationException ("無效的操作"));
                });
                endpoints.MapGet ("/Error/", async context => {
                    await context.Response.WriteAsync ("Error");
                });
                endpoints.MapGet ("/", async context => {
                    logger.LogInformation("Test Log");
                    await context.Response.WriteAsync ("Hello World!" + config.GetValue<string> ("Message")+ productCount+ testOptions.Value.TestOption1 + testOptions.Value.TestOption2);
                });
            });
        }
    }
}