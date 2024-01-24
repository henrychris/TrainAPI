using TrainAPI.Application.Features.Trains.CreateTrain;
using TrainAPI.Application.Features.Trains.GetSingleTrain;
using TrainAPI.Domain.Entities;

namespace TrainAPI.Application.Features.Trains;

public static class TrainMapper
{
    public static Train CreateTrain(CreateTrainRequest request)
    {
        return new Train { Name = request.Name, Code = request.Code };
    }

    public static CreateTrainResponse ToCreateTrainResponse(Train train)
    {
        return new CreateTrainResponse { TrainId = train.Id };
    }

    public static GetTrainResponse ToGetTrainResponse(Train train)
    {
        return new GetTrainResponse { Id = train.Id, Code = train.Code, Name = train.Name };
    }
}