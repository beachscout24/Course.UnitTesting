namespace TestingTechniques;

public class ValueSamples
{
    public string FullName = "Eric Clarke";

    public int Age = 53;

    public DateOnly DateOfBirth = new (1970, 6, 9);

    public User AppUser = new()
    {
        FullName = "Eric Clarke",
        Age = 53,
        DateOfBirth = new (1970, 6, 9)
    };

    public IEnumerable<User> Users = new[]
    {
        new User()
        {
            FullName = "Eric Clarke",
            Age = 53,
            DateOfBirth = new (1970, 6, 9)
        },
        new User()
        {
            FullName = "Tom Scott",
            Age = 37,
            DateOfBirth = new (1984, 6, 9)
        },
        new User()
        {
            FullName = "Steve Mould",
            Age = 43,
            DateOfBirth = new (1978, 10, 5)
        }
    };

    public IEnumerable<int> Numbers = new[] { 1, 5, 10, 15 };

    public event EventHandler ExampleEvent;

    internal int InternalSecretNumber = 42;

    public virtual void RaiseExampleEvent()
    {
        ExampleEvent(this, EventArgs.Empty);
    }
}
