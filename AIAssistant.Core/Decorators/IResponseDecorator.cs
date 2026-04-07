namespace AIAssistant.Core.Decorators
{
    public interface IResponseDecorator
    {
        string Process(string response);
    }
}