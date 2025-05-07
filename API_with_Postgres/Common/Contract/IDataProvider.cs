namespace API_with_Postgres.Common.Contract
{
    public interface IDataProvider<out T> where T : class
    {
        T Data { get; }
        Task Execute();
        bool IsAppVerify();
    }
}
