using Microsoft.Extensions.Localization;
using baitapabp.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace baitapabp;

[Dependency(ReplaceServices = true)]
public class baitapabpBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<baitapabpResource> _localizer;

    public baitapabpBrandingProvider(IStringLocalizer<baitapabpResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}