namespace Alcedosoft.BookStore;

[Volo.Abp.DependencyInjection.Dependency(ReplaceServices = true)]
public class BookStoreBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "BookStore";
}
