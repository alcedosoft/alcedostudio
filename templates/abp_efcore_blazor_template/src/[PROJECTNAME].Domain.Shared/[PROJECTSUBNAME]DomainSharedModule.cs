namespace [PROJECTNAME];

[DependsOn(
    typeof(AbpIdentityDomainSharedModule),
    typeof(AbpIdentityServerDomainSharedModule),
    typeof(AbpAuditLoggingDomainSharedModule),
    typeof(AbpBackgroundJobsDomainSharedModule),
    typeof(AbpFeatureManagementDomainSharedModule),
    typeof(AbpPermissionManagementDomainSharedModule),
    typeof(AbpSettingManagementDomainSharedModule),
    typeof(AbpTenantManagementDomainSharedModule)
    )]
public class [PROJECTSUBNAME]DomainSharedModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        [PROJECTSUBNAME]GlobalFeatureConfigurator.Configure();
        [PROJECTSUBNAME]ModuleExtensionConfigurator.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<[PROJECTSUBNAME]DomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            _ = options.Resources
                .Add<[PROJECTSUBNAME]Resource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/[PROJECTSUBNAME]");

            options.DefaultResourceType = typeof([PROJECTSUBNAME]Resource);
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("[PROJECTSUBNAME]", typeof([PROJECTSUBNAME]Resource));
        });
    }
}
