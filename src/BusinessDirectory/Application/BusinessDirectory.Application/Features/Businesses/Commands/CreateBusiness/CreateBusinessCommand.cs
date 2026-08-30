using MediatR;

namespace BusinessDirectory.Application.Features.Businesses.Commands.CreateBusiness
{
    public record CreateBusinessCommand(
        Guid OwnerId,
        string Title,
        string Description,
        int ProvinceId,
        int CityId,
        string StreetLine,
        string PostalCode
    ) : IRequest<Guid>;
}
