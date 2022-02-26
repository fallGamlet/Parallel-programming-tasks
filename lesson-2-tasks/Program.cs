// See https://aka.ms/new-console-template for more information
Console.WriteLine("Example for Task working");

runExampleForStatusesOfThread();
runExampleForStatusesOfTask();
runExampleForCancelableTask();

Thread.Sleep(1000);
Console.WriteLine("Program finished");


void runExampleForStatusesOfThread() {
    var thread1 = createThread();
    Console.WriteLine($"==> Thread1 state is {thread1.ThreadState}");
    thread1.Start();
    Console.WriteLine($"==> Thread1 state is {thread1.ThreadState}");
    thread1.Join();
    Console.WriteLine($"==> Thread1 state is {thread1.ThreadState}");
}

void runExampleForStatusesOfTask() {
    var task1 = createTask();
    Console.WriteLine($"==> Task1 state is {task1.Status}");
    task1.Start();
    Console.WriteLine($"==> Task1 state is {task1.Status}");
    task1.Wait();
    Console.WriteLine($"==> Task1 state is {task1.Status}");
}

void runExampleForCancelableTask() {
    var cancellationTokenSource = new CancellationTokenSource();
    var task2 = createTask2(cancellationTokenSource.Token);
    Console.WriteLine($"==> Task2 state is {task2.Status}");
    task2.Start();

    var task3 = task2
    .ContinueWith<string>((task) => {
        if (task.IsCanceled) {
            Console.WriteLine($"Canceled...");
            return "canceled";
        }
        Console.WriteLine($"Continue after {task.Id} {task.Status}, {task.Result}");
        return "Continued";
    }, cancellationTokenSource.Token);

    Thread.Sleep(200);
    cancellationTokenSource.Cancel();
    Console.WriteLine($"==> Task2 state is {task2.Status}");
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

Task<string> createTask2(CancellationToken cancellationToken) {
    var action1 = () => {
        var thread = Thread.CurrentThread;
        var id = Task.CurrentId;
        var threadId = thread.ManagedThreadId;
        var label = $"Task {id} with thread {threadId}";
        Console.WriteLine($"{label} started");
        Thread.Sleep(100);
        Console.WriteLine($"{label} ended");
        return $"OK {label}";
    };

    return new Task<string>(action1, cancellationToken);
}

