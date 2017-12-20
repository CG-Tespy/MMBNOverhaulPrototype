using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BattleMovementModule : IPausable
{
	#region Events
	public UnityEvent Paused { get; protected set; }
	public UnityEvent Unpaused { get; protected set; }
	#endregion

	#region Fields

	LivingEntityController controller;
	BattleMovementState currentState;

	GameController gameController;
	#endregion

	#region Properties
	public bool isPaused { get; protected set; }
	public Controls.BattleControls controls
	{
		get { return Controls.BattleControls.instance; }
	}
	#endregion

	#region Methods

	#region Initialization
	public BattleMovementModule()
	{
		isPaused = false;
		Paused = new UnityEvent();
		Unpaused = new UnityEvent();
	}
	public void Init(LivingEntityController controller)
	{
		this.controller = controller;
		gameController = GameController.instance;
	}

	#endregion
	
	public void Execute()
	{
		if (!isPaused)
			if (currentState != null)
				currentState.Execute();
		
	}

	#region IPausable
	public void Pause()
	{
		isPaused = true;
		Paused.Invoke();
	}

	public void Unpause()
	{
		isPaused = false;
		Unpaused.Invoke();
	}

	#endregion

	public void ChangeState(BattleMovementState newState)
	{
		if (currentState != null)
			currentState.Exit();

		currentState = newState;
		currentState.Enter();
	}

	#region Helper funcs

	void WatchForGamePause()
	{
		gameController.Paused.AddListener(Pause);
		gameController.Unpaused.AddListener(Unpause);
	}

	#endregion

	#endregion
	
}
