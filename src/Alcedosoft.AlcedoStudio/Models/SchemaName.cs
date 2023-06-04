namespace Alcedosoft.AlcedoStudio;

public class SchemaName
{
    public SchemaName(string name)
    {
        var subName = name
            .Split('.', StringSplitOptions.RemoveEmptyEntries)
            .LastOrDefault() ?? name;

        this.CamelName = $"{Char.ToLower(subName[0])}{subName[1..]}";
        this.PascalName = $"{Char.ToUpper(subName[0])}{subName[1..]}";
    }

    public string CamelName { get; set; }
    public string PascalName { get; set; }

    public string PluralCamelName => $"{this.CamelName}s";
    public string PluralPascalName => $"{this.PascalName}s";
}
