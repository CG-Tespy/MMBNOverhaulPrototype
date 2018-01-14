using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ProtomanAI : EnemyAI
{
	public class ProtomanMovement : EnemyMovementState
	{
		public UnityEvent poisedToSlash { get; protected set; }
		const int stepsBeforeAttack = 4;
		int stepsTaken = 0;

		public override void Init(LivingEntityController controller)
		{
			base.Init(controller);
			baseMoveDelay = 1f;
			moveDelay = baseMoveDelay;

			poisedToSlash = new UnityEvent();

		}

		protected override void HandleMovement()
		{
			if (stepsTaken < stepsBeforeAttack)
			{
				// teleport to a random panel on own side
				List<PanelController> panelsToMoveTo = battlefield.GetPanelsOnSide(mover.onSideOf) as List<PanelController>;
				panelsToMoveTo.Remove(mover.panelCurrentlyOn);
				// ^ make sure to move to a different panel

				if (panelsToMoveTo.Count > 0)
				{
					PanelController randPanel = panelsToMoveTo[Random.Range(0, panelsToMoveTo.Count)];

					if (randPanel.traversable)
					{
						Vector3 newPos = randPanel.transform.position;
						newPos.y = mover.transform.position.y;
						mover.transform.position = newPos;
					}
				}

				stepsTaken++;
				ResetMoveDelay();
			}
			else 
			{
				
				// Teleport in widesword range of player
				PanelController frontOfNavi = battlefield.GetPanelRelativeTo(navi.panelCurrentlyOn, 
																			Direction.right);
				PanelController belowFrontOfNavi = battlefield.GetPanelRelativeTo(frontOfNavi, 
																				Direction.down);
				PanelController aboveFrontOfNavi = battlefield.GetPanelRelativeTo(frontOfNavi,
																				Direction.up);

				// pick which panel to move to based on traversability
				Vector3 newPos = mover.transform.position;

				if (frontOfNavi != null && frontOfNavi.traversable)
					newPos = frontOfNavi.transform.position;
				else if (belowFrontOfNavi != null && belowFrontOfNavi.traversable)
					newPos = belowFrontOfNavi.transform.position;
				else if (aboveFrontOfNavi != null && aboveFrontOfNavi.traversable)
					newPos = aboveFrontOfNavi.transform.position;
				else 
					return; // For when there are no panels to move to

				newPos.y = mover.transform.position.y;

				mover.transform.position = newPos;
				stepsTaken = 0;
				ResetMoveDelay();

				mover.StartCoroutine(InvokePoisedToSlash());
			
			}
		}

		IEnumerator InvokePoisedToSlash()
		{
			// Need poisedtoSlash to invoke on a two-frame delay;
			// otherwise, the wrong panels will flash.

			float timer = Time.deltaTime * 2;

			while (true)
			{
				if (!mover.isPaused)
					timer -= Time.deltaTime;

				if (timer <= 0)
					break;
				
				yield return null;
			}

			poisedToSlash.Invoke();
		}
	}
	
	float damage = 80;
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

	protected override EnemyMovementState movementStyle
	{
		get { return _movementStyle; }
		set { _movementStyle = value as ProtomanMovement; }
	}

	public override void Init(EnemyController enemy)
	{
		base.Init(enemy);
		//enemy.movementHandler.ChangeState(movementStyle);

		baseAttackDelay = movementStyle.baseMoveDelay / 2f;
		attackDelay = baseAttackDelay;
		
		_movementStyle.poisedToSlash.AddListener(MakePanelsFlash);
		_movementStyle.poisedToSlash.AddListener(Attack);
	}

	public override void Execute()
	{
		// better to have nothing in the base execute work in this case, 
		// hence this empty Execute
		
	}

	protected override void Attack()
	{
		enemy.StartCoroutine(AttackCoroutine());
	}

	IEnumerator AttackCoroutine()
	{
		// after a delay, launch the attack
		float timer = 0.35f + (Time.deltaTime * 2);

		while (true)
		{
			if (!enemy.isPaused)
				timer -= Time.deltaTime;
			
			if (timer <= 0)
				break;

			yield return null;
		}
		
		if (NaviInWideswordRange())
		{
			//TODO: play animation
			navi.TakeDamage(damage);
		}
		
		// For fun, crack the panels in widesword range, and break the ones that are already cracked.

		foreach (PanelController panel in panelsInWideswordRange)
		{
			if (panel != null)
			{
				if (panel.panelName == "Cracked Panel" && !panel.isOccupied)
					panel.ChangeTo(PanelDatabase.instance.GetPanel("Broken Panel"));
				else if (panel.traversable)
					panel.ChangeTo(PanelDatabase.instance.GetPanel("Cracked Panel"));
			}

		}
	

		ResetAttackDelay();

	}

	void MakePanelsFlash()
	{
		// make the panels in widesword range flash
		string matPath = "Materials/Yellow";
		Material yellow = Resources.Load<Material>(matPath);

		foreach (PanelController panel in panelsInWideswordRange)
			if (panel != null)
				panel.FlashMaterial(yellow, 0.25f, 0.05f);
		
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
