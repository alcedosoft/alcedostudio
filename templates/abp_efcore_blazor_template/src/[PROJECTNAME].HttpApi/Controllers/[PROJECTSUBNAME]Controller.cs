namespace [PROJECTNAME];

/* Inherit your controllers from this class.
 */
public abstract class [PROJECTSUBNAME]Controller : AbpControllerBase
{
    protected [PROJECTSUBNAME]Controller()
    {
        LocalizationResource = typeof([PROJECTSUBNAME]Resource);
    }
}
