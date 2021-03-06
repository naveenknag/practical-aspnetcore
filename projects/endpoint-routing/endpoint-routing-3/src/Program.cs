using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace LinkGeneratorSample
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env, ILoggerFactory logger)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory logger, IConfiguration configuration)
        {
            app.Use(async (context, next) =>
            {
                var linkGenerator = context.RequestServices.GetService<LinkGenerator>();

                var url = linkGenerator.GetUriByAction(context,
                       controller: "Hello",
                       action: "World"
                   );

                var url2 = linkGenerator.GetUriByAction(context,
                       controller: "Hello",
                       action: "Goodbye"
                   );

                var url3 = linkGenerator.GetUriByAction(context,
                       controller: "Hello",
                       action: "CallMe"
                   );

                var url4 = linkGenerator.GetUriByAction(context,
                       controller: "Greet",
                       action: "Index"
                   );

                var url5 = linkGenerator.GetUriByAction(context,
                       controller: "Wave",
                       action: "Away"
                   );

                var url6 = linkGenerator.GetUriByAction(context,
                       controller: "XXXX",
                       action: "YYYY"
                   );

                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync($@"Generated Url:
{url}
{url2}
{url3}
{url4}
{url5}
{url6}(It won't produce any link if it cannot figure out controller and action information)");
            });

            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }

    [Route("[controller]")]
    public class HelloController
    {
        [HttpGet("")]
        public ActionResult World() => null;

        [HttpGet("Goodbye")]
        public ActionResult Goodbye() => null;

        [HttpGet("[action]")]
        public ActionResult CallMe() => null;
    }

    [Route("Greet")]
    public class GreetController
    {
        public ActionResult Index() => null;
    }

    public class WaveController
    {
        [Route("Wave-Away")]
        public ActionResult Away() => null;
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                    webBuilder.UseStartup<Startup>()
                );
    }
}