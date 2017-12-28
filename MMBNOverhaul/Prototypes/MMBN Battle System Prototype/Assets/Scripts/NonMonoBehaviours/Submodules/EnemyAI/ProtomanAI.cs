using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ProtomanAI : EnemyAI
{
	public class ProtomanMovement : BattleMovementState
	{
		const int stepsBeforeAttack = 4;
		int stepsTaken = 0;
		GameController gameController;
		NaviBattleController navi;

		public bool poisedToAttack { get; protected set; }

		public override void Init(LivingEntityController controller)
		{
			base.Init(controller);
			baseMoveDelay = 1f;
			moveDelay = baseMoveDelay;

			gameController = GameController.instance;
			navi = gameController.navi;
			poisedToAttack = false;

		}

		protected override void HandleMovement()
		{
			if (stepsTaken < stepsBeforeAttack)
			{
				poisedToAttack = false;
				// teleport to a random panel on own side
				List<PanelController> ownSideOfField = battlefield.GetPanelsOnSide(mover.onSideOf) as List<PanelController>;
				ownSideOfField.Remove(mover.panelCurrentlyOn);
				// ^ make sure to move to a different panel
				PanelController randPanel = ownSideOfField[Random.Range(0, ownSideOfField.Count)];

				if (randPanel.traversable)
				{
					Vector3 newPos = randPanel.transform.position;
					newPos.y = mover.transform.position.y;
					mover.transform.position = newPos;
				}

				stepsTaken++;
				ResetMoveDelay();
			}
			else 
			{
				poisedToAttack = true;
				// teleport in widesword range of player
				PanelController frontOfNavi = battlefield.GetPanelRelativeTo(navi.panelCurrentlyOn, 
																			Direction.right);
				PanelController belowFrontOfNavi = battlefield.GetPanelRelativeTo(frontOfNavi, 
																				Direction.down);
				PanelController aboveFrontOfNavi = battlefield.GetPanelRelativeTo(frontOfNavi,
																				Direction.up);

				Vector3 newPos = mover.transform.position;

				// pick which panel to move to based on traversability
				if (frontOfNavi != null && frontOfNavi.traversable)
					newPos = frontOfNavi.transform.position;
				else if (belowFrontOfNavi != null && belowFrontOfNavi.traversable)
					newPos = belowFrontOfNavi.transform.position;
				else if (aboveFrontOfNavi != null && aboveFrontOfNavi.traversable)
					newPos = aboveFrontOfNavi.transform.position;

				newPos.y = mover.transform.position.y;

				mover.transform.position = newPos;
				stepsTaken = 0;
				ResetMoveDelay();

			}
		}
	}
	
	float damage = 80;
	bool panelsFlashing = false;
	ProtomanMovement _movementStyle = new ProtomanMovement();
	protected PanelController[] panelsInWideswordRange
	{
		get 
		{ 
			PanelController[] panels = new PanelController[3];

			PanelController currentPanel = enemy.panelCurrentlyOn;

			PanelController inFront = battlefield.GetPanelRelativeTo(currentPanel, Direction.left);
			PanelController diagUp = battlefield.GetPanelRelativeTo(inFront, Direction.up);
			PanelController diagDown = battlefield.GetPanelRelativeTo(inFront, Direction.down);

			return new PanelController[] { inFront, diagUp, diagDown };
		}
	}

	protected override BattleMovementState movementStyle
	{
		get { return _movementStyle; }
		set { _movementStyle = value as ProtomanMovement; }
	}

	public override void Init(EnemyController enemy)
	{
		base.Init(enemy);
		movementStyle.Init(enemy);
		enemy.movementHandler.ChangeState(movementStyle);

		baseAttackDelay = movementStyle.baseMoveDelay / 2f;
		attackDelay = baseAttackDelay;
	}

	public override void Execute()
	{
		if (_movementStyle.poisedToAttack)
		{
			if (!panelsFlashing)
			{
				// make the panels flash
				string matPath = "Materials/Yellow";
				Material yellow = Resources.Load<Material>(matPath);

				foreach (PanelController panel in panelsInWideswordRange)
				{
					if (panel != null)
						panel.FlashMaterial(yellow, 0.05f, 0.05f);
				}

				panelsFlashing = true;
			}

			Debug.Log(enemy.name + " poised to attack!");
			attackDelay -= Time.deltaTime;
		}

		if (attackDelay <= 0)
			canAttack = true;

		if (canAttack)
			Attack();
		
	}

	protected override void Attack()
	{
		if (NaviInWideswordRange())
		{
			//TODO: play animation
			navi.TakeDamage(damage);
		}

		ResetAttackDelay();
		panelsFlashing = false;
	}

	bool NaviInWideswordRange()
	{
		PanelController naviPanel = navi.panelCurrentlyOn;
		PanelController currentPanel = enemy.panelCurrentlyOn;

		bool inFront = naviPanel.posOnGrid.x == currentPanel.posOnGrid.x - 1;
		int distBetweenPanels = (int)Mathf.Abs(naviPanel.posOnGrid.y - currentPanel.posOnGrid.y);
		bool vertFlank = inFront && distBetweenPanels == 1;

		return (inFront || vertFlank);
	}

	


}
