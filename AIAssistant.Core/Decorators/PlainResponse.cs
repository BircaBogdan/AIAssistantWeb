namespace AIAssistant.Core.Decorators
{
    public class PlainResponse : IResponseDecorator
    {
        public string Process(string response)
        {
            return response;
        }
    }
}