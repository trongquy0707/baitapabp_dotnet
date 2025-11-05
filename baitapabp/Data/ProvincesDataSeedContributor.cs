using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using baitapabp.Entities; // namespace chứa ProvincesEntity

namespace baitapabp.Data.Seed
{
    public class ProvincesDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<ProvincesEntity, int> _provinceRepository;

        public ProvincesDataSeedContributor(IRepository<ProvincesEntity, int> provinceRepository)
        {
            _provinceRepository = provinceRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (await _provinceRepository.GetCountAsync() > 0)
            {
                // Đã có dữ liệu -> bỏ qua
                return;
            }

            await _provinceRepository.InsertManyAsync(new[]
            {
                new ProvincesEntity { Code = "HN", Name = "Hà Nội" },
                new ProvincesEntity { Code = "HCM", Name = "TP Hồ Chí Minh" },
                new ProvincesEntity { Code = "HD", Name = "Hải Dương" },
                new ProvincesEntity { Code = "DN", Name = "Đà Nẵng" },
                new ProvincesEntity { Code = "HP", Name = "Hải Phòng" },
            }, autoSave: true);
        }
    }
}
