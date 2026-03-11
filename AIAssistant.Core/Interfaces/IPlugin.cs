using System.Collections.Generic;

namespace AIAssistant.Core.Interfaces
{
    public interface IPlugin
    {
        string Name { get; }

        IAsyncEnumerable<string> ProcessStream(string input, double temperature);
    }
}