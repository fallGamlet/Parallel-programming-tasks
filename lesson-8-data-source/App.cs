using Domain;

namespace MyApplication
{
    class App
    {
        private readonly ILaunchDataSource launchDataSource;
        public App(ILaunchDataSource launchDataSource) 
        {
            this.launchDataSource = launchDataSource;
            showLaunches().Wait();

            Console.Write("\n\n==> Enter ID of launch for view details: ");
            string launchId = Console.ReadLine() ?? "";
            if (launchId.Length != 0)
            {
                showLaunchDetails(launchId).Wait();
            }
        }

        private Task showLaunches() {
            return launchDataSource.getLaunches()
            .ContinueWith<Launch>(task => 
            {
                var launches = task.Result;
                launches.ForEach(launch => {
                    Console.WriteLine($"\n{launch}");
                });

                var resTask = launchDataSource.getLaunch(launches.First().Id);
                resTask.Wait();
                return resTask.Result;
            });
        }

        private Task showLaunchDetails(string launchId)
        {
            return launchDataSource.getLaunch(launchId)
            .ContinueWith(task => 
            {
                var launch = task.Result;
                Console.WriteLine($"LaunchDetails => {launch}");
            });
        }
    }
}