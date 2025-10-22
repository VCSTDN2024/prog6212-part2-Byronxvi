using CMCS_WPF.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace CMCS_WPF.Services
{
    public class StorageService
    {
        private readonly string _folder;

        public StorageService(string folder)
        {
            _folder = folder;
            Directory.CreateDirectory(folder);
        }

        public List<Claim>? Load()
        {
            var file = Path.Combine(_folder, "claims.json");
            if (!File.Exists(file)) return new List<Claim>();
            var json = File.ReadAllText(file);
            return JsonSerializer.Deserialize<List<Claim>>(json);
        }

        public void Save(IEnumerable<Claim> claims)
        {
            var file = Path.Combine(_folder, "claims.json");
            var json = JsonSerializer.Serialize(claims);
            File.WriteAllText(file, json);
        }
    }
}

