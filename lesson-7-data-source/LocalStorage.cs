

namespace LocalStorage
{
    interface ILocalStorage {

        LocalUser? getUser(string id);

    }

    class LocalUser
    {
        public readonly string Id;
        public readonly string Name;

        public LocalUser(string id, string name) {
            this.Id = id;
            this.Name = name;
        }

        public override string ToString()
        {
            return $"DbUser({Id},{Name})";
        }
    }
}