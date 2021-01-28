﻿using AntDesign.Pro.Layout;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NewLife.CubeUI.Services;
using NewLife.Remoting;
using NewLife.Log;

namespace NewLife.CubeUI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(
                //builder.HostEnvironment.BaseAddress
                "http://81.69.253.197:8000"
                )
            });

            //XTrace.Log = new CubeUILog();
            var apiHttpClient = new ApiHttpClient("http://81.69.253.197:8000")
            {
                Log = null// XTrace.Log
            };

            builder.Services.AddSingleton(apiHttpClient);
            builder.Services.AddAntDesign();
            builder.Services.Configure<ProSettings>(builder.Configuration.GetSection("ProSettings"));

            builder.Services.AddScoped<IChartService, ChartService>();
            builder.Services.AddScoped<IProjectService, ProjectService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IProfileService, ProfileService>();
            builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();

            var host = builder.Build();

            var accountService = host.Services.GetRequiredService<IAccountService>();
            await accountService.Initialize();

            await host.RunAsync();
        }
    }

    public class CubeUILog : NewLife.Log.ILog
    {
        public Boolean Enable { get; set; }
        public LogLevel Level { get; set; }

        public void Debug(String format, params Object[] args) { }
        public void Error(String format, params Object[] args) { }
        public void Fatal(String format, params Object[] args) { }
        public void Info(String format, params Object[] args) { }
        public void Warn(String format, params Object[] args) { }
        public void Write(LogLevel level, String format, params Object[] args) { }
    }
}