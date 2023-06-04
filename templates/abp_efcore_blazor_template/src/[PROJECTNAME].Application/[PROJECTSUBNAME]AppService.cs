namespace [PROJECTNAME];

/* Inherit your application services from this class.
 */
public abstract class [PROJECTSUBNAME]AppService : ApplicationService
{
    protected [PROJECTSUBNAME]AppService()
    {
        LocalizationResource = typeof([PROJECTSUBNAME]Resource);
    }
}
