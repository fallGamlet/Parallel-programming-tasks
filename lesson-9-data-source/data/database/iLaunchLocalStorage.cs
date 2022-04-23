using Domain;

namespace Database
{
    public interface ILaunchLocalStorage
    {
        public Task<Boolean> Save(List<Launch> launches);

        public Task<List<Launch>> GetAll();

        public Task<Launch> GetById(string launchId);
    }
}