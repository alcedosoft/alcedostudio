namespace Alcedosoft.AlcedoStudio;

public partial class GenerateCodeCommand
{
    public async Task GeneratePageAsync(
        ProjectName projectName, FileSystemDirectoryHandle src, FileSchema schema)
    {
        var schemaName  = new SchemaName(schema.Name);

        var blazor = await src.GetDirectoryHandleAsync(
            $"{projectName.Value}.Blazor", new(){ Create = true});

        var pageDir = await blazor.GetDirectoryHandleAsync(
            "Pages", new(){ Create = true });

        var pageFile = await pageDir.GetFileHandleAsync(
            $"{schemaName.PluralPascalName}.razor", new(){ Create = true });

        var pageContent = this.GeneratePage(projectName, schema);

        await this.WriteTextAsync(pageFile, pageContent);
    }

    private string GeneratePage(ProjectName projectName, FileSchema schema)
    {
        var schemaName = new SchemaName(schema.Name);

        var list = this.GenerateList(schema, schemaName);
        var create = this.GenerateCreate(schema, schemaName);
        var update = this.GenerateUpdate(schema, schemaName);

        return $@"@page ""/{schemaName.PluralCamelName}""
@inject IStringLocalizer<{projectName.PascalSubName}Resource> L
@inject AbpBlazorMessageLocalizerHelper<{projectName.PascalSubName}Resource> LH
@inherits AbpCrudPageBase<I{schemaName.PascalName}AppService, {schemaName.PascalName}QueryDto, {schemaName.PascalName}QueryDto, Guid, PagedAndSortedResultRequestDto, {schemaName.PascalName}CreateDto, {schemaName.PascalName}UpdateDto>

<Card>
    <CardHeader>
        <Row Class=""justify-content-between"">
            <Column ColumnSize=""ColumnSize.IsAuto"">
                <h2>@L[""{schemaName.PluralPascalName}""]</h2>
            </Column>
            <Column ColumnSize=""ColumnSize.IsAuto"">
                @if (HasCreatePermission)
                {{
                    <Button Color=""Color.Primary""
                        Clicked=""OpenCreateModalAsync"">
                        @L[""New""]
                    </Button>
                }}
            </Column>
        </Row>
    </CardHeader>
    <CardBody>
        {list}
    </CardBody>
</Card>

<Modal @ref=""@CreateModal"">
    <ModalBackdrop />
    <ModalContent IsCentered=""true"">
        {create}
    </ModalContent>
</Modal>

<Modal @ref=""@EditModal"">
    <ModalBackdrop />
    <ModalContent IsCentered=""true"">
        {update}
    </ModalContent>
</Modal>

@code {{
    public {schemaName.PluralPascalName}()
    {{
        CreatePolicyName = {projectName.PascalSubName}Permissions.{schemaName.PluralPascalName}.Create;
        UpdatePolicyName = {projectName.PascalSubName}Permissions.{schemaName.PluralPascalName}.Update;
        DeletePolicyName = {projectName.PascalSubName}Permissions.{schemaName.PluralPascalName}.Delete;
    }}
}}
";
    }

    private string GenerateList(FileSchema schema, SchemaName schemaName)
    {
        var columns = new StringBuilder();

        foreach (var item in schema.Items)
        {
            _ = columns.AppendLine($@"
                <DataGridColumn TItem=""{schemaName.PascalName}QueryDto""
                                Field=""@nameof({schemaName.PascalName}QueryDto.{item.Name})""
                                Caption=""@L[""{schemaName.PascalName}:{item.Name}""]""></DataGridColumn>");
        }

        return $@"<DataGrid TItem=""{schemaName.PascalName}QueryDto""
                  Data=""Entities""
                  ReadData=""OnDataGridReadAsync""
                  CurrentPage=""CurrentPage""
                  TotalItems=""TotalCount""
                  ShowPager=""true""
                  PageSize=""PageSize"">
            <DataGridColumns>
                <DataGridEntityActionsColumn TItem=""{schemaName.PascalName}QueryDto"" @ref=""@EntityActionsColumn"">
                    <DisplayTemplate>
                        <EntityActions TItem=""{schemaName.PascalName}QueryDto"" EntityActionsColumn=""@EntityActionsColumn"">
                            <EntityAction TItem=""{schemaName.PascalName}QueryDto""
                                          Text=""@L[""Edit""]""
                                          Visible=""@HasUpdatePermission""
                                          Clicked=""() => OpenEditModalAsync(context)"" />
                            <EntityAction TItem=""{schemaName.PascalName}QueryDto""
                                          Text=""@L[""Delete""]""
                                          Visible=""@HasDeletePermission""
                                          Clicked=""() => DeleteEntityAsync(context)""
                                          ConfirmationMessage=""()=>GetDeleteConfirmationMessage(context)"" />
                        </EntityActions>
                    </DisplayTemplate>
                </DataGridEntityActionsColumn>
{columns}
            </DataGridColumns>
        </DataGrid>";
    }

    private string GenerateCreate(FileSchema schema, SchemaName schemaName)
    {
        var fields = new StringBuilder();

        foreach (var item in schema.Items)
        {
            if (item.DataType is "int" or "float")
            {
                _ = fields.AppendLine($@"
                    <Validation MessageLocalizer=""@LH.Localize"">
                        <Field>
                            <FieldLabel>@L[""{schemaName.PascalName}:{item.Name}""]</FieldLabel>
                            <NumericEdit TValue=""{item.DataType}"" Placeholder=""{item.Description}"" @bind-Text=""@NewEntity.{item.Name}"">
                                <Feedback>
                                    <ValidationError />
                                </Feedback>
                            </NumericEdit>
                        </Field>
                    </Validation>");
            }
            else if (item.DataType is "bool")
            {
                _ = fields.AppendLine($@"
                    <Validation MessageLocalizer=""@LH.Localize"">
                        <Field>
                            <Check TValue=""bool"" @bind-Checked=""@NewEntity.{item.Name}"">
                                @L[""{schemaName.PascalName}:{item.Name}""]
                            </Check>
                        </Field>
                    </Validation>");
            }
            else if (item.DataType is "DateTime")
            {
                _ = fields.AppendLine($@"
                    <Validation MessageLocalizer=""@LH.Localize"">
                        <Field>
                            <FieldLabel>@L[""{schemaName.PascalName}:{item.Name}""]</FieldLabel>
                            <DateEdit TValue=""DateTime"" Placeholder=""{item.Description}"" @bind-Text=""@NewEntity.{item.Name}"">
                                <Feedback>
                                    <ValidationError />
                                </Feedback>
                            </DateEdit>
                        </Field>
                    </Validation>");
            }
            else
            {
                _ = fields.AppendLine($@"
                    <Validation MessageLocalizer=""@LH.Localize"">
                        <Field>
                            <FieldLabel>@L[""{schemaName.PascalName}:{item.Name}""]</FieldLabel>
                            <TextEdit Placeholder=""{item.Description}"" @bind-Text=""@NewEntity.{item.Name}"">
                                <Feedback>
                                    <ValidationError />
                                </Feedback>
                            </TextEdit>
                        </Field>
                    </Validation>");
            }
        }

        return $@"<Form>
            <ModalHeader>
                <ModalTitle>@L[""New""]</ModalTitle>
                <CloseButton Clicked=""CloseCreateModalAsync"" />
            </ModalHeader>
            <ModalBody>
                <Validations @ref=""@CreateValidationsRef"" Model=""@NewEntity"" ValidateOnLoad=""false"">
{fields}
                </Validations>
            </ModalBody>
            <ModalFooter>
                <Button Color=""Color.Secondary""
                        Clicked=""CloseCreateModalAsync"">
                    @L[""Cancel""]
                </Button>
                <Button Color=""Color.Primary""
                        Type=""@ButtonType.Submit""
                        PreventDefaultOnSubmit=""true""
                        Clicked=""CreateEntityAsync"">
                    @L[""Save""]
                </Button>
            </ModalFooter>
        </Form>";
    }

    private string GenerateUpdate(FileSchema schema, SchemaName schemaName)
    {
        var fields = new StringBuilder();

        foreach (var item in schema.Items)
        {
            if (item.DataType is "int" or "float")
            {
                _ = fields.AppendLine($@"
                    <Validation MessageLocalizer=""@LH.Localize"">
                        <Field>
                            <FieldLabel>@L[""{schemaName.PascalName}:{item.Name}""]</FieldLabel>
                            <NumericEdit TValue=""{item.DataType}"" Placeholder=""{item.Description}"" @bind-Text=""@EditingEntity.{item.Name}"">
                                <Feedback>
                                    <ValidationError />
                                </Feedback>
                            </NumericEdit>
                        </Field>
                    </Validation>");
            }
            else if (item.DataType is "bool")
            {
                _ = fields.AppendLine($@"
                    <Validation MessageLocalizer=""@LH.Localize"">
                        <Field>
                            <Check TValue=""bool"" @bind-Checked=""@EditingEntity.{item.Name}"">
                                @L[""{schemaName.PascalName}:{item.Name}""]
                            </Check>
                        </Field>
                    </Validation>");
            }
            else if (item.DataType is "DateTime")
            {
                _ = fields.AppendLine($@"
                    <Validation MessageLocalizer=""@LH.Localize"">
                        <Field>
                            <FieldLabel>@L[""{schemaName.PascalName}:{item.Name}""]</FieldLabel>
                            <DateEdit TValue=""DateTime"" Placeholder=""{item.Description}"" @bind-Text=""@EditingEntity.{item.Name}"">
                                <Feedback>
                                    <ValidationError />
                                </Feedback>
                            </DateEdit>
                        </Field>
                    </Validation>");
            }
            else
            {
                _ = fields.AppendLine($@"
                    <Validation MessageLocalizer=""@LH.Localize"">
                        <Field>
                            <FieldLabel>@L[""{schemaName.PascalName}:{item.Name}""]</FieldLabel>
                            <TextEdit Placeholder=""{item.Description}"" @bind-Text=""@EditingEntity.{item.Name}"">
                                <Feedback>
                                    <ValidationError />
                                </Feedback>
                            </TextEdit>
                        </Field>
                    </Validation>");
            }
        }

        return $@"<Form>
            <ModalHeader>
                <ModalTitle>@EditingEntity.Id</ModalTitle>
                <CloseButton Clicked=""CloseEditModalAsync"" />
            </ModalHeader>
            <ModalBody>
                <Validations @ref=""@EditValidationsRef"" Model=""@EditingEntity"" ValidateOnLoad=""false"">
{fields}
                </Validations>
            </ModalBody>
            <ModalFooter>
                <Button Color=""Color.Secondary""
                        Clicked=""CloseEditModalAsync"">
                    @L[""Cancel""]
                </Button>
                <Button Color=""Color.Primary""
                        Type=""@ButtonType.Submit""
                        PreventDefaultOnSubmit=""true""
                        Clicked=""UpdateEntityAsync"">
                    @L[""Save""]
                </Button>
            </ModalFooter>
        </Form>";
    }
}
