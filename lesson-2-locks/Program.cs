
Thread.CurrentThread.Name = "main";
var mainThreadId = Thread.CurrentThread.ManagedThreadId;
var mainThreadName = Thread.CurrentThread.Name;
Console.WriteLine($"MainThread {mainThreadId} {mainThreadName}");

Example1();
Example2();

void Example1() {
    var data = new SingleAccessData();

    for(var i=1; i<10; i++) {
        new Thread(() => makeSomeAction(data, i)).Start();
    }
}

void makeSomeAction(SingleAccessData data, int number) {
    var name = $"{number}";
    var dataStr = data.PutName(name, name);
    var threadId = Thread.CurrentThread.ManagedThreadId;
    var msg = $"<{threadId}>\t Data {dataStr}";
    new Thread(() => {
        Console.WriteLine(msg);
    }).Start();
}

void Example2() {
    var data = new SingleAccessData();
    
    Console.WriteLine("Enter eny key for continue...");
    Console.ReadKey();
    for (int i = 0; i < 3; i++)
    {
        new Thread(() => {
            inputDataNonSynchronized(data);
        }).Start();
    }

    Console.WriteLine("Enter eny key for continue...");
    Console.ReadKey();
    for (int i = 0; i < 3; i++)
    {
        new Thread(() => {
            inputData(data);
        }).Start();
    }
}

void inputData(SingleAccessData data) {
    lock(data) {
        inputDataNonSynchronized(data);
    }
}

void inputDataNonSynchronized(SingleAccessData data) {
    var threadId = Thread.CurrentThread.ManagedThreadId;
    
    Console.Write($"\n{threadId} Enter name: ");
    var name = Console.ReadLine() ?? "";
    Console.Write($"\n{threadId} Enter second name: ");
    var secondName = Console.ReadLine() ?? "";
    var dataStr = data.PutName(name, secondName);
    Thread.Sleep(100);
    
    Console.WriteLine($"<{threadId}>\t Data {dataStr}");
}

class SingleAccessData
{
    private string name = "";
    private string secondName = "";

    public String PutName(string name, string secondName) {
        string text = "";
        lock(this) {
            this.name = name;
            Thread.Sleep(50);
            this.secondName = secondName;
            text = ToString();
        }
        return text;
    }

    public override string ToString()
    {
        return $"{name} {secondName}";
    }
}