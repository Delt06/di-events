using JetBrains.Annotations;

namespace DELTation.DIFramework.Events
{
	public interface IEventSubscriber
	{
		void SubscribeTo([NotNull] IEvent @event);
		void UnsubscribeFrom([NotNull] IEvent @event);
	}
}