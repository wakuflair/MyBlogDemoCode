using AbpStudy.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace AbpStudy.Permissions
{
    public class AbpStudyPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(AbpStudyPermissions.GroupName);

            //Define your own permissions here. Example:
            //myGroup.AddPermission(AbpStudyPermissions.MyPermission1, L("Permission:MyPermission1"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AbpStudyResource>(name);
        }
    }
}
