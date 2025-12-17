namespace SurveyApiSystem.Domain.Entities;

public class Option
{
    public Guid Id { get; private set; }
    public string Text { get; private set; }
    public Guid QuestionId { get; private set; }

    public Option(string text)
    {
        Id = Guid.NewGuid();
        Text = text;
    }
}