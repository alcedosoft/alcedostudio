namespace [PROJECTNAME];

[DependsOn(
    typeof([PROJECTSUBNAME]DomainSharedModule),
    typeof(AbpObjectExtendingModule),
    typeof(AbpAccountApplicationContractsModule),
    typeof(AbpIdentityApplicationContractsModule),
    typeof(AbpFeatureManagementApplicationContractsModule),
    typeof(AbpPermissionManagementApplicationContractsModule),
    typeof(AbpSettingManagementApplicationContractsModule),
    typeof(AbpTenantManagementApplicationContractsModule)
)]
public class [PROJECTSUBNAME]ApplicationContractsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        [PROJECTSUBNAME]DtoExtensions.Configure();
    }
}
