using MyThreadPool;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace ThreadPool;


public class MyThreadPool
{
    private readonly ConcurrentQueue<Action> taskQueue;
    private readonly Thread[] threads;
    private readonly CancellationTokenSource cts;
    private readonly object lockObject;
    private volatile bool isShutdown = true;

    public MyThreadPool(int countThreads)
    {
        if (countThreads < 1)
        {
            throw new ArgumentException("The thread pool must have at least 1 thread");
        }

        isShutdown = false;
        taskQueue = new();
        threads = new Thread[countThreads];
        cts = new();
        lockObject = new();

        for (var i = 0; i < countThreads; ++i)
        {
            threads[i] = new Thread(Work);
            threads[i].IsBackground = true;
            threads[i].Start();
        }
    }

    private void Work()
    {
        while (!cts.Token.IsCancellationRequested)
        {
            Action? action = null;
            lock (lockObject)
            {
                while (!taskQueue.TryDequeue(out action) && !cts.Token.IsCancellationRequested)
                {
                    Monitor.Wait(lockObject);
                }
            }
            action?.Invoke();
        }
    }

    public void Shutdown()
    {
        lock (lockObject)
        {
            cts.Cancel();
            Monitor.PulseAll(lockObject);
            isShutdown = true;
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }
    }

    public IMyTask<TResult> Submit<TResult>(Func<TResult> function)
    {
        cts.Token.ThrowIfCancellationRequested();

        lock (lockObject)
        {
            cts.Token.ThrowIfCancellationRequested();
            var myTask = new MyTask<TResult>(function, this);
            taskQueue.Enqueue(myTask.Complete);
            Monitor.Pulse(lockObject);
            return myTask;
        }
    }

    private void SubmitContinueWuth(Action action)
    {
        cts.Token.ThrowIfCancellationRequested();

        lock (lockObject)
        {
            cts.Token.ThrowIfCancellationRequested();

            taskQueue.Enqueue(action);
            Monitor.Pulse(lockObject);
        }
    }

    private class MyTask<TResult> : IMyTask<TResult>
    {
        private readonly Func<TResult> function;
        private readonly MyThreadPool threadPool;
        private readonly ConcurrentQueue<Action> continueWithTasks;
        private TResult? result;
        private volatile bool isCompleted = false;
        private Exception? exception;
        private readonly object lockTaskObject;

        public MyTask(Func<TResult> function, MyThreadPool thredPool)
        {
            this.function = function;
            this.threadPool = thredPool;
            continueWithTasks = new();
            lockTaskObject = new();
        }

        public TResult? Result
        {
            get
            {
                lock (lockTaskObject)
                {
                    while (!isCompleted)
                    {
                        Monitor.Wait(lockTaskObject);
                    }

                    if (exception is not null)
                    {
                        throw new AggregateException(exception);
                    }

                    return result;
                }
            }
        }

        public bool IsCompleted
        {
            get { return isCompleted; }
        }

        public void Complete()
        {
            try
            {
                result = function();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                lock (lockTaskObject)
                {
                    isCompleted = true;
                    Monitor.Pulse(lockTaskObject);
                    CompleteContinueWith();
                }
            }
        }

        public void CompleteContinueWith()
        {
            foreach (var continueTasks in continueWithTasks)
            {
                threadPool.taskQueue.Enqueue(continueTasks);
            }
        }

        public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult?, TNewResult> continueWithFunction)
        {
            if (threadPool.isShutdown)
            {
                throw new InvalidOperationException("The thread pool has already been turned off");
            }

            lock (lockTaskObject)
            {
                var continueWithTask = new MyTask<TNewResult>(() => continueWithFunction(Result), threadPool);

                if (isCompleted)
                {
                    threadPool.SubmitContinueWuth(continueWithTask.Complete);
                }
                else
                {
                    continueWithTasks.Enqueue(continueWithTask.Complete);
                }
                return continueWithTask;
            }
        }

    }
}
