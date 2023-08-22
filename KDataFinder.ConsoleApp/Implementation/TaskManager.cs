using KDataFinder.ConsoleApp.Abstraction;

namespace KDataFinder.ConsoleApp.Implementation;

internal class TaskManager
{
    public int MaximumConcurrentTasks { get; set; }
    public Task[] ConcurrentTasks { get; }

    public TaskManager(int maximumConcurrentTasks)
    {
        MaximumConcurrentTasks = maximumConcurrentTasks;
        ConcurrentTasks = new Task[maximumConcurrentTasks];
    }

    public Task AddTask(Task task)
    {
        if (ConcurrentTasks.Count(x => x == null) > 0)
        {
            ConcurrentTasks[Array.IndexOf(ConcurrentTasks, null)] = (task);
        }
        else
        {
            var completedTaskIndex = Task.WaitAny(ConcurrentTasks);
            ConcurrentTasks[completedTaskIndex] = task;
        }
        return Task.CompletedTask;
    }

}
