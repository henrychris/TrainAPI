using TrainAPI.Application.Contracts;
using TrainAPI.Domain.Entities;
using TrainAPI.Infrastructure.Data;

namespace TrainAPI.Infrastructure.Services;

public class TrainService(DataContext context) : ITrainService
{
    public async Task CreateTrain(Train train)
    {
        await context.Trains.AddAsync(train);
        await context.SaveChangesAsync();
    }

    public async Task<Train?> GetTrain(string trainId)
    {
        return await context.Trains.FindAsync(trainId);
    }

    public async Task UpdateTrain(Train train)
    {
        context.Trains.Update(train);
        await context.SaveChangesAsync();
    }

    public async Task DeleteTrain(Train train)
    {
        context.Remove(train);
        await context.SaveChangesAsync();
    }

    public IQueryable<Train> GetQueryable()
    {
        return context.Trains.AsQueryable();
    }
}