using TrainAPI.Domain.Entities;

namespace TrainAPI.Application.Contracts;

public interface ICoachService
{
    Task CreateCoach(Coach coach);
    Task<Coach?> GetCoach(string coachId);
    Task UpdateCoach(Coach coach);
    Task DeleteCoach(Coach coach);
    IQueryable<Coach> GetQueryable();
}