using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UAParser;

namespace Middleware
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
            services.AddRazorPages();
            services.AddMvc();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.Map("/Privacy", Browser);
            app.UseWhen(context => (context.Request.Query.ContainsKey("Privacy")),
                              (IApplicationBuilder applicationBuilder) =>

                              {
                                  app.Use(async (context, next) =>
                                  {
                                      string user_agent = context.Request.Headers["User-Agent"].ToString();
                                      await context.Response.WriteAsync("<p>" + user_agent + "</p>");

                                  });
                              });


            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();
           
            

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
           
        }
        private static void Komunikat(IApplicationBuilder app)
        {
            

        }

        private static void Browser(IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                string user_agent = context.Request.Headers["User-Agent"].ToString();
                var uaParser = Parser.GetDefault();
                ClientInfo c = uaParser.Parse(user_agent);
                //Edge, EdgeChromium i IE
                string browser = c.UA.Family;
                if (browser.Contains("Edge") || browser.Contains("IE"))
                {
                    await context.Response.WriteAsync("Przegl¹darka nie jest obs³ugiwana");
                }
                else
                {
                    
                    
                    
                    
                }

            });

        }
    }
}
