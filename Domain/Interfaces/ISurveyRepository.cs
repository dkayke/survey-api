using SurveyApiSystem.Domain.Entities;

namespace SurveyApiSystem.Domain.Interfaces;

public interface ISurveyRepository
{
    Task<Survey?> GetByIdAsync(Guid id);
    Task AddAsync(Survey survey);
    Task SaveChangesAsync();
}