namespace DELTation.DIFramework.Events
{
	public interface IEventBus
	{
		IEvent<TArgs> GetEvent<TEventTag, TArgs>() where TEventTag : IEventTag<TArgs>;
	}
}