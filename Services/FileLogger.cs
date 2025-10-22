using System;
using System.IO;

namespace CMCS_WPF.Services
{
    public class FileLogger
    {
        private readonly string _folder;

        public FileLogger(string folder)
        {
            _folder = folder;
            Directory.CreateDirectory(folder);
        }

        public void Log(string level, string message, Exception? ex = null)
        {
            var logFile = Path.Combine(_folder, $"log_{DateTime.Now:yyyyMMdd}.txt");
            using var writer = new StreamWriter(logFile, true);
            writer.WriteLine($"{DateTime.Now:u} [{level}] {message}");
            if (ex != null) writer.WriteLine($"Exception: {ex}");
        }
    }
}
