using System;

namespace DELTation.DIFramework.Events
{
	public abstract class EventSubscriber<TArgs> : TypelessEventSubscriber, IEventSubscriber<TArgs>
	{
		private readonly EventSubscriberAction<TArgs> _eventHandler;

		protected EventSubscriber() => _eventHandler = OnEventRaised;

		protected abstract void OnEventRaised(in TArgs args);

		public void SubscribeTo(IEvent<TArgs> @event)
		{
			if (@event == null) throw new ArgumentNullException(nameof(@event));
			@event.Subscribe(_eventHandler);
		}

		public void UnsubscribeFrom(IEvent<TArgs> @event)
		{
			if (@event == null) throw new ArgumentNullException(nameof(@event));
			@event.Unsubscribe(_eventHandler);
		}

		internal sealed override void SubscribeTo(IEvent @event)
		{
			if (@event == null) throw new ArgumentNullException(nameof(@event));
			if (!(@event is IEvent<TArgs> castedEvent)) throw new ArgumentException();
			SubscribeTo(castedEvent);
		}

		internal sealed override void UnsubscribeFrom(IEvent @event)
		{
			if (@event == null) throw new ArgumentNullException(nameof(@event));
			if (!(@event is IEvent<TArgs> castedEvent)) throw new ArgumentException();
			UnsubscribeFrom(castedEvent);
		}

		internal sealed override EventCreationProcedure GetEventCreationProcedure() =>
			EventBus.GetFallbackCreationProcedure<TArgs>();
	}
}