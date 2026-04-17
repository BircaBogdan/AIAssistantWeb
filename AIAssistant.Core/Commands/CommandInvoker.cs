namespace AIAssistant.Core.Commands
{
    public class CommandInvoker
    {
        private ICommand? _lastCommand;

        public async Task ExecuteAsync(ICommand command)
        {
            _lastCommand = command;
            await command.ExecuteAsync();
        }

        public async Task RegenerateAsync()
        {
            if (_lastCommand != null)
            {
                await _lastCommand.ExecuteAsync();
            }
        }
    }
}