// See https://aka.ms/new-console-template for more information
using System.Text;

Console.WriteLine("Linq parallel example!");

var srcData = Enumerable.Range(0, 100).ToList();
var timeStart = DateTime.Now;

var dataStrList = srcData
.AsParallel() // make handle of collection parallele
.Select((number) => 
{
    Thread.Sleep(10); // Imitation of long job
    var threadId = Thread.CurrentThread.ManagedThreadId;
    return new Result1(number.ToString(), threadId);
})
.ToList();

var timeEnd = DateTime.Now;
var duration = (timeEnd - timeStart);

Console.WriteLine($"Time duration: {duration}");
var groups = dataStrList.GroupBy(value => value.ThreadId);

var threadIds = groups.Select(value => value.Key);
var threadIdsStr = String.Join(", ", threadIds);
Console.WriteLine($"Threads count {threadIds.Count()} <{threadIdsStr}>");

groups.ToList().ForEach(group => 
{
    var threadId = group.Key;
    var builder = new StringBuilder();
    var results = group.Select(value => value.Key);
    Console.WriteLine($"{threadId} handled {results.Count()} items");
});


class Result1
{
    public string Key {get; private set;}
    public int ThreadId {get; private set;}

    public Result1(string key, int threadId) 
    {
        this.Key = key;
        this.ThreadId = threadId;
    }
}