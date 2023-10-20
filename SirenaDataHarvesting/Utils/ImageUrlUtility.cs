using System.Text.RegularExpressions;

namespace SirenaDataHarvesting.Utils
{
    public static class ImageUrlUtility
    {
        public static string? ExtractImageUrlFromStyleAttribute(string styleAttribute)
        {
            string pattern = @"url\(""([^""]+)""\)";

            Match match = Regex.Match(styleAttribute, pattern);

            return match.Success ? match.Groups[1].Value : null;
        }
    }
}
