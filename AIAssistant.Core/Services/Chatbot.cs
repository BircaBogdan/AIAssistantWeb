using System.Collections.Generic;
using AIAssistant.Core.Interfaces;
using AIAssistant.Core.States;

namespace AIAssistant.Core.Services
{
    public class Chatbot
    {
        private readonly IEnumerable<IPlugin> _plugins;

        public IChatState CurrentState { get; private set; }

        public Chatbot(IEnumerable<IPlugin> plugins)
        {
            _plugins = plugins;
            CurrentState = new ReadyState();
        }

        public void SetState(IChatState state)
        {
            CurrentState = state;
        }

        public async IAsyncEnumerable<string> HandleMessageStream(string input, double temperature)
        {
            SetState(new WaitingForAiState());

            foreach (var plugin in _plugins)
            {
                await foreach (var token in plugin.ProcessStream(input, temperature))
                {
                    yield return token;
                }

                break;
            }

            SetState(new ReadyState());
        }

        public void SetErrorState()
        {
            SetState(new ErrorState());
        }

        public void SetRateLimitState()
        {
            SetState(new RateLimitState());
        }
    }
}