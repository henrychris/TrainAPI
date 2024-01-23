using MediatR;

using ErrorOr;

using TrainAPI.Application.ApiResponses;
using TrainAPI.Application.Contracts;
using TrainAPI.Application.Features.Coaches.GetSingleCoach;
using TrainAPI.Domain.Entities;
using TrainAPI.Domain.Enums;

namespace TrainAPI.Application.Features.Coaches.GetAllCoaches;

public class GetAllCoachesRequest : QueryStringParameters, IRequest<ErrorOr<PagedResponse<GetCoachResponse>>>
{
    public string? Name { get; set; }
    public string? Class { get; set; }
    public string? TrainId { get; set; }
}

public class GetAllCoachesRequestHandler(ICoachService trainService, ILogger<GetAllCoachesRequestHandler> logger)
    : IRequestHandler<GetAllCoachesRequest, ErrorOr<PagedResponse<GetCoachResponse>>>
{
    public async Task<ErrorOr<PagedResponse<GetCoachResponse>>> Handle(GetAllCoachesRequest request,
                                                                       CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching trains... \nRequest: {request}", request);
        IQueryable<Coach> query = trainService.GetQueryable();
        query = ApplyFilters(query, request);

        var pagedResults = query.Select(x => CoachMapper.ToGetCoachResponse(x));
        var response =
            await new PagedResponse<GetCoachResponse>().ToPagedList(pagedResults, request.PageNumber,
                request.PageSize);

        logger.LogInformation("Fetched Coaches successfully. Returned {totalCount} trains.", response.TotalCount);
        return response;
    }

    private static IQueryable<Coach> ApplyFilters(IQueryable<Coach> query, GetAllCoachesRequest request)
    {
        if (!string.IsNullOrEmpty(request.Name))
        {
            query = query.Where(x => x.Name.Contains(request.Name, StringComparison.CurrentCultureIgnoreCase));
        }

        if (!string.IsNullOrEmpty(request.TrainId))
        {
            query = query.Where(x => x.TrainId == request.TrainId);
        }

        if (!string.IsNullOrEmpty(request.Class))
        {
            _ = Enum.TryParse<CoachClass>(request.Class, out var enumVal);
            query = query.Where(x => x.Class.Equals(enumVal.ToString(), StringComparison.CurrentCultureIgnoreCase));
        }

        return query;
    }
}