using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace DELTation.DIFramework.Events
{
	public abstract class ConfigurableEventBus : MonoBehaviour, IEventBus
	{
		public IEvent<TArgs> GetEvent<TEventTag, TArgs>() where TEventTag : IEventTag<TArgs> =>
			_eventBus.GetEvent<TEventTag, TArgs>();

		protected void OnEnable()
		{
			UnsubscribeAll();
			SubscribeAll();
			OnEnabled();
		}

		protected virtual void OnEnabled() { }

		protected void OnDisable()
		{
			UnsubscribeAll();
			OnDisabled();
		}

		protected virtual void OnDisabled() { }

		private void SubscribeAll()
		{
			EnsureConfigured();

			foreach (var (eventType, subscriber, eventCreationProcedure) in _subscribers)
			{
				var @event = _eventBus.GetEvent(eventType, eventCreationProcedure);
				subscriber.SubscribeTo(@event);
			}
		}

		private void UnsubscribeAll()
		{
			EnsureConfigured();

			foreach (var (eventType, subscriber, eventCreationProcedure) in _subscribers)
			{
				var @event = _eventBus.GetEvent(eventType, eventCreationProcedure);
				subscriber.UnsubscribeFrom(@event);
			}
		}

		private void EnsureConfigured()
		{
			if (_initialized) return;
			_initialized = true;

			Configure();
		}

		protected abstract void Configure();

		protected EventBuilder To<TEventTag, TArgs>() where TEventTag : IEventTag<TArgs>
		{
			var eventType = typeof(TEventTag);
			return new EventBuilder(this, eventType);
		}

		protected EventBuilder To<TEventTag>() where TEventTag : IEventTag
		{
			var eventType = typeof(TEventTag);
			return new EventBuilder(this, eventType);
		}

		private readonly
			List<(Type eventType, TypelessEventSubscriber subscriber, EventCreationProcedure eventCreationProcedure)>
			_subscribers =
				new List<(Type eventType, TypelessEventSubscriber subscriber, EventCreationProcedure
					eventCreationProcedure)>();
		private readonly EventBus _eventBus = new EventBus();
		private bool _initialized;

		public readonly struct EventBuilder
		{
			private readonly ConfigurableEventBus _eventBus;
			private readonly Type _eventType;

			internal EventBuilder(ConfigurableEventBus eventBus, Type eventType)
			{
				_eventBus = eventBus;
				_eventType = eventType;
			}

			public EventBuilder Subscribe<TArgs>([NotNull] EventSubscriber<TArgs> subscriber)
			{
				if (subscriber == null) throw new ArgumentNullException(nameof(subscriber));
				var fallbackCreationProcedure = EventBus.GetFallbackCreationProcedure<TArgs>();
				_eventBus._subscribers.Add((_eventType, subscriber, fallbackCreationProcedure));
				return this;
			}

			internal EventBuilder Subscribe([NotNull] TypelessEventSubscriber subscriber,
				[NotNull] EventCreationProcedure fallbackCreationProcedure)
			{
				if (subscriber == null) throw new ArgumentNullException(nameof(subscriber));
				_eventBus._subscribers.Add((_eventType, subscriber, fallbackCreationProcedure));
				return this;
			}

			public EventBuilder Subscribe<TArgs>([NotNull] EventSubscriberAction<TArgs> onEvent)
			{
				if (onEvent == null) throw new ArgumentNullException(nameof(onEvent));
				var eventSubscriber = new DelegateEventSubscriber<TArgs>(onEvent);
				var fallbackCreationProcedure = EventBus.GetFallbackCreationProcedure<TArgs>();
				_eventBus._subscribers.Add((_eventType, eventSubscriber, fallbackCreationProcedure));
				return this;
			}

			public EventBuilder Subscribe([NotNull] Action onEvent)
			{
				if (onEvent == null) throw new ArgumentNullException(nameof(onEvent));
				var eventSubscriber = new DelegateEventSubscriber<NoArgs>(delegate { onEvent(); });
				var fallbackCreationProcedure = EventBus.GetFallbackCreationProcedure<NoArgs>();
				_eventBus._subscribers.Add((_eventType, eventSubscriber, fallbackCreationProcedure));
				return this;
			}
		}
	}
}