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
        private readonly IRepository<WardsEntity, int> _wardsRepository;

        public ProvincesDataSeedContributor(IRepository<ProvincesEntity, int> provinceRepository, IRepository<WardsEntity, int> wardsRepository)
        {
            _provinceRepository = provinceRepository;
            _wardsRepository = wardsRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            // Seed Provinces
            if (await _provinceRepository.GetCountAsync() == 0)
            {
                await _provinceRepository.InsertManyAsync(new[]
                {
            new ProvincesEntity { Code = "HN", Name = "Hà Nội" },
            new ProvincesEntity { Code = "HCM", Name = "TP Hồ Chí Minh" },
            new ProvincesEntity { Code = "HD", Name = "Hải Dương" },
            new ProvincesEntity { Code = "DN", Name = "Đà Nẵng" },
            new ProvincesEntity { Code = "HP", Name = "Hải Phòng" },
        }, autoSave: true);
            }

            // Seed Wards
            if (await _wardsRepository.GetCountAsync() == 0)
            {
                await _wardsRepository.InsertManyAsync(new[]
                {
            new WardsEntity { Code = "CG", Name = "Cầu Giấy", provinceCode = "HN" },
            new WardsEntity { Code = "VT", Name = "Vũng Tàu", provinceCode = "HCM" },
            new WardsEntity { Code = "TK", Name = "Tứ Kỳ", provinceCode = "HD"},
            new WardsEntity { Code = "NS", Name = "Nam Sách", provinceCode = "HD"},
            new WardsEntity { Code = "GL", Name = "Gia Lộc", provinceCode = "HD"},
            new WardsEntity { Code = "TH", Name = "Thanh Hà", provinceCode = "HD"},
            new WardsEntity { Code = "NT", Name = "Nha Trang", provinceCode = "DN"},
            new WardsEntity { Code = "TL", Name = "Tiên Lãng", provinceCode = "HP"},
        }, autoSave: true);
            }
        }

    }
}
