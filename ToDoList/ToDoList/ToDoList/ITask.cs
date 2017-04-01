using System;

namespace ToDoList
{
	public interface ITask
    {
		int Id { get; }

		string Name { get; }

		string Description { get; }

		int Priority { get; }

		DateTime DueDate { get; }

		bool IsCompleted { get; }

        DateTime? CompletedAt { get; }
	}
}