using Autofac.Extensions.DependencyInjection;
using HomeVideo.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HomeVideo.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

            if (File.Exists(unixsocket))
            {
                File.Delete(unixsocket);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var c = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory).AddJsonFile("appsettings.json").Build();
            unixsocket = c.GetSection("unixsocket")?.Value?.ToLower();

            if (!string.IsNullOrEmpty(unixsocket))
            {
                var path = Path.GetDirectoryName(unixsocket);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }

            return Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseConfiguration(c);
                    if (!string.IsNullOrEmpty(unixsocket))
                    {
                        webBuilder.ConfigureKestrel(option =>
                        {
                            option.ListenUnixSocket(unixsocket);
                        });
                    }
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseKestrel();
                });
        }

        private static string unixsocket;
    }
}
