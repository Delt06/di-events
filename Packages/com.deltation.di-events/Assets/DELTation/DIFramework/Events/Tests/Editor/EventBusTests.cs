using System.Linq;
using NUnit.Framework;

namespace DELTation.DIFramework.Events.Tests
{
	public class EventBusTests
	{
		private class SomeEventTag : IEventTag { }

		private class OtherEventTag : IEventTag { }

		private IEventBus _eventBus;

		[SetUp]
		public void SetUp()
		{
			_eventBus = new EventBus();
		}

		[Test]
		public void GivenEventBus_WhenGettingEvent_ThenItIsNotNull()
		{
			// Arrange

			// Act
			var @event = _eventBus.GetEvent<SomeEventTag>();

			// Assert
			Assert.That(@event, Is.Not.Null);
		}

		[Test, TestCase(2), TestCase(5), TestCase(13)]
		public void GivenEventBus_WhenGettingEventSeveralTimesBySameTag_ThenItIsAlwaysTheSameEvent(int repetitions)
		{
			// Arrange

			// Act
			var events = Enumerable.Range(0, repetitions)
				.Select(i => _eventBus.GetEvent<SomeEventTag>())
				.ToArray();

			// Assert
			Assert.That(events.Distinct().Count(), Is.EqualTo(1));
		}

		[Test]
		public void GivenEventBus_WhenGettingEventsByDifferentTagsSeveralTimesBySameTag_ThenEventsAreDifferent()
		{
			// Arrange

			// Act
			var someEvent = _eventBus.GetEvent<SomeEventTag>();
			var otherEvent = _eventBus.GetEvent<OtherEventTag>();

			// Assert
			Assert.That(someEvent, Is.Not.EqualTo(otherEvent));
		}
	}
}