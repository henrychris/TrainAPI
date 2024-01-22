using TrainAPI.Domain.Entities;

namespace TrainAPI.Application.Contracts;

public interface ITrainService
{
    Task CreateTrain(Train train);
    Task<Train?> GetTrain(string trainId);
    Task UpdateTrain(Train train);
    Task DeleteTrain(Train train);
    IQueryable<Train> GetQueryable();
}