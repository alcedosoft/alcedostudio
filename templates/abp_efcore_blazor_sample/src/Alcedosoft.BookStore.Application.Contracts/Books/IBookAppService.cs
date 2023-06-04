namespace Alcedosoft.BookStore;

public interface IBookAppService
    : ICrudAppService<BookDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateBookDto>
{
}
