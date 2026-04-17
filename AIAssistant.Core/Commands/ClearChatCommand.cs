using AIAssistant.Core.Services;

namespace AIAssistant.Core.Commands
{
    public class ClearChatCommand : ICommand
    {
        private readonly ChatHistory _history;

        public ClearChatCommand(ChatHistory history)
        {
            _history = history;
        }

        public Task ExecuteAsync()
        {
            _history.Clear();
            return Task.CompletedTask;
        }
    }
}