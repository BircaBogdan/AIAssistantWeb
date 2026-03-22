namespace AIAssistant.Core.PromptComposite;

public class CompositePrompt : IPromptComponent
{
    private List<IPromptComponent> _children = new();

    public void Add(IPromptComponent component)
    {
        _children.Add(component);
    }

    public void Remove(IPromptComponent component)
    {
        _children.Remove(component);
    }

    public string GetPromptText()
    {
        return string.Join("\n- ", _children.Select(c => c.GetPromptText()));
    }
}