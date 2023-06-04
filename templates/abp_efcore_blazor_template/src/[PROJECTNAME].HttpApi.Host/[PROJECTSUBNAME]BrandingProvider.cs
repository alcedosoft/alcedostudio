namespace [PROJECTNAME];

[Volo.Abp.DependencyInjection.Dependency(ReplaceServices = true)]
public class [PROJECTSUBNAME]BrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "[PROJECTSUBNAME]";
}
