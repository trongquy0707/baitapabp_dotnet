using baitapabp.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace baitapabp.Permissions;

public class baitapabpPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(baitapabpPermissions.GroupName);


        
        //Define your own permissions here. Example:
        //myGroup.AddPermission(baitapabpPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<baitapabpResource>(name);
    }
}
