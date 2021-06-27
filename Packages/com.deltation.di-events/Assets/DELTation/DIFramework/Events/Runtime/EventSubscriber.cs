using System;

namespace DELTation.DIFramework.Events
{
	public abstract class EventSubscriber : IEventSubscriber
	{
		private readonly Action _eventHandler;

		public EventSubscriber() => _eventHandler = OnEventRaised;

		protected abstract void OnEventRaised();

		public void SubscribeTo(IEvent @event)
		{
			if (@event == null) throw new ArgumentNullException(nameof(@event));
			@event.Subscribe(_eventHandler);
		}

		public void UnsubscribeFrom(IEvent @event)
		{
			if (@event == null) throw new ArgumentNullException(nameof(@event));
			@event.Unsubscribe(_eventHandler);
		}
	}
}