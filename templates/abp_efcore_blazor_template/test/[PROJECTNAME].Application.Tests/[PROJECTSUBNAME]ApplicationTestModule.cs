using Volo.Abp.Modularity;

namespace [PROJECTNAME];

[DependsOn(
    typeof([PROJECTSUBNAME]ApplicationModule),
    typeof([PROJECTSUBNAME]DomainTestModule)
    )]
public class [PROJECTSUBNAME]ApplicationTestModule : AbpModule
{

}
