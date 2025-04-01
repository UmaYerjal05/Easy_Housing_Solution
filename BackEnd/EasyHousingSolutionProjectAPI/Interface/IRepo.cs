namespace EasyHousingSolutionProjectAPI.Interface
{
    public interface IRepo<T> where T : class
    {
        Task<bool> Add(T entity);
        bool Update(T entity);
        T Get(int id);
        List<T> GetAll();

        bool Delete(int id);
    }
}
