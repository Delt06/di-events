using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace DELTation.DIFramework.Events.Tests
{
	public class EventTest
	{
		[Test]
		public void GivenEventSubscription_WhenRaising_ThenInvokedOnce()
		{
			// Arrange
			var invokedTimes = 0;
			var @event = new Event();
			@event.Subscribe(() => invokedTimes++);

			// Act
			@event.Raise();

			// Assert
			Assert.That(invokedTimes, Is.EqualTo(1));
		}

		[Test, TestCase(2), TestCase(5), TestCase(13)]
		public void GivenEventSubscriptions_WhenRaising_ThenInvokedInCorrectOrder(int subscriptions)
		{
			// Arrange
			var invocations = new List<int>();
			var @event = new Event();

			for (var subscriberIndex = 0; subscriberIndex < subscriptions; subscriberIndex++)
			{
				var i = subscriberIndex;
				@event.Subscribe(() => invocations.Add(i));
			}

			// Act
			@event.Raise();

			// Assert
			var expectedInvocations = Enumerable.Range(0, subscriptions);
			Assert.That(invocations, Is.EqualTo(expectedInvocations));
		}

		[Test]
		public void GivenEventSubscription_WhenUnsubscribingAndRaising_ThenNotInvoked()
		{
			// Arrange
			var invokedTimes = 0;
			var @event = new Event();
			void Subscription() => invokedTimes++;
			@event.Subscribe(Subscription);

			// Act
			@event.Unsubscribe(Subscription);
			@event.Raise();

			// Assert
			Assert.That(invokedTimes, Is.EqualTo(0));
		}

		[Test, TestCase(2), TestCase(5), TestCase(13)]
		public void GivenEventSimilarSubscriptions_WhenRaising_ThenInvokedForAllSubscriptions(int repetitions)
		{
			// Arrange
			var invokedTimes = 0;
			var @event = new Event();

			for (var i = 0; i < repetitions; i++)
			{
				@event.Subscribe(() => invokedTimes++);
			}

			// Act
			@event.Raise();

			// Assert
			Assert.That(invokedTimes, Is.EqualTo(repetitions));
		}
	}
}