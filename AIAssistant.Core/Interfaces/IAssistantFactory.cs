namespace AIAssistant.Core.Interfaces
{
    public interface IAssistantFactory
    {
        IPlugin CreatePlugin();
        IResponseStrategy CreateResponseStrategy();
    }
}
