namespace AIAssistant.Core.PromptComposite;
public class SimpleModule : IPromptComponent
{
    private string _text;

    public SimpleModule(string text)
    {
        _text = text;
    }

    public string GetPromptText() => _text;
}