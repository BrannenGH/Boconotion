namespace BocoNotion.TodoTaskManager.Persistence
{
    using System;
    using System.Threading.Tasks;

    public interface ITokenProvider
    {
        Task<string> GetToken();

        Task SetToken(string token);
    }
}
