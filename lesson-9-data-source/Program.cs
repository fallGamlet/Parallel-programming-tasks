// See https://aka.ms/new-console-template for more information
using Data;
using Database;
using MyApplication;
using SpacexApi;

// https://api.spacexdata.com/v4/launches/latest
Console.WriteLine("DataSource app example");
var spacexApi = new SpacexApiImpl();
var launchStorage = new LaunchLocalStorageImpl();
var launchDataSource = new LaunchDataSourceImpl(spacexApi, launchStorage);
var app = new App(launchDataSource);
