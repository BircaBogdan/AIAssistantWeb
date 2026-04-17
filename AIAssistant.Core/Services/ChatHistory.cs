using System.Collections.Generic;
using AIAssistant.Core.Models;

namespace AIAssistant.Core.Services
{
    public class ChatHistory
    {
        private readonly List<Message> _messages = new();

        public void Add(Message message)
        {
            _messages.Add(message);
        }

        public IEnumerable<Message> GetAll()
        {
            return _messages;
        }

        // NECESAR pentru Command
        public void Clear()
        {
            _messages.Clear();
        }
    }
}