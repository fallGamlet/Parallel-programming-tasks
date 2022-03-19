// See https://aka.ms/new-console-template for more information
using System.Text;

Console.WriteLine("Linq parallel example!");

var srcData = Enumerable.Range(0, 100).ToList();
var timeStart = DateTime.Now;

var dataStrList = srcData
.AsParallel()
.Select((number) => 
{
    Thread.Sleep(10); // Imitation of long job
    var threadId = Thread.CurrentThread.ManagedThreadId;
    return new Result1() 
    { 
        key = number.ToString(), 
        threadId = threadId 
    };
})
.ToList();

var timeEnd = DateTime.Now;
var duration = (timeEnd - timeStart);

Console.WriteLine($"Time duration: {duration}");
var groups = dataStrList.GroupBy(value => value.threadId);

var threadIds = groups.Select(value => value.Key);
var threadIdsStr = String.Join(", ", threadIds);
Console.WriteLine($"Threads count {threadIds.Count()} <{threadIdsStr}>");

groups.ToList().ForEach(group => 
{
    var threadId = group.Key;
    var builder = new StringBuilder();
    var results = group.Select(value => value.key);
    // var text = String.Join(", ", results);
    // Console.WriteLine($"{threadId} handle {results.Count()} items => <{text}>");
    Console.WriteLine($"{threadId} handled {results.Count()} items");
});


class Result1
{
    public string key {get; set;}
    public int threadId {get; set;}
}