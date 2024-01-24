namespace TrainAPI.Application.Features.Stations.GetStation;

public class GetStationResponse
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Code { get; init; }
}