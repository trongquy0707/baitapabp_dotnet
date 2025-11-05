using AutoMapper.Internal.Mappers;
using baitapabp.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp;
using baitapabp.Services.Dtos;

namespace baitapabp.Services.Ward
{
    public class WardAppService : CrudAppService<
            WardsEntity,
            WardsDto,
            int,
            WardsPagedRequestDto,
            CreateUpdateWardsDto>,
            IWardAppService
    {
        private readonly IRepository<WardsEntity, int> _wardsRepository;
        private readonly IRepository<ProvincesEntity, int> _provinceRepository;

        public WardAppService(
            IRepository<WardsEntity, int> wardsRepository,
            IRepository<ProvincesEntity, int> provinceRepository)
            : base(wardsRepository)
        {
            _wardsRepository = wardsRepository;
            _provinceRepository = provinceRepository;
        }

        public async Task<List<WardsDto>> GetByProvinceCodeAsync(string provinceCode)
        {
            var wards = await _wardsRepository.GetListAsync(w => w.provinceCode == provinceCode);
            return ObjectMapper.Map<List<WardsEntity>, List<WardsDto>>(wards);
        }

        [RemoteService(false)]
        public async Task<bool> CheckCodeAsync(string code)
        {
            return await _wardsRepository.AnyAsync(x => x.Code == code);
        }

        [RemoteService(false)]
        private async Task ValidateProvinceAndWardCodeAsync(CreateUpdateWardsDto input)
        {
            var provinceExists = await _provinceRepository.AnyAsync(p => p.Code == input.provinceCode);
            if (!provinceExists)
                throw new UserFriendlyException("Mã tỉnh/thành phố không hợp lệ!");

            var wardCodeExists = await _wardsRepository.AnyAsync(x => x.Code == input.Code && x.Id != input.Id);
            if (wardCodeExists)
                throw new UserFriendlyException($"Mã phường/xã '{input.Code}' đã được dùng ở bản ghi khác!");
        }
        [HttpPost("Ward/create-or-update")]
        public async Task<WardsDto> CreateOrUpdateAsync(CreateUpdateWardsDto input)
        {
            await ValidateProvinceAndWardCodeAsync(input);

            WardsEntity entity;
            if (input.Id > 0)
            {
                entity = await _wardsRepository.GetAsync(input.Id);
                ObjectMapper.Map(input, entity);
                await _wardsRepository.UpdateAsync(entity);
            }
            else
            {
                entity = ObjectMapper.Map<CreateUpdateWardsDto, WardsEntity>(input);
                await _wardsRepository.InsertAsync(entity);
            }

            return ObjectMapper.Map<WardsEntity, WardsDto>(entity);
        }

        public override async Task<WardsDto> GetAsync(int id)
        {
            var entity = await _wardsRepository.GetAsync(id);
            if (entity == null)
                throw new UserFriendlyException($"Không tìm thấy dữ liệu với Id = {id}");
            var dto = ObjectMapper.Map<WardsEntity, WardsDto>(entity);
            var province = await _provinceRepository.FirstOrDefaultAsync(p => p.Code == entity.provinceCode);
            dto.ProvinceName = province?.Name;
            return dto;
        }

        public override async Task DeleteAsync(int id)
        {
            var entity = await _wardsRepository.FindAsync(id);
            if (entity == null)
                throw new UserFriendlyException($"Không tìm thấy dữ liệu với Id = {id}");

            await _wardsRepository.DeleteAsync(entity);
        }

        public override async Task<PagedResultDto<WardsDto>> GetListAsync(WardsPagedRequestDto input)
        {
            var query = await _wardsRepository.GetQueryableAsync();

            if (!string.IsNullOrWhiteSpace(input.SearchKey))
            {
                query = query.Where(x =>
                    x.Code.Contains(input.SearchKey) ||
                    x.Name.Contains(input.SearchKey));
            }

            if (!string.IsNullOrWhiteSpace(input.SearchProvinceCode))
            {
                query = query.Where(x => x.provinceCode == input.SearchProvinceCode);
            }

            var totalCount = query.Count();
            var items = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            var result = ObjectMapper.Map<List<WardsEntity>, List<WardsDto>>(items);
            return new PagedResultDto<WardsDto>(totalCount, result);
        }
    }
}
