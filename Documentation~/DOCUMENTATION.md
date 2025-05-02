# Overview

BasicUtilities includes basic `abstract` Singleton classes and a few Timer classes to help lower the time you spend on creating tools.

## How to use BasicUtilities

Within the namespace `BasicUtilities` holds two major concepts. The first is [**Singletons**](#singletons), the latter being [**Timers**](#timers).

### Singletons

> [!NOTE]
> All singletons are abstract classes meaning that your scripts should inherit from these classes in order for that class to be considered a "Singleton".

Every singleton has 2 parts that contributes to the role they play. 

- The first is `AutoUnparentOnAwake`. This is a boolean that can be set in the inspector. It defaults to `true` meaning that the script will take it's attached game object and decouple it from any parent game objects.
- The second is `Instance`. This will get access to the **non-static** properties and methods of the class. Be careful with checking if `Instance` is null as it automatically creates a new game object with that singleton if it cannot find any existing objects. Use `HasInstance` to check for if the singleton exists if you do not want it to automatically create a new one.

Past this point there are three subsets of Singletons that each have a unique implementation.
1. **Singleton:** This is the basic implementation and does nothing but ensure that only one instance of that class exists at any given point.
1. **Persistent Singleton:** This is almost identical to the basic singleton but also ensures that the existing instance doesn't destroy itself when it's parent scene is unloaded.
1. **Annex Singleton:** This is almost identical to the persistent singleton with one exception. Any new scene that harbors it's own instance of this class will override the existing singleton and overtake the role.

### Timers

> [!NOTE]
> All timers are base C# classes that do not inherit from Monobehaviours. You can only access them through script.

There are 2 basic timers `CountdownTimer` and `StopwatchTimer` that both inherit from a `TimerBase` abstract class.

#### Timer Base

Each timer inherits from this base and holds some standard implementation. First, the base has 4 events:
1. **On Start:** This is called when the timer starts.
1. **On Tick:** This is called every frame the timer is running and not paused.
1. **On Stop:** This is called when the timer halts.
1. **On Pause:** This is called when the timer pauses or resumes and can be checked which it is through the `IsPaused` boolean.

Any timer that inherits from this class also has a boolean `Realtime` to check if it is counting in "Realtime" or "Gametime."

#### The Countdown Timer

This timer starts with a duration and once it has been started will run until that duration has been reached and will end.

This timer is the most versitile of the timers in this package.

#### The Stopwatch Timer

This timer has a built in laps list that when started will start tracking the current `LapTime` and when the user `Lap()`s the timer will send that `LapTime` to a list of laps and zero it. As with all Timers. This timer has an `ElapsedTime` that will keep track of the total time for all laps.

#### One-Shot Timer

This static class holds two methods that are built to be *lightweight** helper methods. The methods take in a duration and will call a specified `Action` after said time. 