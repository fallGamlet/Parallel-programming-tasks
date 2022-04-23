namespace SpacexApi
{
    interface ISpacexApi
    {
        public Task<List<LaunchJson>> GetLaunches();
    }
}