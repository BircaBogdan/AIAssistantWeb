using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIAssistant.Core.Services
{
    public sealed class GlobalMetrics
    {
        private static GlobalMetrics _instance;
        private static readonly object _lock = new object();

        public int TotalMessagesProcessed { get; private set; }

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