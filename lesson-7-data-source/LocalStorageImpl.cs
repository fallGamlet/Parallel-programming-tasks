
using Microsoft.Data.Sqlite;

namespace LocalStorage
{
    class LocalStorageSqlite: ILocalStorage {
        private SqliteConnection connection;
        public LocalStorageSqlite(string dbFilePath) 
        {
            connection = new SqliteConnection(dbFilePath);
            connection.Open();
            initDatabase();
            fillInitialData();
        }

        ~LocalStorageSqlite() 
        {
            connection.Close();
        }

        private void initDatabase() 
        {
            var commandDbCreate = connection.CreateCommand();
            commandDbCreate.CommandText = @"
                CREATE TABLE IF NOT EXISTS user(
                    id TEXT PRIMARY KEY, 
                    name TEXT
                );
            ";
            commandDbCreate.ExecuteNonQuery();
        }

        private void fillInitialData() 
        {
            var commandChekUsers = connection.CreateCommand();
            commandChekUsers.CommandText = @"SELECT COUNT(id) FROM user";
            var result = commandChekUsers.ExecuteScalar();
            long count = (long) result;
            if (count > 0) return;

            var commandInsert = connection.CreateCommand();
            commandInsert.CommandText = @"
                INSERT INTO user (id, name) VALUES ($id, $name);
            ";

            commandInsert.Parameters.AddWithValue("$id", "1");
            commandInsert.Parameters.AddWithValue("$name", "Unser Name 1");

            commandInsert.ExecuteNonQuery();
        }

        public LocalUser? getUser(string id)
        {
            var command = connection.CreateCommand();
            command.CommandText =
            @"
                SELECT id, name
                FROM user
                WHERE id = $id
            ";
            command.Parameters.AddWithValue("$id", id);

            using (var reader = command.ExecuteReader())
            {
                if (!reader.Read()) return null;

                return new LocalUser(
                    reader.GetString(0), 
                    reader.GetString(1)
                    );
            }
        }
    }
}