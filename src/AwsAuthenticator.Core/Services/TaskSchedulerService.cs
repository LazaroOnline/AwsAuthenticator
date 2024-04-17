using Microsoft.Win32.TaskScheduler;

namespace AwsAuthenticator.Core.Services;

public interface ITaskSchedulerService {
	public void AddDailyTask();
}

public class TaskSchedulerService : ITaskSchedulerService
{
	public void AddDailyTask()
	{
		// Running twice is idempotent, it overrides any task that has the same task name.
		// https://stackoverflow.com/questions/7394806/creating-scheduled-tasks
		// https://github.com/dahall/taskscheduler
		using TaskService taskService = new();

		TaskDefinition task = taskService.NewTask();
		task.RegistrationInfo.Description = "AwsAuthenticator-AutoTask";

		task.Triggers.Add(new DailyTrigger {
			DaysInterval = 1,
			StartBoundary = new DateTime(2021, 1, 1, 8, 30, 0),
			ExecutionTimeLimit = TimeSpan.FromMinutes(2),
		});

		task.Triggers.Add(new DailyTrigger {
			DaysInterval = 1,
			StartBoundary = new DateTime(2021, 1, 1, 20, 30, 0),
			ExecutionTimeLimit = TimeSpan.FromMinutes(2),
		});

		var appPath = CurrentApp.GetLocation();
		task.Actions.Add(new ExecAction(appPath, "-UpdateCreds", null));
		task.Settings.AllowDemandStart = true;
		task.Settings.StartWhenAvailable = true;

		var taskName = "AwsAuthenticator-AutoTask";
		taskService.RootFolder.RegisterTaskDefinition(taskName, task);
		//ts.RootFolder.DeleteTask(taskName);
	}
}
