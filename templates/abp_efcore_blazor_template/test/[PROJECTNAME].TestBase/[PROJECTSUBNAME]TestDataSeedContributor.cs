using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace [PROJECTNAME];

public class [PROJECTSUBNAME]TestDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    public Task SeedAsync(DataSeedContext context)
    {
        /* Seed additional test data... */

        return Task.CompletedTask;
    }
}
