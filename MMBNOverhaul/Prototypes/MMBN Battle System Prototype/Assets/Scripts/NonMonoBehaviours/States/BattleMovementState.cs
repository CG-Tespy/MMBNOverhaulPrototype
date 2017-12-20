using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public abstract class BattleMovementState : State
{
	protected const float baseMoveDelay = 0.25f;
	protected float moveDelay = 0.25f;

	protected float moveDistance = 1f;
	protected LivingEntityController mover;

	protected virtual LivingEntityInfo moverInfo 
	{
		get { return mover.entityInfo; }
	}	

	protected bool canMove { get; set; }
	public Controls.BattleControls controls 
	{
		get { return Controls.BattleControls.instance; }
	}

	public virtual void Init(LivingEntityController controller)
	{
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
		
	}

	protected abstract void HandleMovement();

	
}
