using Dmd.Designer.Data;
using ElectronNET.API;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Dmd.Designer.Services;
using Dmd.Designer.Services.Canvas;
using Dmd.Designer.Services.File;
using Dmd.Designer.Services.Generator;
using Dmd.Designer.Services.Solution;
using ElectronNET.API.Entities;
using MediatR;

namespace Dmd.Designer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
            services.AddSingleton<ISolutionManager, SolutionManager>();
            services.AddScoped<IBrowserService, BrowserService>();
            services.AddTransient<ICanvasService, CanvasService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IGeneratorService, GeneratorService>();
            services.AddBlazorContextMenu();
            services.AddBlazorise(options =>
               {
                   options.ChangeTextOnKeyPress = true; // optional
               })
                .AddBootstrapProviders()
                .AddFontAwesomeIcons();

            services.AddMediatR(typeof(Startup));
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapFallbackToPage("/designer/{SolutionPath}", "/_Host");
            });

            Task.Run(async () => await Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions()
            {
                MinHeight = 600,
                MinWidth = 800,
                Height = 1200,
                Width = 1600
            }));
        }
    }
}
