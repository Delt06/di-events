using JetBrains.Annotations;

namespace DELTation.DIFramework.Events
{
	public interface IEventSubscriber<TArgs>
	{
		void SubscribeTo([NotNull] IEvent<TArgs> @event);
		void UnsubscribeFrom([NotNull] IEvent<TArgs> @event);
	}
}