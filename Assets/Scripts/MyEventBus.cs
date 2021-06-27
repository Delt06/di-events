using DELTation.DIFramework.Events;
using UnityEngine;

public class MyEventBus : ConfigurableEventBus
{
	protected override void Configure()
	{
		To<SpacePressEvent>()
			.Subscribe<SpacePressEventSubscriber>() // uses DI for creation
			;

		To<LmbClickEvent>()
			.Subscribe(() => Debug.Log("Clicked left mouse button."))
			;
	}
}

public class SpacePressEventSubscriber : EventSubscriber
{
	protected override void OnEventRaised()
	{
		Debug.Log("Pressed space.");
	}
}

public class SpacePressEvent : IEventTag { }

public class LmbClickEvent : IEventTag { }