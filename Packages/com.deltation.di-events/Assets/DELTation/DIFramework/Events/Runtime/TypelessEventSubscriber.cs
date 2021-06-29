using JetBrains.Annotations;

namespace DELTation.DIFramework.Events
{
	public abstract class TypelessEventSubscriber
	{
		internal abstract void SubscribeTo([NotNull] IEvent @event);
		internal abstract void UnsubscribeFrom([NotNull] IEvent @event);
		internal abstract EventCreationProcedure GetEventCreationProcedure();

		internal TypelessEventSubscriber() { }
	}
}