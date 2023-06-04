using Volo.Abp.Modularity;

namespace [PROJECTNAME];

[DependsOn(
    typeof([PROJECTSUBNAME]EntityFrameworkCoreTestModule)
    )]
public class [PROJECTSUBNAME]DomainTestModule : AbpModule
{

}
