namespace SurveyApiSystem.Domain.Entities;

public class Question
{
    public Guid Id { get; private set; }
    public string Text { get; private set; }
    public Guid SurveyId { get; private set; }
    
    private readonly List<Option> _options = new();
    public IReadOnlyCollection<Option> Options => _options.AsReadOnly();

    public Question(string text)
    {
        Id = Guid.NewGuid();
        Text = text;
    }

    public void AddOption(string text) => _options.Add(new Option(text));
}