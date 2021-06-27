using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace DELTation.DIFramework.Events
{
	public static class EventBusExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Raise<TEventTag>([NotNull] this IEventBus eventBus) where TEventTag : IEventTag
		{
			if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
			eventBus.GetEvent<TEventTag>().Raise();
		}
	}
}