using System.Threading.Tasks;

namespace AIAssistant.Core.Interfaces
{
    public interface IPlugin
    {
        string Name { get; }
        Task<string> Process(string input);
    }
}
