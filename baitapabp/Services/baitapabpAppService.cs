using Volo.Abp.Application.Services;
using baitapabp.Localization;

namespace baitapabp.Services;

/* Inherit your application services from this class. */
public abstract class baitapabpAppService : ApplicationService
{
    protected baitapabpAppService()
    {
        LocalizationResource = typeof(baitapabpResource);
    }
}