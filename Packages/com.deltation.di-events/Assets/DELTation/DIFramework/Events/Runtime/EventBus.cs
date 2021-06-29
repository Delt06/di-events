using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace DELTation.DIFramework.Events
{
	public sealed class EventBus : IEventBus
	{
		public IEvent<TArgs> GetEvent<TEventTag, TArgs>() where TEventTag : IEventTag<TArgs>
		{
			var eventType = typeof(TEventTag);
			var fallbackCreationProcedure = GetFallbackCreationProcedure<TArgs>();
			return (IEvent<TArgs>) GetEvent(eventType, fallbackCreationProcedure);
		}

		internal static EventCreationProcedure GetFallbackCreationProcedure<TArgs>() => Event<TArgs>.CreationProcedure;

		internal IEvent GetEvent([NotNull] Type eventType, [NotNull] EventCreationProcedure fallbackCreationProcedure)
		{
			if (eventType == null) throw new ArgumentNullException(nameof(eventType));
			if (fallbackCreationProcedure == null) throw new ArgumentNullException(nameof(fallbackCreationProcedure));

			if (!_events.TryGetValue(eventType, out var @event))
				_events[eventType] = @event = fallbackCreationProcedure();
			return (IEvent) @event;
		}

		private readonly IDictionary<Type, object> _events = new Dictionary<Type, object>();
	}
}