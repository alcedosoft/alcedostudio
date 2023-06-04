namespace [PROJECTNAME];

[DependsOn(
    typeof([PROJECTSUBNAME]DomainModule),
    typeof([PROJECTSUBNAME]ApplicationContractsModule),
    typeof(AbpAccountApplicationModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule)
    )]
public class [PROJECTSUBNAME]ApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<[PROJECTSUBNAME]ApplicationModule>();
        });
    }
}
