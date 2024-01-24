using TrainAPI.Application.Contracts;
using TrainAPI.Domain.Entities;
using TrainAPI.Infrastructure.Data;

namespace TrainAPI.Infrastructure.Services;

public class CoachService(DataContext context) : ICoachService
{
    public async Task CreateCoach(Coach coach)
    {
        await context.Coaches.AddAsync(coach);
        await context.SaveChangesAsync();
    }

    public async Task<Coach?> GetCoach(string coachId)
    {
        return await context.Coaches.FindAsync(coachId);
    }

    public async Task UpdateCoach(Coach coach)
    {
        context.Coaches.Update(coach);
        await context.SaveChangesAsync();
    }

    public async Task DeleteCoach(Coach coach)
    {
        context.Remove(coach);
        await context.SaveChangesAsync();
    }

    public IQueryable<Coach> GetQueryable()
    {
        return context.Coaches.AsQueryable();
    }
}