namespace AIAssistant.Core.Decorators
{
    public abstract class BaseResponseDecorator : IResponseDecorator
    {
        protected readonly IResponseDecorator _inner;

        protected BaseResponseDecorator(IResponseDecorator inner)
        {
            _inner = inner;
        }

        public virtual string Process(string response)
        {
            return _inner?.Process(response) ?? response;
        }
    }
}