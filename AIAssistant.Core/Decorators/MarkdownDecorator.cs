using System.Text.RegularExpressions;

namespace AIAssistant.Core.Decorators
{
    public class MarkdownDecorator : BaseResponseDecorator
    {
        public MarkdownDecorator(IResponseDecorator inner) : base(inner) { }

        public override string Process(string response)
        {
            var processed = base.Process(response);

            // 🔥 FIX: normalizează output AI (uneori e "``` c code")
            processed = processed.Replace("```c ", "```c\n");
            processed = processed.Replace("``` ", "```\n");

            // 🔥 CODE BLOCK (corect + tolerant)
            processed = Regex.Replace(
                processed,
                @"```[a-zA-Z]*\s*(.*?)```",
                match =>
                {
                    var code = match.Groups[1].Value.Trim();
                    return $"<pre><code>{code}</code></pre>";
                },
                RegexOptions.Singleline
            );

            // 🔥 FALLBACK: detectare cod dacă AI nu dă markdown
            if (!processed.Contains("<pre><code>") &&
                (processed.Contains("#include") || processed.Contains("int main")))
            {
                processed = $"<pre><code>{processed}</code></pre>";
            }

            // inline code
            processed = Regex.Replace(processed, @"`([^`]+)`", "<code>$1</code>");

            // bold
            processed = Regex.Replace(processed, @"\*\*(.*?)\*\*", "<b>$1</b>");

            // italic
            processed = Regex.Replace(processed, @"\*(.*?)\*", "<i>$1</i>");

            // 🔥 NEWLINE → <br> (dar NU în <pre>)
            processed = Regex.Replace(
                processed,
                @"(?![^<]*</pre>)\n",
                "<br>"
            );

            return processed;
        }
    }
}