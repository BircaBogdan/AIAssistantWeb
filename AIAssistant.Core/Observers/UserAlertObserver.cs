namespace AIAssistant.Core.Observers
{
    public class UserAlertObserver : IObserver
    {
        public string? AlertMessage { get; private set; }

        public void Update(int messageCount)
        {
            int remaining = 20 - messageCount;

            if (remaining == 15 || remaining == 10 || remaining == 5)
            {
                AlertMessage = $"Atenție: Mai ai doar {remaining} mesaje disponibile astăzi!";
            }
            else
            {
                AlertMessage = null;
            }
        }
    }
}