using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public abstract class BattleMovementState : State
{
	#region Fields

	protected float moveDistance = 1f;
	protected LivingEntityController mover;
	#endregion

	#region Properties
	protected float baseMoveDelay { get; set; }
	protected float moveDelay { get; set; }

	protected virtual LivingEntityInfo moverInfo 
	{
		get { return mover.entityInfo; }
	}	

	protected bool canMove { get; set; }
	public Controls.BattleControls controls 
	{
		get { return Controls.BattleControls.instance; }
	}

	#endregion

	#region Methods

	public virtual void Init(LivingEntityController controller)
	{
		baseMoveDelay = 0.25f;
		moveDelay = baseMoveDelay;
		canMove = false;
		this.mover = controller;
	}

	public override void Execute()
	{
		if (moveDelay <= 0)
			canMove = true;

		if (canMove)
			HandleMovement();
		else 
			moveDelay -= Time.deltaTime;

		//Debug.Log("Time delta time in battle movement is " + Time.deltaTime);
		
	}

	protected abstract void HandleMovement();

	protected virtual void ResetMoveDelay()
	{
		moveDelay = baseMoveDelay;
		canMove = false;
	}
	
	#endregion
}
