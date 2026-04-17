namespace AIAssistant.Core.Commands
{
    public interface ICommand
    {
        Task ExecuteAsync();
    }
}