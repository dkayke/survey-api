using Microsoft.EntityFrameworkCore;
using SurveyApiSystem.Domain.Entities;
using SurveyApiSystem.Domain.Interfaces;
using SurveyApiSystem.Infrastructure.Persistence;
using SurveyApiSystem.Application.DTOs;

namespace SurveyApiSystem.Infrastructure.Repositories;

public class VoteRepository(ApplicationDbContext context) : IVoteRepository
{
    public async Task AddAsync(Vote vote)
    {
        await context.Votes.AddAsync(vote);
        await context.SaveChangesAsync();
    }

    public async Task<List<VoteResultDto>> GetResultsAsync(Guid surveyId)
    {
        return await context.Votes
            .Where(v => v.SurveyId == surveyId)
            .GroupBy(v => new { v.QuestionId, v.OptionId })
            .Select(g => new VoteResultDto(g.Key.QuestionId, g.Key.OptionId, g.Count()))
            .ToListAsync();
    }
}