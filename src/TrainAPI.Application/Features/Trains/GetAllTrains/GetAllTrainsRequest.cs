using MediatR;

using ErrorOr;

using TrainAPI.Application.ApiResponses;
using TrainAPI.Application.Contracts;
using TrainAPI.Application.Features.Trains.GetSingleTrain;
using TrainAPI.Domain.Entities;

namespace TrainAPI.Application.Features.Trains.GetAllTrains;

public class GetAllTrainsRequest : QueryStringParameters, IRequest<ErrorOr<PagedResponse<GetTrainResponse>>>
{
    public string? Name { get; set; }
    public string? Code { get; set; }
}

public class GetAllTrainsRequestHandler(ITrainService trainService, ILogger<GetAllTrainsRequestHandler> logger)
    : IRequestHandler<GetAllTrainsRequest, ErrorOr<PagedResponse<GetTrainResponse>>>
{
    public async Task<ErrorOr<PagedResponse<GetTrainResponse>>> Handle(GetAllTrainsRequest request,
                                                                       CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching trains... \nRequest: {request}", request);
        IQueryable<Train> query = trainService.GetQueryable();
        query = ApplyFilters(query, request);

        var pagedResults = query.Select(x => TrainMapper.ToGetTrainResponse(x));
        var response =
            await new PagedResponse<GetTrainResponse>().ToPagedList(pagedResults, request.PageNumber,
                request.PageSize);

        logger.LogInformation("Fetched Trains successfully. Returned {totalCount} trains.", response.TotalCount);
        return response;
    }

    private static IQueryable<Train> ApplyFilters(IQueryable<Train> query, GetAllTrainsRequest request)
    {
        if (!string.IsNullOrEmpty(request.Name))
        {
            query = query.Where(x => x.Name.Contains(request.Name, StringComparison.CurrentCultureIgnoreCase));
        }

        if (!string.IsNullOrEmpty(request.Code))
        {
            query = query.Where(x => x.Code.Contains(request.Code, StringComparison.CurrentCultureIgnoreCase));
        }

        return query;
    }
}