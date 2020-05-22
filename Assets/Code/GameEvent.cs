using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

// Base interface for all game events.
public interface IGameEvent { }

// Main API class for game events.
public class GameEvent<T> where T : IGameEvent, new()
{
	public delegate void Callback(T data);

	// Internal table that maps game event types to callbacks.
	private static Dictionary<System.Type, List<Callback>> CallbackTable = new Dictionary<System.Type, List<Callback>>();

	private static Dictionary<System.Type, Dictionary<GameObject, List<Callback>>> TargetedCallbackTable = new Dictionary<System.Type, Dictionary<GameObject, List<Callback>>>();

	~GameEvent()
	{
		Assert.AreEqual(0, CallbackTable[typeof(T)].Count, string.Format("There are still event listeners for {0} at shutdown.", typeof(T).Name));
	}

	/// <summary>
	/// Registers a callback for whenever a game event of type T is posted.
	/// </summary>
	/// <param name="cb">Callback to register</param>
	public static void Register(Callback cb)
	{
		// First time this event type is registered?
		if (!CallbackTable.ContainsKey(typeof(T)))
		{
			CallbackTable[typeof(T)] = new List<Callback>();
		}

		// Register the callback, if it's not already registered.
		if (!CallbackTable[typeof(T)].Contains(cb))
		{
			CallbackTable[typeof(T)].Add(cb);
		}
		else
		{
			Assert.IsTrue(false, "Tried to register the same event callback twice!");
		}
	}

	public static void RegisterTargeted(GameObject target, Callback cb)
	{
		if (!TargetedCallbackTable.ContainsKey(typeof(T)))
		{
			TargetedCallbackTable[typeof(T)] = new Dictionary<GameObject, List<Callback>>();
		}

		if (!TargetedCallbackTable[typeof(T)].ContainsKey(target))
		{
			TargetedCallbackTable[typeof(T)][target] = new List<Callback>();
		}

		if (!TargetedCallbackTable[typeof(T)][target].Contains(cb))
		{
			TargetedCallbackTable[typeof(T)][target].Add(cb);
		}
		else
		{
			Assert.IsTrue(false, "Tried to register the same event callback twice!");
		}
	}

	/// <summary>
	/// Unregisters a previously registered callback.
	/// </summary>
	/// <param name="cb">Callback to unregister</param>
	public static void Unregister(Callback cb)
	{
		if (CallbackTable.ContainsKey(typeof(T)))
		{
			CallbackTable[typeof(T)].Remove(cb);
		}

		if (TargetedCallbackTable.ContainsKey(typeof(T)))
		{
			foreach (var target in TargetedCallbackTable[typeof(T)])
			{
				TargetedCallbackTable[typeof(T)][target.Key].Remove(cb);
			}
		}
	}

	/// <summary>
	/// Posts a game event.
	/// </summary>
	public static void Post()
	{
		T data = new T();
		Post(data);
	}

	public static void Post(GameObject target)
	{
		T data = new T();
		PostTargeted(target, data);
	}

	/// <summary>
	/// Posts a game event.
	/// </summary>
	/// <param name="data">Event data</param>
	public static void Post(T data)
	{
		// Bail if no one has registered for this event type.
		if (!CallbackTable.ContainsKey(typeof(T)))
		{
			return;
		}

		List<Callback> callbacks = CallbackTable[typeof(T)];
		List<Callback> newCallbacks = new List<Callback>();
		foreach (var cb in callbacks)
		{
			newCallbacks.Add(cb);
		}

		foreach (var cb in newCallbacks)
		{
			cb(data);
		}
	}

	public static void PostTargeted(GameObject target, T data)
	{
		// Bail if no one has registered for this event type.
		if (!TargetedCallbackTable.ContainsKey(typeof(T)) || !TargetedCallbackTable[typeof(T)].ContainsKey(target))
		{
			return;
		}

		List<Callback> callbacks = TargetedCallbackTable[typeof(T)][target];
		List<Callback> newCallbacks = new List<Callback>();
		foreach (var cb in callbacks)
		{
			newCallbacks.Add(cb);
		}

		foreach (var cb in newCallbacks)
		{
			cb(data);
		}
	}
}