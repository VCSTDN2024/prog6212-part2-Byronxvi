using System.Collections.Generic;

namespace CMCS_WPF
{
    public class AppSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; } = new ConnectionStrings();
        public Settings AppSettingsSection { get; set; } = new Settings();
    }

    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; } = string.Empty;
    }

    public class Settings
    {
        public string DataDirectory { get; set; } = "Data";
        public string LogDirectory { get; set; } = "Logs";
        public string UploadDirectory { get; set; } = "UploadedClaims";
    }
}
