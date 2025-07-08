using System.Globalization;
using System.IO;
using System.Text.Json;

namespace TuDus.Helpers
{
    public class LocalizationManager
    {
        private static Dictionary<string, Dictionary<string, string>>? _all = new();
        private static Dictionary<string, string> _current = new();
        public static void Initialize(string jsonPath, CultureInfo culture)
        {
            var json = File.ReadAllText(jsonPath);
            _all = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json);

            if (_all == null)
            {
                return;
            }

            var lang = culture.Name;
            _current = _all.ContainsKey(lang) ? _all[lang] : _all["en-US"];
        }
        public static string Get(string key) => _current.TryGetValue(key, out var v) ? v : key;
    }
}
