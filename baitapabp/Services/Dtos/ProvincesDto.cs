using AutoMapper;
using baitapabp.Entities;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace baitapabp.Services.Dtos
{
    [AutoMap(typeof(ProvincesEntity), ReverseMap = true)]
    public class ProvincesDto : EntityDto<int>
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
    }

    [AutoMap(typeof(ProvincesEntity), ReverseMap = true)]
    public class CreateUpdateProvincesDto : EntityDto<int>
    {
        [Required(ErrorMessage = "Mã tỉnh/thành phố không được để trống")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên tỉnh/thành phố không được để trống")]
        public string? Name { get; set; }
    }

    public class ProvincesPagedRequestDto : PagedAndSortedResultRequestDto
    {
        public string? SearchKey { get; set; }
    }
}
