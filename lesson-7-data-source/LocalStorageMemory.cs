
namespace LocalStorage
{
    class LocalStorageMemory : ILocalStorage
    {
        private Dictionary<string, LocalUser> source = new Dictionary<string, LocalUser>();

        public LocalStorageMemory()
        {
            source["1"] = new LocalUser("1", "Memory user 1");
        }

        public LocalUser? getUser(string id)
        {
            if (source.ContainsKey(id)) 
            {
                return source[id];
            } else {
                return null;
            }
        }
    }
}
