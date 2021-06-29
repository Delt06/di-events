using System;
using JetBrains.Annotations;

namespace DELTation.DIFramework.Events
{
	/*
	* RaiseEvent.On(bus).WithArguments(args).AndTag<TEventTag>();
	* RaiseEvent.On(bus).WithTag<TEventTag>();
	*/
	public static class RaiseEvent
	{
		[Pure]
		public static RaiseEventBuilder On([NotNull] IEventBus eventBus)
		{
			if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
			return new RaiseEventBuilder(eventBus);
		}

		public readonly struct RaiseEventBuilder
		{
			private readonly IEventBus _eventBus;

			public RaiseEventBuilder(IEventBus eventBus) => _eventBus = eventBus;

			public RaiseEventBuilder<TArgs> WithArguments<TArgs>(in TArgs args) =>
				new RaiseEventBuilder<TArgs>(_eventBus, args);

			public void WithTag<TEventTag>() where TEventTag : IEventTag<NoArgs>
			{
				if (_eventBus == null) throw new InvalidOperationException();
				_eventBus.GetEvent<TEventTag, NoArgs>().Raise();
			}
		}

		public readonly struct RaiseEventBuilder<TArgs>
		{
			private readonly IEventBus _eventBus;
			private readonly TArgs _args;

			public RaiseEventBuilder(IEventBus eventBus, TArgs args)
			{
				_eventBus = eventBus;
				_args = args;
			}

			public void AndTag<TEventTag>() where TEventTag : IEventTag<TArgs>
			{
				if (_eventBus == null) throw new InvalidOperationException();
				_eventBus.GetEvent<TEventTag, TArgs>().Raise(_args);
			}
		}
	}
}