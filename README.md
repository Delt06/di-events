# DI Events

#  Unity DI Framework

[![Version](https://img.shields.io/github/v/release/Delt06/di-events?sort=semver)](https://github.com/Delt06/di-events/releases)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)


An event system for [DI Framework](https://github.com/Delt06/di-framework).

> Developed and tested with Unity 2020.3.0f1 LTS

## Installation
### Option 1
- Open Package Manager through Window/Package Manager
- Click "+" and choose "Add package from git URL..."
- Insert the URL: https://github.com/Delt06/di-events.git?path=Packages/com.deltation.di-events

### Option 2
Add the following line to `Packages/manifest.json`:
```
"com.deltation.di-events": "https://github.com/Delt06/di-events.git?path=Packages/com.deltation.di-events",
```

## Example
Explanation assumes you are familiar with basics of [DI Framework](https://github.com/Delt06/di-framework).

- `MyEventBus.cs`:
```c#
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
```

- `ControlsEventEmitter.cs`:
```c#
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
			_eventBus.Raise<SpacePressEvent>();

		if (Input.GetMouseButtonDown(0))
			_eventBus.Raise<LmbClickEvent>();
	}

	private IEventBus _eventBus;
}
```