using LocalStorage;

namespace MyApplication
{
    class App
    {
        public App(ILocalStorage localStorage) 
        {
            var user = localStorage.getUser("1");
            Console.WriteLine($"{user}");
        }
    }
}