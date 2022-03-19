// See https://aka.ms/new-console-template for more information

using System.Threading.Tasks.Dataflow;


Console.WriteLine("DataFlow example!");

runExampleForActionBlockWithSuccessfullTask();
runExampleForActionBlockWithFailureTask();
runExampleForActionBlockWithFailureTask();
runExampleForBufferBlock();

void runExampleForActionBlockWithSuccessfullTask()
{
    Console.WriteLine("runExampleForActionBlockWithSuccessfullTask started");

    // create action block
    var throwIfNegative = new ActionBlock<int>(number =>
    {
        Console.WriteLine("n = {0}", number);
        if (number < 0)
        {
            throw new ArgumentOutOfRangeException();
        }
    });

    // add action for execute after when action block will be completed
    throwIfNegative.Completion.ContinueWith(task =>
    {
        Console.WriteLine("The status of the completion task is '{0}'.",task.Status);
    });

    // post values into action block
    throwIfNegative.Post(1);
    throwIfNegative.Post(2);
    throwIfNegative.Post(3);
    throwIfNegative.Post(4);

    // run task for wait 1 second and mark action block as completed
    Task.Run(() => 
    {
        Thread.Sleep(1000);
        Console.WriteLine("Tast for complete executed");
        throwIfNegative.Complete();
    });

    try
    {
        Console.WriteLine("DataFlow. Waiting for complete");
        throwIfNegative.Completion.Wait();
        Console.WriteLine("DataFlow completed");
    }
    catch (AggregateException ae)
    {
        // AggregateException exception is wrapper for real exception
        ae.Handle(e =>
        {
            Console.WriteLine("Encountered {0}: {1}", e.GetType().Name, e.Message);
            return true;
        });
    }
    Console.WriteLine("runExampleForActionBlockWithSuccessfullTask finished");
}

void runExampleForActionBlockWithFailureTask()
{
    Console.WriteLine("runExampleForActionBlockWithFailureTask started");
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
    throwIfNegative.Post(-2); // post the value that emit exception
    throwIfNegative.Post(4); // this value can't be apply
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
    Console.WriteLine("runExampleForActionBlockWithFailureTask finished");
}

void runExampleForBufferBlock() 
{
    Console.WriteLine("runExampleForBufferBlock started");
    var options = new DataflowBlockOptions();
    options.BoundedCapacity = 2; // set the limit of the buffer
    var bufferBlock = new BufferBlock<int>(options);

    // run task for async receive values while bufferBlock not completed
    Task.Run(() => {
        Thread.Sleep(1000);
        while (!bufferBlock.Completion.IsCompleted)
        {
            var value = bufferBlock.Receive();
            Console.WriteLine($"Reaceive value <{value}> from buffer block");
        }
    });

    // run task for post values into bufferBlock
    Task.Run(() => {
        for (int i = 0; i < 3; i++)
        {
            Thread.Sleep(100);
            Console.WriteLine($"Post value {i} into buffer block");
            bufferBlock.Post(i);
        }
        Thread.Sleep(2000);
        bufferBlock.Complete(); // make bufferBlock as completed
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