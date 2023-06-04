namespace [PROJECTNAME];

[DependsOn(
    typeof([PROJECTSUBNAME]ApplicationContractsModule),
    typeof(AbpAccountHttpApiModule),
    typeof(AbpIdentityHttpApiModule),
    typeof(AbpPermissionManagementHttpApiModule),
    typeof(AbpTenantManagementHttpApiModule),
    typeof(AbpFeatureManagementHttpApiModule),
    typeof(AbpSettingManagementHttpApiModule)
    )]
public class [PROJECTSUBNAME]HttpApiModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureLocalization();
    }

    private void ConfigureLocalization()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            _ = options.Resources
                .Get<[PROJECTSUBNAME]Resource>()
                .AddBaseTypes(
                    typeof(AbpUiResource)
                );
        });
    }
}
