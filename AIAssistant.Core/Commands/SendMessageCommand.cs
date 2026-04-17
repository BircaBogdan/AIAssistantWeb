using AIAssistant.Core.Facades;

namespace AIAssistant.Core.Commands
{
    public class SendMessageCommand : ICommand
    {
        private readonly ChatFacade _facade;
        private readonly string _message;
        private readonly double _temperature;
        private readonly Func<string, Task> _onToken;
        private readonly bool _isRegenerate;

        public SendMessageCommand(
            ChatFacade facade,
            string message,
            double temperature,
            bool isRegenerate,
            Func<string, Task> onToken)
        {
            _facade = facade;
            _message = message;
            _temperature = temperature;
            _onToken = onToken;
            _isRegenerate = isRegenerate;
        }

        public async Task ExecuteAsync()
        {
            await foreach (var token in _facade.SendMessageStream(_message, _temperature, _isRegenerate))
            {
                await _onToken(token);
            }
        }
    }
}