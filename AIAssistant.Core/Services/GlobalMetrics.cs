using System;

namespace AIAssistant.Core.Services
{
    public sealed class GlobalMetrics
    {
        private static GlobalMetrics _instance;
        private static readonly object _lock = new object();

        public int TotalMessagesProcessed { get; private set; }

        // 🔥 ALIAS pentru UI (fără să stricăm codul existent)
        public int TotalMessages => TotalMessagesProcessed;

        private GlobalMetrics()
        {
        }

        public static GlobalMetrics Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new GlobalMetrics();
                    }

                    return _instance;
                }
            }
        }

        public void IncrementMessages()
        {
            lock (_lock)
            {
                TotalMessagesProcessed++;
            }
        }
    }
}