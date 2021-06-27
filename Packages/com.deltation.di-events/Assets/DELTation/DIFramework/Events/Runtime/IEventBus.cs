namespace DELTation.DIFramework.Events
{
	public interface IEventBus
	{
		IEvent GetEvent<TEventTag>() where TEventTag : IEventTag;
	}
}