using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PictureLibraryModel.Services;

namespace PictureLibrary_RaspberryAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var fileSystemService = new FileSystemService();
            System.Console.WriteLine(fileSystemService.GetAllDirectories(".",System.IO.SearchOption.TopDirectoryOnly).Count);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
