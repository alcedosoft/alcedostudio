namespace Alcedosoft.AlcedoStudio;

public class ProjectName
{
    public ProjectName(string name)
    {
        this.Value = name;

        var subName = name
            .Split('.', StringSplitOptions.RemoveEmptyEntries)
            .LastOrDefault() ?? name;

        this.CamelSubName = $"{Char.ToLower(subName[0])}{subName[1..]}";
        this.PascalSubName = $"{Char.ToUpper(subName[0])}{subName[1..]}";
    }

    public string Value { get; set; }

    public string CamelSubName { get; set; }
    public string PascalSubName { get; set; }

    public string PluralCamelSubName => $"{this.CamelSubName}s";
    public string PluralPascalSubName => $"{this.PascalSubName}s";
}
