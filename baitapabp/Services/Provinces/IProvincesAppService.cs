using baitapabp.Services.Dtos;
using Volo.Abp.Application.Services;

namespace baitapabp.Services.Provinces
{
    public interface IProvincesAppService : ICrudAppService<
        ProvincesDto,
        int,
        ProvincesPagedRequestDto,
        CreateUpdateProvincesDto>
    {
        Task<bool> CheckCodeAsync(string code, int? id = null);

    }
}
