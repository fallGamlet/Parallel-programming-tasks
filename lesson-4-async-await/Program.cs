
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Examle for async/await!");

ExampleForThread();
ExampleForTask();
ExampleForAyncAwait();


void ExampleForThread() {
    Console.WriteLine($"Start ExampleForThread");
    var action = (object? arg, ResultWrapper<string> result) => {
        try {
            if (!(arg is int)) 
                throw new Exception("argument must be a positive int number");

            var count = (int) arg;
            result.Value = GenerateId(count);
            
        } catch(Exception e) {
            result.Error = e;
        }
    };

    var resultList = new List<String>();
    var result1 = new ResultWrapper<string>();
    var thread1 = new Thread(arg => action(arg, result1));
    thread1.Start(1);

    var result2 = new ResultWrapper<string>();
    var thread2 = new Thread(arg => action(arg, result2));
    thread2.Start("3");

    var result3 = new ResultWrapper<string>();
    var thread3 = new Thread(arg => action(arg, result3));
    thread3.Start(7);

    thread1.Join();
    thread2.Join();
    thread3.Join();

    resultList.Add(result1.Value);
    resultList.Add(result2.Value);
    resultList.Add(result3.Value);
    
    var err = result1.Error ?? result2.Error ?? result3.Error;
    if (err != null) {
        Console.WriteLine($"Thread {thread1.ManagedThreadId} finish with error <{err.Message}>");
    }
    resultList.ForEach(value => {
        Console.WriteLine($"Result: {value}");
    });

    Console.WriteLine($"End of ExampleForThread\n\n");
}

void ExampleForTask() {
    Console.WriteLine($"Start ExampleForTask");
    var resultList = new List<string>();

    var task = GetTaskOfGenerateId(1);

    var lastTasks = task.ContinueWith<string>(task => {
            resultList.Add(task.Result);
            throw new Exception("My test exeption for ExampleForTask");
            return GenerateId(3);
        })
        .ContinueWith<string>(task => {
            resultList.Add(task.Result);
            return GenerateId(7);
        })
        .ContinueWith<string>(task => {
            resultList.Add(task.Result);
            return "";
        });

    task.Start();

    try {
        lastTasks.Wait();
    } catch(Exception e) {
        Console.WriteLine($"Thread {task.Id} finish with error <{e.Message}>");
    }

    resultList.ForEach(value => {
        Console.WriteLine($"Result: {value}");
    });

    Console.WriteLine($"Ended of ExampleForTask\n\n");
}

void ExampleForAyncAwait() {
    Console.WriteLine($"Start ExampleForAyncAwait");
    var resultList = new List<string>();
    try {
        var task = Task.Run(async () => {
            var result1 = await GenerateIdAsync(1);
            resultList.Add(result1);
            var result2 = await GenerateIdAsync(-1);
            resultList.Add(result2);
            var result3 = await GenerateIdAsync(7);
            resultList.Add(result3);
        });
        task.Wait();
    } catch(Exception e) {
        Console.WriteLine($"error <{e.Message}>");
    }

    resultList.ForEach(value => {
        Console.WriteLine($"Result: {value}");
    });
    Console.WriteLine($"Ended of ExampleForAyncAwait\n\n");
}


Task<string> GetTaskOfGenerateId(int length) {
    var action = string (object? arg) => {
        if (!(arg is int)) 
            throw new Exception("argument must be a positive int number");

        var count = (int) arg;
        return GenerateId(count);
    };
    return new Task<string>(action, length);
} 

async Task<string> GenerateIdAsync(int length) {
    return GenerateId(length);
}

string GenerateId(int length) {
    var builder = new System.Text.StringBuilder(length);

    var random = new Random();
    var buffer = new byte[2];
    for (int i=0; i < length; i++) {
        random.NextBytes(buffer);
        builder.Append(Convert.ToHexString(buffer));
    }
    Thread.Sleep(100);

    return builder.ToString();
}

class ResultWrapper<T>
{
    public T Value {get; set; }
    public Exception Error {get; set; }

}