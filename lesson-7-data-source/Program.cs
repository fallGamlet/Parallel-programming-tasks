// See https://aka.ms/new-console-template for more information
using LocalStorage;
using MyApplication;

Console.WriteLine("DataSource app example");
ILocalStorage localStorage = new LocalStorageSqlite("Data Source=database.db"); 
// ILocalStorage localStorage = new LocalStorageMemory(); 
var app = new App(localStorage);
