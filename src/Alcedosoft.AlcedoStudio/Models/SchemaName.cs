namespace Alcedosoft.AlcedoStudio;

public class SchemaName
{
    public SchemaName(string name)
    {
        var subName = name
            .Split('.', StringSplitOptions.RemoveEmptyEntries)
            .LastOrDefault() ?? name;

        CamelName = $"{Char.ToLower(subName[0])}{subName[1..]}";
        PascalName = $"{Char.ToUpper(subName[0])}{subName[1..]}";
    }

    public string CamelName { get; set; }
    public string PascalName { get; set; }

    public string PluralCamelName => $"{CamelName}s";
    public string PluralPascalName => $"{PascalName}s";
}
