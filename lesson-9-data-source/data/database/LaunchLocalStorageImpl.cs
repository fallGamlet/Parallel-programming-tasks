using Domain;
using Microsoft.Data.Sqlite;

namespace Database
{
    public class LaunchLocalStorageImpl: ILaunchLocalStorage
    {
        private Dictionary<string, Launch> launchDict = new Dictionary<string, Launch>();
        private readonly SqliteConnection connection;

        public LaunchLocalStorageImpl()
        {
            connection = new SqliteConnection("Data Source=database.db");
            connection.Open();
            Configure();
        }

        private void Configure()
        {
            var tableImagesCreate = connection.CreateCommand();
            tableImagesCreate.CommandText = @"
                CREATE TABLE IF NOT EXISTS images (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    small TEXT DEFAULT '',
                    large TEXT DEFAULT ''
                );
            ";
            tableImagesCreate.ExecuteNonQuery();

            var tableLaunchesCreate = connection.CreateCommand();
            tableImagesCreate.CommandText = @"
                CREATE TABLE IF NOT EXISTS launches (
                    id TEXT PRIMARY KEY,
                    name TEXT DEFAULT '',
                    description TEXT DEFAULT '',
                    date INTEGER DEFAULT 0,
                    video_url TEXT DEFAULT '',
                    article_url TEXT DEFAULT '',
                    image_id INTEGER DEFAULT 0
                );
            ";
            tableImagesCreate.ExecuteNonQuery();

            var tableLaunchesImagesRelsCreate = connection.CreateCommand();
            tableImagesCreate.CommandText = @"
                CREATE TABLE IF NOT EXISTS launches_images_rels (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    launch_id TEXT,
                    image_id INTEGER
                );
            ";
            tableImagesCreate.ExecuteNonQuery();
        }

        ~LaunchLocalStorageImpl()
        {
            connection.Close();
        }

        public async Task<Boolean> Save(List<Launch> launches) 
        {
            launchDict.Clear();
            clearTables();

            launches.ForEach(item => 
            {
                launchDict[item.Id] = item;
                SaveLaunch(item);
            });

            return true;
        }

        private void clearTables() {
            var imagesTeleteCommand = connection.CreateCommand();
            imagesTeleteCommand.CommandText = @"DELETE FROM images";
            var result = imagesTeleteCommand.ExecuteNonQuery();

            var launchesDeleteCommand = connection.CreateCommand();
            launchesDeleteCommand.CommandText = @"DELETE FROM launches";
            result = launchesDeleteCommand.ExecuteNonQuery();

            var relsDeleteCommand = connection.CreateCommand();
            relsDeleteCommand.CommandText = @"DELETE FROM launches_images_rels";
            result = relsDeleteCommand.ExecuteNonQuery();
        }

        private void SaveLaunch(Launch launch)
        {
            var imageId = SaveImage(launch.Image);
            SaveLaunch(launch, imageId);
            List<long> imageIds = launch.Images.Select(image => 
            {
                var rowId = SaveImage(image);
                return rowId;
            }).ToList();
            SaveImagesRels(launch.Id, imageIds);
        }

        private void SaveLaunch(Launch launch, long imageId) 
        {
            var command = connection.CreateCommand();
            command.CommandText = 
            @"
                INSERT INTO launches 
                (
                    id,
                    name,
                    description,
                    date,
                    video_url,
                    article_url,
                    image_id
                ) 
                VALUES 
                (
                    $id,
                    $name,
                    $description,
                    $date,
                    $video_url,
                    $article_url,
                    $image_id
                ) 
            ";

            var date = new DateTimeOffset(launch.Date);
            var unitTime = date.ToUnixTimeMilliseconds();

            command.Parameters.AddWithValue("$id", launch.Id);
            command.Parameters.AddWithValue("$name", launch.Name);
            command.Parameters.AddWithValue("$description", launch.Description);
            command.Parameters.AddWithValue("$date", unitTime);
            command.Parameters.AddWithValue("$video_url", launch.Id);
            command.Parameters.AddWithValue("$article_url", launch.Id);
            command.Parameters.AddWithValue("$image_id", imageId);

            command.ExecuteNonQuery();
        }

        private long SaveImage(ImageUrl image) 
        {
            var command = connection.CreateCommand();
            command.CommandText = 
            @"
                INSERT INTO images 
                (small, large) 
                VALUES 
                ($small, $large)
            ";
            command.Parameters.AddWithValue("$small", image.Small);
            command.Parameters.AddWithValue("$large", image.Large);

            command.ExecuteNonQuery();
            return getLastInsertedRowId();
        }

        private long getLastInsertedRowId()
        {
            var command = connection.CreateCommand();
            command.CommandText = @"select last_insert_rowid()";
            var result = command.ExecuteScalar();
            return (long) result;
        }

        private void SaveImagesRels(string launchId, List<long> imageIds)
        {
            imageIds.ForEach(id => 
            {
                var command = connection.CreateCommand();
                command.CommandText = 
                @"
                    INSERT INTO launches_images_rels 
                    (launch_id, image_id) 
                    VALUES 
                    ($launch_id, $image_id)
                ";
                command.Parameters.AddWithValue("$launch_id", launchId);
                command.Parameters.AddWithValue("$image_id", id);
                command.ExecuteNonQuery();
            });
        }

        public async Task<List<Launch>> GetAll() 
        {
            return launchDict.Values.ToList();
        }

        public async Task<Launch> GetById(string launchId)
        {
            try {
                return launchDict[launchId];
            } catch(Exception e) {
                throw new KeyNotFoundException($"launch with Id {launchId} not found", e);
            }
        }
    }
}