using AIAssistant.Core.Models;

namespace AIAssistant.Core.Exporters
{
    public abstract class ChatExporterTemplate
    {
        public byte[] Export(IEnumerable<Message> messages)
        {
            var content = FormatContent(messages);

            return GenerateFile(content);
        }

        protected abstract string FormatContent(IEnumerable<Message> messages);

        protected virtual byte[] GenerateFile(string content)
        {
            return System.Text.Encoding.UTF8.GetBytes(content);
        }
    }
}