using SurveyApiSystem.Domain.Entities;
using SurveyApiSystem.Application.DTOs;

namespace SurveyApiSystem.Domain.Interfaces;

public interface IVoteRepository
{
    Task AddAsync(Vote vote);
    Task<List<VoteResultDto>> GetResultsAsync(Guid surveyId);
}