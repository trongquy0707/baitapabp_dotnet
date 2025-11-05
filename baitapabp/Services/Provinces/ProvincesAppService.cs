using baitapabp.Entities;
using baitapabp.Services.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp;

namespace baitapabp.Services.Provinces
{
    [RemoteService(Name = "provinces")]
    //[Authorize]
    public class ProvincesAppService : CrudAppService<ProvincesEntity, ProvincesDto, int, ProvincesPagedRequestDto, CreateUpdateProvincesDto>, IProvincesAppService
    {
        private readonly IRepository<ProvincesEntity, int> _repositoryProvinces;
        private readonly IRepository<WardsEntity, int> _repositoryWards;
        public ProvincesAppService(IRepository<ProvincesEntity, int> repository, IRepository<WardsEntity, int> wardsRepository)
            : base(repository)
        {
            _repositoryProvinces = repository;
            _repositoryWards = wardsRepository;
        }
        [RemoteService(false)]
        public async Task<bool> CheckCodeAsync(string code, int? id = null)
        {
            var existing = await _repositoryProvinces.FirstOrDefaultAsync(x => x.Code == code);
            if (existing == null)
                return false;
            if (id.HasValue && existing.Id == id.Value)
                return false;

            return true;
        }
        //[Authorize(Roles = "employee")]
        protected override async Task<IQueryable<ProvincesEntity>> CreateFilteredQueryAsync(ProvincesPagedRequestDto input)
        {
            var query = await base.CreateFilteredQueryAsync(input);
            if (!string.IsNullOrWhiteSpace(input.SearchKey))
            {
                query = query.WhereIf(!string.IsNullOrWhiteSpace(input.SearchKey),
                    x => x.Name.Contains(input.SearchKey) || x.Code.Contains(input.SearchKey));
            }
            return query;
        }
        [RemoteService(false)]
        public override Task<ProvincesDto> CreateAsync(CreateUpdateProvincesDto input)
         => throw new NotImplementedException();

        [RemoteService(false)]
        public override Task<ProvincesDto> UpdateAsync(int id, CreateUpdateProvincesDto input)
            => throw new NotImplementedException();

        [HttpPost]
        [Route("provinces/create-or-update")]
        public async Task<ProvincesDto> CreateOrUpdateAsync(CreateUpdateProvincesDto input)
        {
            var exists = await CheckCodeAsync(input.Code, input.Id);
            if (exists)
            {
                throw new UserFriendlyException($"Mã Tỉnh/Thành phố '{input.Code}' đã được dùng ở bản ghi khác!");
            }
            if (input.Id == 0)
            {
                return await base.CreateAsync(input);
            }
            else
            {
                return await base.UpdateAsync(input.Id, input);
            }
        }
        public override async Task DeleteAsync(int id)
        {

            var entity = await _repositoryProvinces.GetAsync(id);
            if (entity == null)
            {
                throw new UserFriendlyException($"Không tìm thấy dữ liệu với Id = {id}");
            }
            var exitesWard = await _repositoryWards.GetListAsync(x => x.provinceCode == entity.Code);
            if (exitesWard.Count > 0)
            {
                throw new UserFriendlyException("Không thể xóa Tỉnh/Thành phố này vì có phường/xã thuộc Tỉnh/Thành phố này");
            }
            await base.DeleteAsync(id);
        }


    }
}
