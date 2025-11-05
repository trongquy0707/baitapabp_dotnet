using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace baitapabp.Entities
{
    public class WardsEntity : FullAuditedEntity<int>
    {
        [MaxLength(50)]
        public string? Code { get; set; } = string.Empty;
        [Required]
        [MaxLength(200)]
        public string? Name { get; set; }
        [Required]
        public string? provinceCode { get; set; }
    }
}
