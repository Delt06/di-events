using System;
using JetBrains.Annotations;

namespace DELTation.DIFramework.Events
{
	public interface IEvent
	{
		void Raise();
		void Subscribe([NotNull] Action subscription);
		void Unsubscribe([NotNull] Action subscription);
	}
}