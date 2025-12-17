using Microsoft.EntityFrameworkCore;
using SurveyApiSystem.Domain.Entities;
using SurveyApiSystem.Domain.Interfaces;
using SurveyApiSystem.Infrastructure.Persistence;

namespace SurveyApiSystem.Infrastructure.Repositories;

public class SurveyRepository(ApplicationDbContext context) : ISurveyRepository
{
    public async Task<Survey?> GetByIdAsync(Guid id)
    {
        return await context.Surveys
            .Include(s => s.Questions)
            .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task AddAsync(Survey survey) => await context.Surveys.AddAsync(survey);
    public async Task SaveChangesAsync() => await context.SaveChangesAsync();
}