namespace [PROJECTNAME];

public partial class [PROJECTSUBNAME]MenuContributor : IMenuContributor
{
    private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<[PROJECTSUBNAME]Resource>();

        context.Menu.Items.Insert(
            0,
            new ApplicationMenuItem(
                [PROJECTSUBNAME]Menus.Home,
                l["Menu:Home"],
                "/",
                icon: "fas fa-home"
            )
        );
    }
}
