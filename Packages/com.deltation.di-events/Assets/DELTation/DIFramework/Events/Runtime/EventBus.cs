using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace DELTation.DIFramework.Events
{
	public sealed class EventBus : IEventBus
	{
		public IEvent GetEvent<TEventTag>() where TEventTag : IEventTag
		{
			var eventType = typeof(TEventTag);
			return GetEvent(eventType);
		}

		internal IEvent GetEvent([NotNull] Type eventType)
		{
			if (eventType == null) throw new ArgumentNullException(nameof(eventType));
			if (!_events.TryGetValue(eventType, out var @event))
				_events[eventType] = @event = new Event();
			return @event;
		}

		private readonly IDictionary<Type, IEvent> _events = new Dictionary<Type, IEvent>();
	}
}