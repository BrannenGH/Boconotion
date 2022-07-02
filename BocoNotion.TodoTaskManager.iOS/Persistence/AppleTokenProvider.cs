namespace BocoNotion.TodoTaskManager.iOS.Persistence 
{
    using System.Threading.Tasks;
    using BocoNotion.TodoTaskManager.Persistence;
    using PerpetualEngine.Storage;

    public class AppleTokenProvider : ITokenProvider
    {
        private SimpleStorage storage;

        public AppleTokenProvider()
        {
            storage = SimpleStorage.EditGroup("BocoNotion");
        }

        public async Task<string> GetToken()
        {
            return await storage.GetAsync<string>("Token");
        }

        public async Task SetToken(string token)
        {
            await storage.PutAsync<string>("Token", token);
        }
    }
}