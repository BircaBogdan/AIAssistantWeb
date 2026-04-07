using System;

namespace AIAssistant.Core.Decorators
{
    public class TimestampDecorator : BaseResponseDecorator
    {
        public TimestampDecorator(IResponseDecorator inner) : base(inner) { }

        public override string Process(string response)
        {
            var processed = base.Process(response);
            return $"[{DateTime.Now:HH:mm:ss}] {processed}";
        }
    }
}