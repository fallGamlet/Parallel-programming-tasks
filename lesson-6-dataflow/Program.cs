// See https://aka.ms/new-console-template for more information

using System.Threading.Tasks.Dataflow;


Console.WriteLine("DataFlow example!");

// runExampleForActionBlockWithSuccessfullTask();
// runExampleForActionBlockWithFailureTask();
// runExampleForActionBlockWithFailureTask();
runExampleForBufferBlock();

void runExampleForActionBlockWithSuccessfullTask()
{
    var throwIfNegative = new ActionBlock<int>(n =>
    {
        Console.WriteLine("n = {0}", n);
        if (n < 0)
        {
            throw new ArgumentOutOfRangeException();
        }
    });

    throwIfNegative.Completion.ContinueWith(task =>
    {
        Console.WriteLine("The status of the completion task is '{0}'.",task.Status);
    });

    throwIfNegative.Post(1);
    throwIfNegative.Post(2);
    throwIfNegative.Post(3);
    throwIfNegative.Post(4);

    new Task(() => 
    {
        Thread.Sleep(1000);
        Console.WriteLine("Tast for complete executed");
        throwIfNegative.Complete();
    }).Start();

    try
    {
        Console.WriteLine("DataFlow. Waiting for complete");
        throwIfNegative.Completion.Wait();
        Console.WriteLine("DataFlow completed");
    }
    catch (AggregateException ae)
    {
    ae.Handle(e =>
    {
        Console.WriteLine("Encountered {0}: {1}", e.GetType().Name, e.Message);
        return true;
    });
    }
}

void runExampleForActionBlockWithFailureTask()
{
    var throwIfNegative = new ActionBlock<int>(n =>
    {
        Console.WriteLine("n = {0}", n);
        if (n < 0)
        {
            throw new ArgumentOutOfRangeException();
        }
    });

    throwIfNegative.Completion.ContinueWith(task =>
    {
        Console.WriteLine("The status of the completion task is '{0}'.",task.Status);
    });

    throwIfNegative.Post(1);
    throwIfNegative.Post(2);
    throwIfNegative.Post(-2);
    throwIfNegative.Post(4);
    throwIfNegative.Complete();

    try
    {
        Console.WriteLine("DataFlow. Waiting for complete");
        throwIfNegative.Completion.Wait();
        Console.WriteLine("DataFlow completed");
    }
    catch (AggregateException ae)
    {
    ae.Handle(e =>
    {
        Console.WriteLine("Encountered {0}: {1}", e.GetType().Name, e.Message);
        return true;
    });
    }
}

void runExampleForBufferBlock() 
{
    Console.WriteLine("runExampleForBufferBlock started");
    var options = new DataflowBlockOptions();
    options.BoundedCapacity = 2;
    var bufferBlock = new BufferBlock<int>(options);

    Task.Run(() => {
        Thread.Sleep(1000);
        while (!bufferBlock.Completion.IsCompleted)
        {
            var value = bufferBlock.Receive();
            Console.WriteLine($"Reaceive value <{value}> from buffer block");
        }
    });

    Task.Run(() => {
        for (int i = 0; i < 3; i++)
        {
            Thread.Sleep(100);
            Console.WriteLine($"Post value {i} into buffer block");
            bufferBlock.Post(i);
        }
        Thread.Sleep(2000);
        bufferBlock.Complete();
    });
    
    try {
        bufferBlock.Completion.Wait();
    } catch (AggregateException ae)
    {
        ae.Handle(e =>
        {
            Console.WriteLine("Encountered {0}: {1}", e.GetType().Name, e.Message);
            return true;
        });
    }

    Console.WriteLine("runExampleForBufferBlock finished");
}

Console.WriteLine("DataFlow example finished...!");