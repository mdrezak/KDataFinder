using KDataFinder.ConsoleApp.Abstraction;
using System.Threading;
using System.Threading.Tasks;

namespace KDataFinder.ConsoleApp.Implementation;

internal class TaskManager<TRes>
{
    public int MaximumConcurrentTasks { get; set; }
    public Task<TRes>[] ConcurrentTasks { get; }
    public Action<(TRes res, int index)> OnTaskCompleted { get; }
    //private static SemaphoreSlim _semaphore;

    public TaskManager(int maximumConcurrentTasks, Action<(TRes res, int index)> onTaskCompleted = null)
    {
        MaximumConcurrentTasks = maximumConcurrentTasks;
        ConcurrentTasks = new Task<TRes>[maximumConcurrentTasks];
        //_semaphore = new SemaphoreSlim(maximumConcurrentTasks);
        OnTaskCompleted = onTaskCompleted;
    }

    public Task AddTask(Func<Task<TRes>> func)
        => AddTask(Task.Run(async () => await func()));
    
    public Task AddTask<TArg1>(Func<TArg1,Task<TRes>> func, TArg1 arg1)
        => AddTask(Task.Run(async () => await func(arg1)));
    
    public Task AddTask<TArg1,TArg2>(Func<TArg1,TArg2,Task<TRes>> func, TArg1 arg1, TArg2 arg2)
        => AddTask(Task.Run(async () => await func(arg1,arg2)));
    
    private Task AddTask(Task<TRes> task)
    {
        if (ConcurrentTasks.Count(x => x == null) > 0)
            ConcurrentTasks[Array.IndexOf(ConcurrentTasks, null)] = (task);
        else
        {
            var completedTaskIndex = Task.WaitAny(ConcurrentTasks);
            if (OnTaskCompleted != null)
                OnTaskCompleted(
                    (ConcurrentTasks[completedTaskIndex].Result, completedTaskIndex)
                );
            ConcurrentTasks[completedTaskIndex] = task;
        }
        return Task.CompletedTask;
    }

}
