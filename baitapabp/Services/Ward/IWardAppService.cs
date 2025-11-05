using baitapabp.Services.Dtos;
using Volo.Abp.Application.Services;

namespace baitapabp.Services.Ward
{
    public interface IWardAppService : ICrudAppService<WardsDto, int, WardsPagedRequestDto, CreateUpdateWardsDto>
    {
        Task<List<WardsDto>> GetByProvinceCodeAsync(string provinceCode);
        Task<bool> CheckCodeAsync(string code);
    }
}
