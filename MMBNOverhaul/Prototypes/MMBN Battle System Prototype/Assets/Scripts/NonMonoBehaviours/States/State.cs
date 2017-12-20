using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Superclass for all states FSMs may involve.
/// </summary>
public abstract class State 
{

	public virtual void Enter() {}
	public abstract void Execute();
	public virtual void Exit() {}
	
}
