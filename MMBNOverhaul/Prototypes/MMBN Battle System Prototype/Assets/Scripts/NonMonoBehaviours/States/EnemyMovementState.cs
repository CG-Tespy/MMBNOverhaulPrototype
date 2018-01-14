using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public abstract class EnemyMovementState : BattleMovementState
{
	public UnityEvent Moved = new UnityEvent();
	protected NaviBattleController navi;
	protected GameController gameController;

	new protected EnemyController mover;

	public override void Init(LivingEntityController controller)
	{
		base.Init(controller);
		mover = controller as EnemyController;
		gameController = GameController.instance;
		navi = gameController.navi;
	}
    
}
