using DELTation.DIFramework.Events;
using UnityEngine;

public class ControlsEventEmitter : MonoBehaviour
{
	// Access event bus through DI
	public void Construct(IEventBus eventBus)
	{
		_eventBus = eventBus;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			RaiseEvent.On(_eventBus).WithTag<SpacePressEvent>();

		for (var i = 0; i < 3; i++)
		{
			if (Input.GetMouseButtonDown(i))
				RaiseEvent.On(_eventBus).WithArguments(i).AndTag<LmbClickEvent>();
		}
	}

	private IEventBus _eventBus;
}