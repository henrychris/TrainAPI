using ErrorOr;

using MediatR;

using TrainAPI.Application.ApiResponses;
using TrainAPI.Application.Contracts;
using TrainAPI.Application.Features.Stations.GetStation;
using TrainAPI.Domain.Entities;

namespace TrainAPI.Application.Features.Stations.GetAllStations;

public class GetAllStationsRequest : QueryStringParameters, IRequest<ErrorOr<PagedResponse<GetStationResponse>>>
{
    public string? Name { get; set; }
    public string? Code { get; set; }
}

public class GetAllStationsRequestHandler(IStationService stationService, ILogger<GetAllStationsRequestHandler> logger)
    : IRequestHandler<GetAllStationsRequest, ErrorOr<PagedResponse<GetStationResponse>>>
{
    public async Task<ErrorOr<PagedResponse<GetStationResponse>>> Handle(GetAllStationsRequest request,
                                                                         CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching stations... \nRequest: {request}", request);
        IQueryable<Station> query = stationService.GetQueryable();
        query = ApplyFilters(query, request);

        var pagedResults = query.Select(x => StationMapper.ToGetStationResponse(x));
        var response =
            await new PagedResponse<GetStationResponse>().ToPagedList(pagedResults, request.PageNumber,
                request.PageSize);

        logger.LogInformation("Fetched Stations successfully. Returned {totalCount} stations.", response.TotalCount);
        return response;
    }

    private static IQueryable<Station> ApplyFilters(IQueryable<Station> query, GetAllStationsRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            query = query.Where(x => x.Name.Contains(request.Name, StringComparison.CurrentCultureIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(request.Code))
        {
            query = query.Where(x => x.Code.Contains(request.Code, StringComparison.CurrentCultureIgnoreCase));
        }

        return query;
    }
}