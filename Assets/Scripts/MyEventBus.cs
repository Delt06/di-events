using DELTation.DIFramework.Events;
using UnityEngine;

public class MyEventBus : ConfigurableEventBus
{
	protected override void Configure()
	{
		To<SpacePressEvent>()
			.Subscribe<SpacePressEventSubscriber>() // uses DI for creation
			;

		To<LmbClickEvent, int>()
			.Subscribe((in int button) => Debug.Log($"Clicked mouse button: {button}."))
			;
	}
}

public class SpacePressEventSubscriber : EventSubscriber<NoArgs>
{
	protected override void OnEventRaised(in NoArgs args)
	{
		Debug.Log("Pressed space.");
	}
}

public class SpacePressEvent : IEventTag { }

public class LmbClickEvent : IEventTag<int> { }