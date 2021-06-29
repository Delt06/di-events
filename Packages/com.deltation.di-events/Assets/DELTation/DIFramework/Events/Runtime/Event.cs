using System;
using System.Collections.Generic;

namespace DELTation.DIFramework.Events
{
	public sealed class Event<TArgs> : IEvent<TArgs>
	{
		private LinkedList<EventSubscriberAction<TArgs>>
			_subscriptions = new LinkedList<EventSubscriberAction<TArgs>>();
		private bool _raisingWithSubscriptions;

		private int _raisingCounter;

		public void Raise(in TArgs args)
		{
			_raisingCounter++;

			var subscriptions = _subscriptions;
			_raisingWithSubscriptions = true;

			var enumerator = subscriptions.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					var subscription = enumerator.Current;

					var cancellationToken = new EventCancellationToken();
					subscription?.Invoke(args, ref cancellationToken);
					if (cancellationToken.IsCancelled)
						break;
				}
			}
			finally
			{
				enumerator.Dispose();
				_raisingCounter--;
				if (_raisingCounter == 0)
					_raisingWithSubscriptions = false;
			}
		}

		public void Subscribe(EventSubscriberAction<TArgs> subscription)
		{
			if (subscription == null) throw new ArgumentNullException(nameof(subscription));
			CopySubscriptionsIfRaising();
			_subscriptions.AddLast(subscription);
		}

		public void Unsubscribe(EventSubscriberAction<TArgs> subscription)
		{
			if (subscription == null) throw new ArgumentNullException(nameof(subscription));
			CopySubscriptionsIfRaising();
			_subscriptions.Remove(subscription);
		}

		private void CopySubscriptionsIfRaising()
		{
			if (_raisingCounter == 0) return;
			if (!_raisingWithSubscriptions) return;
			_subscriptions = Clone(_subscriptions);
			_raisingWithSubscriptions = false;
		}

		private static LinkedList<EventSubscriberAction<TArgs>> Clone(
			LinkedList<EventSubscriberAction<TArgs>> subscriptions)
		{
			var clone = new LinkedList<EventSubscriberAction<TArgs>>();

			foreach (var subscription in subscriptions)
			{
				clone.AddLast(subscription);
			}

			return clone;
		}

		internal static readonly EventCreationProcedure CreationProcedure = () => new Event<TArgs>();
	}
}