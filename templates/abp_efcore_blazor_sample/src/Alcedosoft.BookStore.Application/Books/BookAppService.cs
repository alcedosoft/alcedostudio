namespace Alcedosoft.BookStore;

public class BookAppService
    : CrudAppService<Book, BookDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateBookDto>
    , IBookAppService
{
    public BookAppService(IRepository<Book, Guid> repository) : base(repository)
    {

    }
}
