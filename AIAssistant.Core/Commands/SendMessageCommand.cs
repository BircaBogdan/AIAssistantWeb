using AIAssistant.Core.Facades;

namespace AIAssistant.Core.Commands
{
    public class SendMessageCommand : ICommand
    {
        private readonly ChatFacade _facade;
        private readonly string _message;
        private readonly double _temperature;
        private readonly Func<string, Task> _onToken;

        public SendMessageCommand(
            ChatFacade facade,
            string message,
            double temperature,
            Func<string, Task> onToken)
        {
            _facade = facade;
            _message = message;
            _temperature = temperature;
            _onToken = onToken;
        }

        public async Task ExecuteAsync()
        {
            await foreach (var token in _facade.SendMessageStream(_message, _temperature))
            {
                await _onToken(token);
            }
        }
    }
}