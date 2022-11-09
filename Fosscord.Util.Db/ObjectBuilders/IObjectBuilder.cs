using IdGen;

namespace Fosscord.Util.Db.ObjectBuilders;

public class GenericObjectBuilder<T1> where T1 : class, new()
{
    private static IdGenerator _idGenerator = new(Environment.CurrentManagedThreadId);
    public DbModel.Db db;

    protected GenericObjectBuilder(DbModel.Db db)
    {
        this.db = db;
    }
    public string GenerateId()
    {
        return _idGenerator.CreateId() + "";
    }
    /// <summary>
    /// Creates an object and saves to database
    /// </summary>
    /// <returns>The created object</returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<T1> CreateAsync(object data)
    {
        throw new NotImplementedException();
    }
}