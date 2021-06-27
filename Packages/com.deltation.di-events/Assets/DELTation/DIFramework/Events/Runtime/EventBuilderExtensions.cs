namespace DELTation.DIFramework.Events
{
	public static class EventBuilderExtensions
	{
		public static ConfigurableEventBus.EventBuilder Subscribe<TEventSubscriber>(
			in this ConfigurableEventBus.EventBuilder eventBuilder) where TEventSubscriber : class, IEventSubscriber
		{
			var eventSubscriber = Di.Create<TEventSubscriber>();
			return eventBuilder.Subscribe(eventSubscriber);
		}
	}
}