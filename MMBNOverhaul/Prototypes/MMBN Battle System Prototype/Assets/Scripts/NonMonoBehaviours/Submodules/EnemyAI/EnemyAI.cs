using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public abstract class EnemyAI : State
{
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
		this.enemy = enemy;
		gameController = GameController.instance;
		battlefield = BattlefieldManager.instance;
		navi = gameController.navi;

	}

	public override void Execute()
	{
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
