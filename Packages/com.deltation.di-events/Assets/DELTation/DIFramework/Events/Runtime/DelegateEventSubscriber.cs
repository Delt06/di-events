using System;
using JetBrains.Annotations;

namespace DELTation.DIFramework.Events
{
	internal sealed class DelegateEventSubscriber : EventSubscriber
	{
		private readonly Action _onEvent;

		public DelegateEventSubscriber([NotNull] Action onEvent) =>
			_onEvent = onEvent ?? throw new ArgumentNullException(nameof(onEvent));

		protected override void OnEventRaised() => _onEvent();
	}
}