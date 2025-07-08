using System.Globalization;
using System.Text.RegularExpressions;
using TuDus.Models;

namespace TuDus.Parser
{
    public static class TuDuParser
    {
        private static readonly List<string> checkboxSigns = new List<string>()
        {
            "[ ]",
            "[]",
        };

        private static readonly string checkedCheckboxSign = "[x]";

        private static readonly string tagSign = "#";

        private static readonly string dateSign = "@";

        private static readonly string dateFormat = "dd.MM.yyyy";

        private static readonly string hourSign = "&";

        public static TuDuItem Parse(string data)
        {
            string description = data;
            bool? checkbox = CheckCheckbox(description, out description);
            DateTime? date = CheckDate(description, out description);
            var tag = CheckTag(description, out description);

            TuDuItem item = new TuDuItem
            {
                RawData = data,
                Description = description,
                IsSelected = checkbox is null ? false : (bool)checkbox,
                IsCheckboxActive = checkbox is null ? true : false,
                Tag = tag,
                Date = date.HasValue ? date.Value.ToShortDateString() : "",
            };
            return item;
        }

        public static string Serialize(TuDuItem item)
        {
            string rawData = string.Empty;
            rawData = item.IsSelected ? checkedCheckboxSign : checkboxSigns[0];

            string temp;
            CheckCheckbox(item.RawData, out temp);
            rawData += temp;

            return rawData;
        }

        private static bool? CheckCheckbox(string? input, out string output)
        {
            if (string.IsNullOrEmpty(input))
            {
                output = "";
                return null;
            }

            foreach (string sign in checkboxSigns)
            {
                if (input.StartsWith(sign))
                {
                    output = input.Substring(sign.Length);
                    return false;
                }
            }

            if (input.StartsWith(checkedCheckboxSign))
            {
                output = input.Substring(checkedCheckboxSign.Length);
                return true;
            }

            output = input;
            return null;
        }

        private static DateTime? CheckDate(string input, out string output)
        {
            if (string.IsNullOrEmpty(input))
            {
                output = string.Empty;
                return null;
            }

            string pattern = @"@\d{2}\.\d{2}\.\d{4}";
            Match match = Regex.Match(input, pattern);

            if (match.Success)
            {
                string dateString = match.Value.Substring(1);
                DateTime parsedDate;

                if (DateTime.TryParseExact(dateString, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                {
                    output = input.Replace(match.Value, "").Trim(); ;
                    return parsedDate;
                }
                else
                {
                    output = input;
                    return null;
                }
            }
            else
            {
                output = input;
                return null;
            }
        }

        private static string CheckTag(string input, out string output)
        {
            if (string.IsNullOrEmpty(input))
            {
                output = string.Empty;
                return string.Empty;
            }

            string pattern = @"#(\S+)";
            Match match = Regex.Match(input, pattern);

            if (match.Success)
            {
                string extractedHash = match.Groups[1].Value;
                output = input.Replace(match.Value, "").Trim();

                return extractedHash;
            }
            else
            {
                output = input;
                return string.Empty;
            }
        }
    }
}
