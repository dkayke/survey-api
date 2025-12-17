namespace SurveyApiSystem.Domain.Entities;

public class Survey
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    
    private readonly List<Question> _questions = new();
    public IReadOnlyCollection<Question> Questions => _questions.AsReadOnly();

    public Survey(string title, string description)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        IsActive = true;
    }

    public void AddQuestion(string text, List<string> optionTexts)
    {
        var question = new Question(text);
        foreach (var opt in optionTexts) question.AddOption(opt);
        _questions.Add(question);
    }
}