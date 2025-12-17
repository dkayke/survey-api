namespace SurveyApiSystem.Domain.Entities;

public class Vote
{
    public Guid Id { get; private set; }
    public Guid SurveyId { get; private set; }
    public Guid QuestionId { get; private set; }
    public Guid OptionId { get; private set; }

    public Vote(Guid surveyId, Guid questionId, Guid optionId)
    {
        Id = Guid.NewGuid();
        SurveyId = surveyId;
        QuestionId = questionId;
        OptionId = optionId;
    }
}