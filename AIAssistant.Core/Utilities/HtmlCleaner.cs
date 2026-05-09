using System.Text.RegularExpressions;

namespace AIAssistant.Core.Utilities
{
    public static class HtmlCleaner
    {
        public static string Clean(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return string.Empty;

            string text = html;

            // BR
            text = text.Replace("<br>", "\n");
            text = text.Replace("<br/>", "\n");
            text = text.Replace("<br />", "\n");

            // BOLD
            text = text.Replace("<b>", "");
            text = text.Replace("</b>", "");

            // HTML SPACE
            text = text.Replace("&nbsp;", " ");

            // REMOVE HTML TAGS
            text = Regex.Replace(text, "<.*?>", string.Empty);

            // REMOVE "Mesaje rămase"
            text = Regex.Replace(
                text,
                @"\(Mesaje rămase:\s*\d+\)",
                "",
                RegexOptions.IgnoreCase);

            // REMOVE DECORATOR TIMESTAMP
            text = Regex.Replace(
                text,
                @"^\[\d{2}:\d{2}:\d{2}\]\s*",
                "",
                RegexOptions.Multiline);

            // REMOVE EMPTY LINES
            text = Regex.Replace(
                text,
                @"^\s*$\n|\r",
                "",
                RegexOptions.Multiline);

            return text.Trim();
        }
    }
}