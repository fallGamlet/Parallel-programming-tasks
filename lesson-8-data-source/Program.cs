// See https://aka.ms/new-console-template for more information
using Data;
using MyApplication;

// https://api.spacexdata.com/v4/launches/latest
Console.WriteLine("DataSource app example");
// ILocalStorage localStorage = new LocalStorageSqlite("Data Source=database.db"); 
var launchDataSource = new LaunchDataSourceImpl();
var app = new App(launchDataSource);
