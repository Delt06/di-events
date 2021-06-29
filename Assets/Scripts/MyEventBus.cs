using DELTation.DIFramework.Events;
using UnityEngine;

public class MyEventBus : ConfigurableEventBus
{
	protected override void Configure()
	{
		To<SpacePressEvent>()
			.Subscribe<SpacePressEventSubscriber>() // uses DI for creation
			.Subscribe(() => enabled = false)
			;

		To<LmbClickEvent, int>()
			.Subscribe((in int button, ref EventCancellationToken token) =>
				Debug.Log($"Clicked mouse button: {button}.")
			)
			;
	}
}

public class SpacePressEventSubscriber : EventSubscriber<NoArgs>
{
	protected override void OnEventRaised(in NoArgs args, ref EventCancellationToken cancellationToken)
	{
		Debug.Log("Pressed space.");
	}
}

public class SpacePressEvent : IEventTag { }

public class LmbClickEvent : IEventTag<int> { }