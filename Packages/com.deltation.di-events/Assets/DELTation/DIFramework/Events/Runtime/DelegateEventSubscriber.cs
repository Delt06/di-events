using System;
using JetBrains.Annotations;

namespace DELTation.DIFramework.Events
{
	internal sealed class DelegateEventSubscriber<TArgs> : EventSubscriber<TArgs>
	{
		private readonly EventSubscriberAction<TArgs> _onEvent;

		public DelegateEventSubscriber([NotNull] EventSubscriberAction<TArgs> onEvent) =>
			_onEvent = onEvent ?? throw new ArgumentNullException(nameof(onEvent));

		protected override void OnEventRaised(in TArgs args, ref EventCancellationToken cancellationToken) =>
			_onEvent(args, ref cancellationToken);
	}
}