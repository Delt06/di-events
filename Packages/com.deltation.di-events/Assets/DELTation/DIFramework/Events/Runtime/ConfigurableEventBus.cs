using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace DELTation.DIFramework.Events
{
	public abstract class ConfigurableEventBus : MonoBehaviour, IEventBus
	{
		public IEvent GetEvent<TEventTag>() where TEventTag : IEventTag => _eventBus.GetEvent<TEventTag>();

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

			foreach (var (eventType, subscriber) in _subscribers)
			{
				var @event = _eventBus.GetEvent(eventType);
				subscriber.SubscribeTo(@event);
			}
		}

		private void UnsubscribeAll()
		{
			EnsureConfigured();

			foreach (var (eventType, subscriber) in _subscribers)
			{
				var @event = _eventBus.GetEvent(eventType);
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

		protected EventBuilder To<TEventTag>() where TEventTag : IEventTag
		{
			var eventType = typeof(TEventTag);
			return new EventBuilder(this, eventType);
		}

		private readonly List<(Type eventType, IEventSubscriber subscriber)> _subscribers =
			new List<(Type eventType, IEventSubscriber subscriber)>();
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

			public EventBuilder Subscribe([NotNull] IEventSubscriber subscriber)
			{
				if (subscriber == null) throw new ArgumentNullException(nameof(subscriber));
				_eventBus._subscribers.Add((_eventType, subscriber));
				return this;
			}

			public EventBuilder Subscribe([NotNull] Action onEvent)
			{
				if (onEvent == null) throw new ArgumentNullException(nameof(onEvent));
				var eventSubscriber = new DelegateEventSubscriber(onEvent);
				_eventBus._subscribers.Add((_eventType, eventSubscriber));
				return this;
			}
		}
	}
}