using System;
using JetBrains.Annotations;

namespace DELTation.DIFramework.Events
{
	public static class EventExtensions
	{
		public static void Raise([NotNull] this IEvent<NoArgs> @event)
		{
			if (@event == null) throw new ArgumentNullException(nameof(@event));
			@event.Raise(default);
		}
	}
}