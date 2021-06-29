namespace DELTation.DIFramework.Events
{
	public struct EventCancellationToken
	{
		internal bool IsCancelled { get; private set; }

		public void Cancel() => IsCancelled = true;
	}
}