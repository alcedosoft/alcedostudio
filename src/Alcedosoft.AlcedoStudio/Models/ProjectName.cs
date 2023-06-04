namespace Alcedosoft.AlcedoStudio;

public class ProjectName
{
    public ProjectName(string name)
    {
        Value = name;

        var subName = name
            .Split('.', StringSplitOptions.RemoveEmptyEntries)
            .LastOrDefault() ?? name;

        CamelSubName = $"{Char.ToLower(subName[0])}{subName[1..]}";
        PascalSubName = $"{Char.ToUpper(subName[0])}{subName[1..]}";
    }

    public string Value { get; set; }

    public string CamelSubName { get; set; }
    public string PascalSubName { get; set; }

    public string PluralCamelSubName => $"{CamelSubName}s";
    public string PluralPascalSubName => $"{PascalSubName}s";
}
