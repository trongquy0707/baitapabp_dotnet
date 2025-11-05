using AutoMapper;
using baitapabp.Entities;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace baitapabp.Services.Dtos
{
    [AutoMap(typeof(WardsEntity), ReverseMap = true)]
    public class WardsDto : EntityDto<int>
    {
        public string? Code { get; set; }
        public string? Name { get; set; }

        public string? provinceCode { get; set; }

        public string? ProvinceName { get; set; }
    }
    [AutoMap(typeof(WardsEntity), ReverseMap = true)]
    public class CreateUpdateWardsDto : EntityDto<int>
    {
        [Required(ErrorMessage = "Mã Xã/Phường không được để trống")]
        public string Code { get; set; } = string.Empty;
        [Required(ErrorMessage = "Tên Xã/Phường không được để trống")]
        public string Name { get; set; }
        [Required(ErrorMessage = " Mã Tỉnh thành phố không được để trống")]
        public string provinceCode { get; set; }
    }

    public class WardsPagedRequestDto : PagedAndSortedResultRequestDto
    {
        public string? SearchKey { get; set; }
        public string? SearchProvinceCode { get; set; }
    }
}
