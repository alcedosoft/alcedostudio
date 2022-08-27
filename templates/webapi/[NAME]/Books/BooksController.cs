namespace [NAME];

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly DataContext _context;

    public BooksController(DataContext context)
    {
        _context = context;
    }

    // GET: api/Books
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetBook()
    {
        if (_context.Book == null)
        {
            return NotFound();
        }

        return await _context.Book.ToListAsync();
    }

    // GET: api/Books/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetBook(int id)
    {
        if (_context.Book == null)
        {
            return NotFound();
        }

        var book = await _context.Book.FindAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        return book;
    }

    // PUT: api/Books/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutBook(int id, Book book)
    {
        if (id != book.Id)
        {
            return BadRequest();
        }

        _context.Entry(book).State = EntityState.Modified;

        try
        {
            _ = await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BookExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Books
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Book>> PostBook(Book book)
    {
        if (_context.Book == null)
        {
            return Problem("Entity set 'WebApplication1Context.Book'  is null.");
        }

        _ = _context.Book.Add(book);
        _ = await _context.SaveChangesAsync();

        return CreatedAtAction("GetBook", new { id = book.Id }, book);
    }

    // DELETE: api/Books/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        if (_context.Book == null)
        {
            return NotFound();
        }

        var book = await _context.Book.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }

        _ = _context.Book.Remove(book);
        _ = await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool BookExists(int id)
    {
        return (_context.Book?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
