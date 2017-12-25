using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public class MettaurAI : EnemyAI
{
	#region Nested classes
	public class MettaurMovement : BattleMovementState
	{
		NaviBattleController navi;
		GameController gameController;

		public override void Init(LivingEntityController enemy)
		{
			base.Init(enemy);
			baseMoveDelay = 1.5f;
			moveDelay = baseMoveDelay;
			gameController = GameController.instance;
			navi = gameController.navi;
			//battlefield = BattlefieldManager.instance;
		}

		public override void Execute()
		{
			base.Execute();
			Debug.Log("Executing mettaur movement!");
		}

		protected override void HandleMovement()
		{
			// move up or down a panel depending on where the player is relative to tbis 
			// mettaur

			// get the panel this mettaur is standing on, as well as that the player is 
			// standing on, before moving the mettaur up or down depending on whether 
			// the player is.
			PanelController standingOn = mover.panelCurrentlyOn;
			PanelController playerPanel = navi.panelCurrentlyOn;

			bool moveUp = playerPanel.posOnGrid.y > standingOn.posOnGrid.y;
			bool moveDown = playerPanel.posOnGrid.y < standingOn.posOnGrid.y;

			PanelController panelToMoveTo = null;

			if (moveUp)
				panelToMoveTo = battlefield.GetPanelRelativeTo(standingOn, Direction.up);
			else if (moveDown)
				panelToMoveTo = battlefield.GetPanelRelativeTo(standingOn, Direction.down);
			else
			{
				// mettaurs are dumb.
				ResetMoveDelay();
				return;
			}

			Vector3 newPos = panelToMoveTo.transform.position;
			newPos.y = mover.transform.position.y;
			Debug.Log("Moving " + mover.name + " to panel at coords" + panelToMoveTo.posOnGrid);
			mover.transform.position = newPos;
			ResetMoveDelay();
		}



	}

	#endregion

	static List<EnemyController> mettsInField = new List<EnemyController>();
	static EnemyController activeMett = null;
	static int activeMettIndex = 0;
	static bool mettsOrdered = false;

	MettaurMovement _movementStyle = new MettaurMovement();
	protected float shockwaveDamage = 10;
	protected float shockwaveSpeed = 1f;

	GameObject shockWaveObject = null;
	protected override BattleMovementState movementStyle
	{
		get { return _movementStyle; }
		set { _movementStyle = value as MettaurMovement; }
	}

	public override void Init(EnemyController mett)
	{
		base.Init(mett);
		mettsInField.Add(mett);

		canAttack = false;
		// ^make the first mettaur wait before attacking

		baseAttackDelay = 5f;
		attackDelay = baseAttackDelay;

		movementStyle.Init(mett);
		enemy.movementHandler.ChangeState(movementStyle);

		mett.mBEvents.Destroy.AddListener(OnDestroy);
	}

	public override void Execute()
	{
		if (!mettsOrdered)
			OrderMetts();
		
		if (activeMett != this.enemy)
		{
			this.enemy.Pause();
			return;
		}

		base.Execute();
		Debug.Log("Executing mettaur ai!");
	}

	protected override void Attack()
	{
		
		// spawn a shockwave on the panel to the left of this mettaur

		PanelController toTheLeft = battlefield.GetPanelRelativeTo(enemy.panelCurrentlyOn, Direction.left);
		
		Vector3 spawnPos = toTheLeft.transform.position;
		spawnPos.y = enemy.transform.position.y;

		// TODO: play an animation, make the shockwave spawn at the end of it

		// spawn the shockwave prefab and move it
		string prefabPath = "Prefabs/Attacks/Shockwave";
		GameObject shockwavePrefab = Resources.Load<GameObject>(prefabPath);

		GameObject shockWaveGo = MonoBehaviour.Instantiate<GameObject>(shockwavePrefab);
		shockWaveGo.transform.position = spawnPos;

		Shockwave shockwaveScript = shockWaveGo.GetComponent<Shockwave>();
		shockwaveScript.Init(enemy);
		shockwaveScript.moveSpeed = shockwaveSpeed;
		shockwaveScript.Move(Vector3.left);

		Debug.Log(enemy.name + " created a shockwave!");
		ResetAttackDelay();
		PassToNextMett();
	}

	void OrderMetts()
	{
		// order the metts to the ones further to the left act first
		mettsInField = new List<EnemyController>(mettsInField.OrderBy(mett => mett.transform.position.x));

		// set the first one as the active mett, so it acts first
		activeMett = mettsInField[activeMettIndex];

		mettsOrdered = true;
	}

	void OnDestroy()
	{
		mettsInField.Remove(enemy);

		if (activeMett == enemy && mettsInField.Count > 0)
		{
			activeMettIndex = 0;
			activeMett = mettsInField[activeMettIndex];
			activeMett.Unpause();
		}
	}

	void PassToNextMett()
	{
		activeMettIndex++;

		if (activeMettIndex >= mettsInField.Count)
			activeMettIndex = 0;

		activeMett = mettsInField[activeMettIndex];
		activeMett.Unpause();
	}
	
}
