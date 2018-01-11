using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public abstract class EnemyAI : State
{
	/*
	Each enemy has an instance of this, and each instance gets its execute method called 
	by the enemy controller when it isn't paused. 
	 */

	#region Fields
	protected EnemyController enemy;
	protected GameController gameController;
	protected BattlefieldManager battlefield;
	protected NaviBattleController navi;
	protected bool canAttack = false;

	#endregion

	#region Properties
	
	protected virtual BattleMovementState movementStyle { get; set; }
	protected virtual float baseAttackDelay { get; set; }
	protected virtual float attackDelay { get; set; }

	#endregion

	#region Methods
	public virtual void Init(EnemyController enemy)
	{
		/*
		The AI needs to know who it belongs to, what is the navi, what is the game controller, 
		and what it the battlefield. References to these objects make its job easier.
		 */
		 
		this.enemy = enemy;
		gameController = GameController.instance;
		battlefield = BattlefieldManager.instance;
		navi = gameController.navi;

	}

	public override void Execute()
	{
		// The attack delay ticks down, until it is time to attack. This 
		// function will probably be removed or reworked when animations get implemented, 
		// since I've found that it'd be better for the EnemyAI subclasses to work 
		// by reacting to events, rather than ticking down some delay value every frame.
		if (!canAttack)
			attackDelay -= Time.deltaTime;
		else 
			Attack();

		if (attackDelay <= 0)
			canAttack = true;
	}

	protected abstract void Attack();

	protected virtual void ResetAttackDelay()
	{
		attackDelay = baseAttackDelay;
		canAttack = false;
	}

	#endregion
	
}
