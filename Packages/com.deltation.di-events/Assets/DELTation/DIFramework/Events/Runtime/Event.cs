using System;

namespace DELTation.DIFramework.Events
{
	public static class Event { }

	public sealed class Event<TArgs> : IEvent<TArgs>
	{
		private EventSubscriberAction<TArgs> _subscriptions;

		public void Raise(in TArgs args)
		{
			_subscriptions?.Invoke(args);
		}

		public void Subscribe(EventSubscriberAction<TArgs> subscription)
		{
			if (subscription == null) throw new ArgumentNullException(nameof(subscription));
			_subscriptions = (EventSubscriberAction<TArgs>) Delegate.Combine(_subscriptions, subscription);
		}

		public void Unsubscribe(EventSubscriberAction<TArgs> subscription)
		{
			if (subscription == null) throw new ArgumentNullException(nameof(subscription));
			_subscriptions = (EventSubscriberAction<TArgs>) Delegate.Remove(_subscriptions, subscription);
		}

		internal static readonly EventCreationProcedure CreationProcedure = () => new Event<TArgs>();
	}
}