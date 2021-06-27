using DELTation.DIFramework.Events;
using UnityEngine;

public class ControlsEventEmitter : MonoBehaviour
{
	public void Construct(IEventBus eventBus)
	{
		_eventBus = eventBus;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			_eventBus.Raise<SpacePressEvent>();

		if (Input.GetMouseButtonDown(0))
			_eventBus.Raise<LmbClickEvent>();
	}

	private IEventBus _eventBus;
}