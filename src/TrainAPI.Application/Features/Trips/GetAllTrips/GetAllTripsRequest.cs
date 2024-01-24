using ErrorOr;

using MediatR;

using TrainAPI.Application.ApiResponses;
using TrainAPI.Application.Contracts;
using TrainAPI.Application.Features.Trips.GetSingleTrip;
using TrainAPI.Domain.Entities;

namespace TrainAPI.Application.Features.Trips.GetAllTrips;

public class GetAllTripsRequest : QueryStringParameters, IRequest<ErrorOr<PagedResponse<GetTripResponse>>>
{
    public string? Name { get; set; }
    public DateTime? DateOfTrip { get; set; }
    public string ToStationId { get; set; } = string.Empty;
    public string FromStationId { get; set; } = string.Empty;
}

public class GetAllTripsRequestHandler(ITripService tripService, ILogger<GetAllTripsRequestHandler> logger)
    : IRequestHandler<GetAllTripsRequest, ErrorOr<PagedResponse<GetTripResponse>>>
{
    public async Task<ErrorOr<PagedResponse<GetTripResponse>>> Handle(GetAllTripsRequest request,
                                                                      CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching trips... \nRequest: {request}", request);
        IQueryable<Trip> query = tripService.GetQueryable();
        query = ApplyFilters(query, request);

        var pagedResults = query.Select(x => TripMapper.ToGetTripResponse(x));
        var response =
            await new PagedResponse<GetTripResponse>().ToPagedList(pagedResults, request.PageNumber,
                request.PageSize);

        logger.LogInformation("Fetched Trips successfully. Returned {totalCount} trips.", response.TotalCount);
        return response;
    }

    private static IQueryable<Trip> ApplyFilters(IQueryable<Trip> query, GetAllTripsRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            query = query.Where(x => x.Name.Contains(request.Name, StringComparison.CurrentCultureIgnoreCase));
        }

        if (request.DateOfTrip is not null)
        {
            // get the trips that are on the same day
            query = query.Where(x => request.DateOfTrip.Value.Day == x.DateOfTrip.Day);
        }

        query = query.Where(x => x.FromStationId == request.FromStationId && x.ToStationId == request.ToStationId);
        return query;
    }
}