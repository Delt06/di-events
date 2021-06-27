using System;

namespace DELTation.DIFramework.Events
{
	public sealed class Event : IEvent
	{
		private Action _subscriptions;

		public void Raise()
		{
			_subscriptions?.Invoke();
		}

		public void Subscribe(Action subscription)
		{
			if (subscription == null) throw new ArgumentNullException(nameof(subscription));
			_subscriptions = (Action) Delegate.Combine(_subscriptions, subscription);
		}

		public void Unsubscribe(Action subscription)
		{
			if (subscription == null) throw new ArgumentNullException(nameof(subscription));
			_subscriptions = (Action) Delegate.Remove(_subscriptions, subscription);
		}
	}
}