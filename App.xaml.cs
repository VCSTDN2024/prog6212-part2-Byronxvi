using CMCS_WPF.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Windows;

namespace CMCS_WPF
{
    public partial class App : Application
    {
        public static IConfiguration Configuration { get; private set; } = null!;
        public static FileLogger Logger { get; private set; } = null!;
        public static StorageService Storage { get; private set; } = null!;
        public static ClaimService ClaimService { get; private set; } = null!;
        public static AppSettings Settings { get; private set; } = new AppSettings();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();

            
            Settings.ConnectionStrings.DefaultConnection = Configuration["ConnectionStrings:DefaultConnection"];
            Settings.AppSettingsSection.DataDirectory = Configuration["AppSettings:DataDirectory"];
            Settings.AppSettingsSection.LogDirectory = Configuration["AppSettings:LogDirectory"];
            Settings.AppSettingsSection.UploadDirectory = Configuration["AppSettings:UploadDirectory"];

            
            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Settings.AppSettingsSection.DataDirectory));
            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Settings.AppSettingsSection.LogDirectory));
            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Settings.AppSettingsSection.UploadDirectory));

           
            Logger = new FileLogger(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Settings.AppSettingsSection.LogDirectory));
            Storage = new StorageService(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Settings.AppSettingsSection.DataDirectory));
            ClaimService = new ClaimService(Logger, Storage);
        }
    }
}

