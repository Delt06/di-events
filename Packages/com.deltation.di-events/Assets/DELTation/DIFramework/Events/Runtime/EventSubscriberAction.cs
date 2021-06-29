namespace DELTation.DIFramework.Events
{
	public delegate void EventSubscriberAction<TArgs>(in TArgs args, ref EventCancellationToken cancellationToken);
}