using System.Text.RegularExpressions;

namespace AIAssistant.Core.Decorators
{
    public class MarkdownDecorator : BaseResponseDecorator
    {
        public MarkdownDecorator(IResponseDecorator inner) : base(inner) { }

        public override string Process(string response)
        {
            var processed = base.Process(response);

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

            if (!processed.Contains("<pre><code>") &&
                (processed.Contains("#include") || processed.Contains("int main")))
            {
                processed = $"<pre><code>{processed}</code></pre>";
                return processed;
            }

            // inline code
            processed = Regex.Replace(processed, @"`([^`]+)`", "<code>$1</code>");

            // bold
            processed = Regex.Replace(processed, @"\*\*(.*?)\*\*", "<b>$1</b>");

            // italic
            processed = Regex.Replace(processed, @"\*(.*?)\*", "<i>$1</i>");

            return processed;
        }
    }
}