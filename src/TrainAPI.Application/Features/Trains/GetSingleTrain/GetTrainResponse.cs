namespace TrainAPI.Application.Features.Trains.GetSingleTrain;

public class GetTrainResponse
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Code { get; init; }
}