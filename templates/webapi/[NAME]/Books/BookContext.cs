namespace [NAME];

public partial class DataContext
{
    public DbSet<Book> Book { get; set; } = default!;
}
