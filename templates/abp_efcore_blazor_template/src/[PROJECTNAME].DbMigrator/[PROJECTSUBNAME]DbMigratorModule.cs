namespace [PROJECTNAME];

[DependsOn(
    typeof(AbpAutofacModule),
    typeof([PROJECTSUBNAME]EntityFrameworkCoreModule),
    typeof([PROJECTSUBNAME]ApplicationContractsModule)
    )]
public class [PROJECTSUBNAME]DbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
    }
}
