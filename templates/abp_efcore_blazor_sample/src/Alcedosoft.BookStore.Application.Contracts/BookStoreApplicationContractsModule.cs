namespace Alcedosoft.BookStore;

[DependsOn(
    typeof(BookStoreDomainSharedModule),
    typeof(AbpObjectExtendingModule),
    typeof(AbpAccountApplicationContractsModule),
    typeof(AbpIdentityApplicationContractsModule),
    typeof(AbpFeatureManagementApplicationContractsModule),
    typeof(AbpPermissionManagementApplicationContractsModule),
    typeof(AbpSettingManagementApplicationContractsModule),
    typeof(AbpTenantManagementApplicationContractsModule)
)]
public class BookStoreApplicationContractsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        BookStoreDtoExtensions.Configure();
    }
}
