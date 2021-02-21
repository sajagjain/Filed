namespace Filed.Services.Contracts
{
    public interface IRepository<T> where T : class
    {
        int Save(T t);
    }
}
