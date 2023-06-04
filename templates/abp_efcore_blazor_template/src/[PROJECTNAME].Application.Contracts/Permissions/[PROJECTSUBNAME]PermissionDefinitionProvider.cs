namespace [PROJECTNAME];

public class [PROJECTSUBNAME]PermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        _ = context.AddGroup([PROJECTSUBNAME]Permissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission([PROJECTSUBNAME]Permissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<[PROJECTSUBNAME]Resource>(name);
    }
}
