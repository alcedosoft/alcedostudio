namespace Alcedosoft.BookStore;

public interface IBookStoreDbSchemaMigrator
{
    Task MigrateAsync();
}
