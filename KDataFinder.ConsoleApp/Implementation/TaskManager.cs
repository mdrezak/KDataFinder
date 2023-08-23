using KDataFinder.ConsoleApp.Abstraction;

namespace KDataFinder.ConsoleApp.Implementation;

internal class TaskManager<TTask> where TTask : Task
{
    public int MaximumConcurrentTasks { get; set; }
    public TTask[] ConcurrentTasks { get; }
    public Action<(TTask task, int index)> OnTaskCompleted { get; }

    public TaskManager(int maximumConcurrentTasks, Action<(TTask task, int index)> onTaskCompleted = null)
    {
        MaximumConcurrentTasks = maximumConcurrentTasks;
        ConcurrentTasks = new TTask[maximumConcurrentTasks];
        OnTaskCompleted = onTaskCompleted;
    }

    public Task AddTask(TTask task)
    {
        if (ConcurrentTasks.Count(x => x == null) > 0)
        {
            ConcurrentTasks[Array.IndexOf(ConcurrentTasks, null)] = (task);
        }
        else
        {
            var completedTaskIndex = Task.WaitAny(ConcurrentTasks);
            if (OnTaskCompleted != null) 
                OnTaskCompleted(
                    (ConcurrentTasks[completedTaskIndex], completedTaskIndex)
                );
            ConcurrentTasks[completedTaskIndex] = task;
        }
        return Task.CompletedTask;
    }

}
