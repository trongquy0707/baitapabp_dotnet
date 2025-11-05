using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace baitapabp.Entities
{
    public class ProvincesEntity : FullAuditedEntity<int>
    {
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;
        [MaxLength(200)]
        public string? Name { get; set; }
    }
}
