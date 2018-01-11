using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HammerAI : EnemyAI
{
	public class HammerMovement : BattleMovementState
	{
		/*
		Waits until the player is within the hammer's line of sight (the entire row it is on), 
		and then moves in to attack the player. The actual attacking is handled by the HammerAI 
		class that has an instance of this as a member.
		*/

		Vector3 basePosition;
		public bool atBasePosition = true;

        protected GameController gameController;
        protected NaviBattleController navi;

		public UnityEvent ReadyToSlash { get; protected set; }

		public override void Init(LivingEntityController controller)
		{
			/*
			Setting up details that will drive how the movement works.
			 */

			base.Init(controller);
            gameController = GameController.instance;
            navi = gameController.navi;

			baseMoveDelay = 2f;
			moveDelay = 0.1f; // let the hammer attack immediately

			basePosition = mover.panelCurrentlyOn.transform.position;
			basePosition.y = mover.transform.position.y;

			ReadyToSlash = new UnityEvent();
		}

		protected override void HandleMovement()
		{

			if (atBasePosition)
			{
				// Check if the player is within line of sight (the same row)

				PanelController playerPanel = navi.panelCurrentlyOn;
				PanelController currentPanel = mover.panelCurrentlyOn;
				bool inSameRow = playerPanel.posOnGrid.y == currentPanel.posOnGrid.y;

				if (!inSameRow)
					return;

				// Now move in to attack the player
				PanelController movementTarget = battlefield.GetPanelRelativeTo(playerPanel, Direction.right);
				
				// Only if the space to move on it traversable, of course
				if (movementTarget.traversable)
				{
					Vector3 targetPos = movementTarget.transform.position;
					targetPos.y = mover.transform.position.y;

					mover.transform.position = targetPos;
					atBasePosition = false;

					ReadyToSlash.Invoke(); 
					// ^ Signals to the HammerAI object to start the attack

					ResetMoveDelay();
				}

			}
			else 
			{
				// Go back to the original position 
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

		_movementStyle.ReadyToSlash.AddListener(MakePanelFlash);
		_movementStyle.ReadyToSlash.AddListener(Attack);
	}

	public override void Execute()
	{
		// Better to have the ai here be event-triggered isntead of relying on 
		// delay timers, hence this empty execute. 
		// Another reason why the EnemyAI class might be reworked; it's better for the AI
		// to react to events by other objects than just update some delay every frame.

	}

	protected override void Attack()
	{
		enemy.StartCoroutine(AttackCoroutine());
	}

	IEnumerator AttackCoroutine()
	{
		// the attack has to happen after a delay
		float timer = gameController.frameRate * (0.35f + (Time.deltaTime * 2));

		while (true)
		{
			if (!enemy.isPaused)
				timer -= Time.deltaTime;

			if (timer <= 0)
				break;

			yield return null;
		}

		if (PlayerInAttackRange())
		{
			//TODO: play an animation
			navi.TakeDamage(damage);
			
		}

		ResetAttackDelay();

	}

	IEnumerator PanelFlashCoroutine()
	{
		// the panel must flash after a two-frame delay, otherwise the wrong panel will flash
		yield return new WaitForSeconds(Time.deltaTime * 2);

		string matPath = "Materials/Yellow";
		Material yellow = Resources.Load<Material>(matPath);

		// make the panel to the left of this one flash
		PanelController panel = battlefield.GetPanelRelativeTo( enemy.panelCurrentlyOn, 
																Direction.left);

		if (panel != null)
			panel.FlashMaterial(yellow, 0.25f, 0.05f);

	}


	bool PlayerInAttackRange()
	{
		PanelController playerPanel = navi.panelCurrentlyOn;
		PanelController currentPanel = enemy.panelCurrentlyOn;

		return (playerPanel.posOnGrid.x == currentPanel.posOnGrid.x - 1);
		
	}

	void MakePanelFlash()
	{
		enemy.StartCoroutine(PanelFlashCoroutine());
	}
	
}
