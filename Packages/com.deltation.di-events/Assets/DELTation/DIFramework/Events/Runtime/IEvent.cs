using JetBrains.Annotations;

namespace DELTation.DIFramework.Events
{
	public interface IEvent { }

	public interface IEvent<TArgs> : IEvent
	{
		void Raise(in TArgs args);
		void Subscribe([NotNull] EventSubscriberAction<TArgs> subscription);
		void Unsubscribe([NotNull] EventSubscriberAction<TArgs> subscription);
	}
}