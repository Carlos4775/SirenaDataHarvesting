using System.Text.RegularExpressions;

namespace SirenaDataHarvesting.Utils
{
    public static class ImageUrlUtility
    {
        public static string? ExtractImageUrlFromStyleAttribute(string styleAttribute)
        {
            Match match = Regex.Match(styleAttribute, @"url\(""([^""]+)""\)");

            return match.Success ? match.Groups[1].Value : null;
        }
    }
}
