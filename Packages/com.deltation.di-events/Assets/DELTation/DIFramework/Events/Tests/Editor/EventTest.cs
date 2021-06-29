using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace DELTation.DIFramework.Events.Tests
{
	public class EventTest
	{
		[Test]
		public void GivenEventSubscription_WhenRaising_ThenInvokedOnce()
		{
			// Arrange
			var invokedTimes = 0;
			var @event = new Event<NoArgs>();
			@event.Subscribe(delegate { invokedTimes++; });

			// Act
			@event.Raise();

			// Assert
			Assert.That(invokedTimes, Is.EqualTo(1));
		}

		[Test, TestCase(1), TestCase(2), TestCase(5)]
		public void GivenEventSubscription_WhenRaisingWithArgs_ThenReceiveArgs(int passedArgs)
		{
			// Arrange
			var correctArgsInvokedTimes = 0;
			var @event = new Event<int>();
			@event.Subscribe((in int args, ref EventCancellationToken token) =>
				{
					if (args != passedArgs) return;
					correctArgsInvokedTimes++;
				}
			);

			// Act
			@event.Raise(passedArgs);

			// Assert
			Assert.That(correctArgsInvokedTimes, Is.EqualTo(1));
		}

		[Test, TestCase(2), TestCase(5), TestCase(13)]
		public void GivenEventSubscriptions_WhenRaising_ThenInvokedInCorrectOrder(int subscriptions)
		{
			// Arrange
			var invocations = new List<int>();
			var @event = new Event<NoArgs>();

			for (var subscriberIndex = 0; subscriberIndex < subscriptions; subscriberIndex++)
			{
				var i = subscriberIndex;
				@event.Subscribe(delegate { invocations.Add(i); });
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
			var @event = new Event<NoArgs>();
			void Subscription(in NoArgs args, ref EventCancellationToken token) => invokedTimes++;
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
			var @event = new Event<NoArgs>();

			for (var i = 0; i < repetitions; i++)
			{
				@event.Subscribe(delegate { invokedTimes++; });
			}

			// Act
			@event.Raise();

			// Assert
			Assert.That(invokedTimes, Is.EqualTo(repetitions));
		}

		[Test]
		public void GivenRaisingEvent_WhenFirstSubscriberRemovesSecond_ThenBothAreCalled()
		{
			// Arrange
			var firstInvokedTimes = 0;
			var secondInvokedTimes = 0;
			var @event = new Event<NoArgs>();

			void FirstSubscriber(in NoArgs args, ref EventCancellationToken token)
			{
				firstInvokedTimes++;
				@event.Unsubscribe(SecondSubscriber);
			}

			void SecondSubscriber(in NoArgs args, ref EventCancellationToken token) => secondInvokedTimes++;
			@event.Subscribe(FirstSubscriber);
			@event.Subscribe(SecondSubscriber);

			// Act
			@event.Raise();

			// Assert
			Assert.That(firstInvokedTimes, Is.EqualTo(1));
			Assert.That(secondInvokedTimes, Is.EqualTo(1));

			@event.Raise();
			Assert.That(firstInvokedTimes, Is.EqualTo(2));
			Assert.That(secondInvokedTimes, Is.EqualTo(1));
		}

		[Test]
		public void GivenRaisingEvent_WhenFirstSubscriberRemovesItself_ThenBothAreCalled()
		{
			// Arrange
			var firstInvokedTimes = 0;
			var secondInvokedTimes = 0;
			var @event = new Event<NoArgs>();

			void FirstSubscriber(in NoArgs args, ref EventCancellationToken token)
			{
				firstInvokedTimes++;
				@event.Unsubscribe(FirstSubscriber);
			}

			void SecondSubscriber(in NoArgs args, ref EventCancellationToken token) => secondInvokedTimes++;
			@event.Subscribe(FirstSubscriber);
			@event.Subscribe(SecondSubscriber);

			// Act
			@event.Raise();

			// Assert
			Assert.That(firstInvokedTimes, Is.EqualTo(1));
			Assert.That(secondInvokedTimes, Is.EqualTo(1));

			@event.Raise();
			Assert.That(firstInvokedTimes, Is.EqualTo(1));
			Assert.That(secondInvokedTimes, Is.EqualTo(2));
		}

		[Test]
		public void GivenRaisingEvent_WhenFirstSubscriberRemovesItselfAndRaisesEvent_ThenBothAreCalledTwice()
		{
			// Arrange
			var invocations = new List<int>();
			var @event = new Event<NoArgs>();

			void FirstSubscriber(in NoArgs args, ref EventCancellationToken token)
			{
				invocations.Add(0);
				@event.Unsubscribe(FirstSubscriber);
				@event.Raise();
			}

			void SecondSubscriber(in NoArgs args, ref EventCancellationToken token) => invocations.Add(1);
			@event.Subscribe(FirstSubscriber);
			@event.Subscribe(SecondSubscriber);

			// Act
			@event.Raise();

			// Assert
			Assert.That(invocations, Is.EqualTo(new[] { 0, 1, 1 }));
		}

		[Test]
		public void GivenRaisingEvent_WhenSecondSubscriberCancels_ThenNothingFurtherIsInvoked()
		{
			// Arrange
			var invocations = new List<int>();
			var @event = new Event<NoArgs>();

			@event.Subscribe((in NoArgs args, ref EventCancellationToken token) => invocations.Add(0));
			@event.Subscribe((in NoArgs args, ref EventCancellationToken token) =>
				{
					invocations.Add(1);
					token.Cancel();
				}
			);
			@event.Subscribe((in NoArgs args, ref EventCancellationToken token) => invocations.Add(2));

			// Act
			@event.Raise();

			// Assert
			Assert.That(invocations, Is.EqualTo(new[] { 0, 1 }));
		}
	}
}