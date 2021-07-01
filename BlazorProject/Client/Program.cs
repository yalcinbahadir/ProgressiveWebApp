using BlazorProject.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Syncfusion.Blazor;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazorProject.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            string key = "NDYzNzc3QDMxMzkyZTMxMmUzMEQ1a3ZTa2VmWGJEaTd5VTA1TjhsUGsrV1F0VWVBQUR3SC9CK0FBdUJCMEU9";
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(key);

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");       
            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services
                .AddHttpClient<IEmployeeService, EmployeeService>(
                EmployeeService.clientName,
                conf => { conf.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); }
                );
            builder.Services.AddSyncfusionBlazor();
            await builder.Build().RunAsync();  
        }
        //BaseAddress="https://localhost:44332/"
    }
}
