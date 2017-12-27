using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HammerAI : EnemyAI
{
	public class HammerMovement : BattleMovementState
	{
		Vector3 basePosition;
		public bool atBasePosition = true;

        protected GameController gameController;
        protected NaviBattleController navi;

		public override void Init(LivingEntityController controller)
		{
			base.Init(controller);
            gameController = GameController.instance;
            navi = gameController.navi;

			baseMoveDelay = 2f;
			moveDelay = 0.1f; // let the hammer attack immediately

			basePosition = mover.panelCurrentlyOn.transform.position;
			basePosition.y = mover.transform.position.y;
		}

		protected override void HandleMovement()
		{

			if (atBasePosition)
			{
				// move in and slash the player if the player is in the same row as this 
				// hammer

				PanelController playerPanel = navi.panelCurrentlyOn;
				PanelController currentPanel = mover.panelCurrentlyOn;
				bool inSameRow = playerPanel.posOnGrid.y == currentPanel.posOnGrid.y;

				if (!inSameRow)
					return;

				PanelController movementTarget = battlefield.GetPanelRelativeTo(playerPanel, Direction.right);
				
				// but don't if you can't move on that target
				if (movementTarget.traversable)
				{
					Vector3 targetPos = movementTarget.transform.position;
					targetPos.y = mover.transform.position.y;

					mover.transform.position = targetPos;
					atBasePosition = false;
					ResetMoveDelay();
				}

			}
			else 
			{
				// go back to previous position
				mover.transform.position = basePosition;
				ResetMoveDelay();
				atBasePosition = true;
			}
		}


	}

	HammerMovement _movementStyle = new HammerMovement();
	protected override BattleMovementState movementStyle 
	{
		get { return _movementStyle as BattleMovementState; }
		set { _movementStyle = value as HammerMovement; }
	}

	float damage = 30;


	public override void Init(EnemyController enemy)
	{
		base.Init(enemy);

        movementStyle.Init(enemy);
		baseAttackDelay = movementStyle.baseMoveDelay / 2.5f;
		attackDelay = baseAttackDelay;

		enemy.movementHandler.ChangeState(movementStyle);
		canAttack = false;
	}

	public override void Execute()
	{
		// execute the attack delay when not at base position
		if (!_movementStyle.atBasePosition && !canAttack)
			attackDelay -= Time.deltaTime;

		if (attackDelay <= 0)
			canAttack = true;

		if (canAttack)
			Attack();

		
	}

	protected override void Attack()
	{
		PanelController playerPanel = navi.panelCurrentlyOn;
		PanelController toTheLeft = battlefield.GetPanelRelativeTo(	enemy.panelCurrentlyOn, 
																	Direction.left);

		if (playerPanel == toTheLeft)
		{
			//TODO: play an animation
			navi.TakeDamage(damage);
			
		}

		ResetAttackDelay();
	}

	bool PlayerInAttackRange()
	{
		PanelController playerPanel = navi.panelCurrentlyOn;
		PanelController currentPanel = enemy.panelCurrentlyOn;

		return (playerPanel.posOnGrid.x == currentPanel.posOnGrid.x - 1);
		
	}
	
}
