using SpacexApi;
using Domain;
using Database;

namespace Data
{
    class LaunchDataSourceImpl : ILaunchDataSource
    {
        private readonly ISpacexApi api;
        private readonly ILaunchLocalStorage storage;
        private readonly LaunchMapper launchMapper;
        private Dictionary<string, Launch> launchDict = new Dictionary<string, Launch>();

        public LaunchDataSourceImpl(ISpacexApi api, ILaunchLocalStorage storage) 
        {
            this.api = api;
            this.storage = storage;
            this.launchMapper = new LaunchMapper();
        }

        public async Task<List<Launch>> getLaunches()
        {
            await UpdateData();
            return await GetLaunchesLocal();
        }

        private async Task<bool> UpdateData()
        {
            if (IsCacheExpired()) {
                var launchListJson = await api.GetLaunches();
                List<Launch> launchList = launchMapper.MapLaunches(launchListJson);
                return await storage.Save(launchList);
            } else {
                return false;
            }
        }

        private bool IsCacheExpired() {
            return true;
        }

        private Task<List<Launch>> GetLaunchesLocal()
        {
            return storage.GetAll();
        }

        public Task<Launch> getLaunch(string launchId)
        {
            return storage.GetById(launchId);
        }
    }
}