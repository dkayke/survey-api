using SurveyApiSystem.Application.DTOs;
using SurveyApiSystem.Domain.Entities;
using SurveyApiSystem.Domain.Interfaces;

namespace SurveyApiSystem.Application.Services;

public class SurveyService(ISurveyRepository surveyRepo, IVoteRepository voteRepo)
{
    public async Task<Guid> CreateSurveyAsync(CreateSurveyRequest request)
    {
        var survey = new Survey(request.Title, request.Description);
        foreach (var q in request.Questions)
        {
            survey.AddQuestion(q.Text, q.Options);
        }
        await surveyRepo.AddAsync(survey);
        await surveyRepo.SaveChangesAsync();
        return survey.Id;
    }

    public async Task<bool> VoteAsync(Guid surveyId, Guid questionId, Guid optionId)
    {
        var survey = await surveyRepo.GetByIdAsync(surveyId);
        
        if (survey == null) return false;

        var vote = new Vote(surveyId, questionId, optionId);
        await voteRepo.AddAsync(vote);
        return true;
    }

    public async Task<SurveyReportDto?> GetSurveyReportAsync(Guid surveyId)
    {
        var survey = await surveyRepo.GetByIdAsync(surveyId);
        
        if (survey == null) return null;

        var results = await voteRepo.GetResultsAsync(surveyId);

        var questionReports = survey.Questions.Select(q => new QuestionReportDto(
            q.Id,
            q.Text,
            q.Options.Select(o => new OptionReportDto(
                o.Id,
                o.Text,
                results.FirstOrDefault(r => r.OptionId == o.Id)?.Count ?? 0
            )).ToList()
        )).ToList();

        return new SurveyReportDto(survey.Id, survey.Title, questionReports);
    }
}