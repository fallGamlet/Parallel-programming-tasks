namespace Domain
{
    interface ILaunchDataSource {
        public Task<List<Launch>> getLaunches();

        public Task<Launch> getLaunch(string launchId);
    }
}