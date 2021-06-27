using DELTation.DIFramework.Events;
using UnityEngine;

public class MyEventBus : ConfigurableEventBus
{
	protected override void Configure()
	{
		To<SpacePressEvent>()
			.Subscribe(() => Debug.Log("Pressed space."))
			;

		To<LmbClickEvent>()
			.Subscribe(() => Debug.Log("Clicked left mouse button."))
			;
	}
}

public class SpacePressEvent : IEventTag { }

public class LmbClickEvent : IEventTag { }