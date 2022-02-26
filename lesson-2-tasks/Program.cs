// See https://aka.ms/new-console-template for more information
Console.WriteLine("Example for Task working\n");

runExampleForStatusesOfThread();
runExampleForStatusesOfTask();
runExampleForCancelableTask();

Thread.Sleep(1000);
Console.WriteLine("Program finished\n");


void runExampleForStatusesOfThread() {
    Console.WriteLine("method runExampleForStatusesOfThread started");
    var thread1 = createThread();
    // print status before start
    Console.WriteLine($"==> Thread1 state is {thread1.ThreadState}");
    thread1.Start();
    // print status after start
    Console.WriteLine($"==> Thread1 state is {thread1.ThreadState}");
    thread1.Join();
    // print status after complete
    Console.WriteLine($"==> Thread1 state is {thread1.ThreadState}");
    Console.WriteLine("method runExampleForStatusesOfThread ended\n\n");
}

void runExampleForStatusesOfTask() {
    Console.WriteLine("method runExampleForStatusesOfTask started");
    var task1 = createTask();
    // print status before start
    Console.WriteLine($"==> Task1 state is {task1.Status}");
    task1.Start();
    // print status after start
    Console.WriteLine($"==> Task1 state is {task1.Status}");
    task1.Wait();
    // print status after complete
    Console.WriteLine($"==> Task1 state is {task1.Status}");
    Console.WriteLine("method runExampleForStatusesOfTask ended\n\n");
}

void runExampleForCancelableTask() {
    Console.WriteLine("method runExampleForCancelableTask started");
    var cancellationTokenSource = new CancellationTokenSource();
    var action2 = () => {
            var thread = Thread.CurrentThread;
            var id = Task.CurrentId;
            var threadId = thread.ManagedThreadId;
            var label = $"Task {id} with thread {threadId}";
            Console.WriteLine($"{label} started");
            Thread.Sleep(100);
            Console.WriteLine($"{label} ended");
            return $"OK {label}";
        };
    var action3 = string (Task<string> task) => {
        if (task.IsCanceled) {
            Console.WriteLine($"Canceled...");
            return "canceled";
        }
        Console.WriteLine($"Continue after {task.Id} {task.Status}, {task.Result}");
        return "Continued";
    };

    var task2 = new Task<string>(action2, cancellationTokenSource.Token);
    var task3 = task2.ContinueWith<string>(action3, cancellationTokenSource.Token);

    // print status of Task2 before start
    Console.WriteLine($"==> Task2 state is <{task2.Status}> before start");
    // print status of Task3 before start
    Console.WriteLine($"==> Task3 state is <{task2.Status}> before start");
    task2.Start();

    // print status of Task2 after start
    Console.WriteLine($"==> Task2 state is <{task2.Status}> after start");
    // print status of Task3 after start
    Console.WriteLine($"==> Task3 state is <{task3.Status}> after start");

    Thread.Sleep(100);
    cancellationTokenSource.Cancel();

    // print status of Task2 after cancel
    Console.WriteLine($"==> Task2 state is <{task2.Status}> after cancel");
    // print status of Task3 after cancel
    Console.WriteLine($"==> Task3 state is <{task3.Status}> after cancel");

    Thread.Sleep(200);
    // print status of Task2 after cancel with delay 200
    Console.WriteLine($"==> Task2 state is <{task2.Status}> after cancel with delay 200");
    // print status of Task3 after cancel with delay 200
    Console.WriteLine($"==> Task3 state is <{task3.Status}> after cancel with delay 200");
    try {
        Console.WriteLine($"==> Task2 result is <{task2.Result}>");
        Console.WriteLine($"==> Task3 result is <{task3.Result}>");
    } catch (AggregateException e) { 
        Console.WriteLine($"==> Task error <{e.Message}>");
        Console.WriteLine($"==> Task2 error <{task2.Exception?.Message}>");
        Console.WriteLine($"==> Task3 error <{task3.Exception?.Message}>");
    }
    Console.WriteLine("method runExampleForCancelableTask ended\n\n");
}

Thread createThread() {
    return new Thread(() => {
        var thread = Thread.CurrentThread;
        var id = thread.ManagedThreadId;
        var label = $"Thread {id}";
        Console.WriteLine($"{label} started");
        Console.WriteLine($"{label} state is {thread.ThreadState}");
        Thread.Sleep(100);
        Console.WriteLine($"{label} state is {thread.ThreadState}");
        Thread.Sleep(100);
        Console.WriteLine($"{label} ended");
    });
}

Task createTask() {
    var action = () => {
        var thread = Thread.CurrentThread;
        var id = Task.CurrentId;
        var threadId = thread.ManagedThreadId;
        var label = $"Task {id} with thread {threadId}";
        Console.WriteLine($"{label} started");
        Console.WriteLine($"{label} state is {thread.ThreadState}");
        Thread.Sleep(100);
        Console.WriteLine($"{label} is {thread.ThreadState}");
        Thread.Sleep(100);
        Console.WriteLine($"{label} ended");
    };
    return new Task(action);    
}
