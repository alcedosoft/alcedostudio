namespace [PROJECTNAME];

public static class [PROJECTSUBNAME]DtoExtensions
{
    private static readonly OneTimeRunner _oneTimeRunner = new();

    public static void Configure()
    {
        _oneTimeRunner.Run(() =>
        {
            /* You can add extension properties to DTOs
             * defined in the depended modules.
             *
             * Example:
             *
             * ObjectExtensionManager.Instance
             *   .AddOrUpdateProperty<IdentityRoleDto, string>("Title");
             *
             * See the documentation for more:
             * https://docs.abp.io/en/abp/latest/Object-Extensions
             */
        });
    }
}
