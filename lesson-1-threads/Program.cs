
Console.WriteLine("Hello, World!");

var arr = new List<Double>();
var random = new Random();

for (int i = 0; i<1000; i++) {
    arr.Add(random.NextDouble());
}


var start = DateTime.Now;
var result = makeSum(arr);
var end = DateTime.Now;
var duration = (end - start).TotalMilliseconds;
// Console.WriteLine($"Result: {result}, Time: {duration}");

var start1 = DateTime.Now;
var result1 = makeSum(arr);
var end1 = DateTime.Now;
var duration1 = (end1 - start1).TotalMilliseconds;
Console.WriteLine($"Result: {result1}, Time: {duration1}");


var start2 = DateTime.Now;
var result2 = makeSumWithThreads(arr);
var end2 = DateTime.Now;
var duration2 = (end2 - start2).TotalMilliseconds;
Console.WriteLine($"Result: {result2}, Time: {duration2}");



double makeSum(List<Double> arr) {
    return makeSumWithRange(arr, 0, arr.Count);
}

double makeSumWithThreads(List<Double> arr) {
    var sum1 = 0.0;
    var sum2 = 0.0;
    var sum3 = 0.0;
    var index1 = arr.Count / 3;
    var index2 = index1 * 2;

    var t1 = new Thread(() => {
        sum1 = makeSumWithRange(arr, 0, index1);
    });

    var t2 = new Thread(() => {
        sum2 = makeSumWithRange(arr, index1, index2);
    });

    var t3 = new Thread(() => {
        sum3 = makeSumWithRange(arr, index2, arr.Count);
    });

    t1.Start();
    t2.Start();
    t3.Start();
    
    t1.Join();
    t2.Join();
    t3.Join();

    return sum1+sum2+sum3;
}

double makeSumWithRange(List<Double> arr, int start, int end) {
    var sum = 0.0;
    for (var i=start; i< end; i++) {
        sum += arr[i];
        Thread.Sleep(1);
    }
    return sum;
}
