namespace DELTation.DIFramework.Events
{
	public static class EventBuilderExtensions
	{
		public static ConfigurableEventBus.EventBuilder Subscribe<TEventSubscriber>(
			in this ConfigurableEventBus.EventBuilder eventBuilder) where TEventSubscriber : TypelessEventSubscriber
		{
			var eventSubscriber = Di.Create<TEventSubscriber>();
			var fallbackCreationProcedure = eventSubscriber.GetEventCreationProcedure();
			return eventBuilder.Subscribe(eventSubscriber, fallbackCreationProcedure);
		}
	}
}