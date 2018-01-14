using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public class MettaurAI : EnemyAI
{
	#region Nested classes
	public class MettaurMovement : EnemyMovementState
	{

		public override void Init(LivingEntityController enemy)
		{
			base.Init(enemy);
			baseMoveDelay = 1.5f;
			moveDelay = baseMoveDelay;
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

			// Get the panel this mettaur is standing on, as well as that the player is 
			// standing on...
			PanelController standingOn = mover.panelCurrentlyOn;
			PanelController playerPanel = navi.panelCurrentlyOn;

			// Decide how to move...
			bool moveUp = playerPanel.posOnGrid.y > standingOn.posOnGrid.y;
			bool moveDown = playerPanel.posOnGrid.y < standingOn.posOnGrid.y;

			PanelController panelToMoveTo = null;

			if (moveUp)
				panelToMoveTo = battlefield.GetPanelRelativeTo(standingOn, Direction.up);
			else if (moveDown)
				panelToMoveTo = battlefield.GetPanelRelativeTo(standingOn, Direction.down);
			
			if (moveUp || moveDown)
			{
				ResetMoveDelay();
				Moved.Invoke();
			}

			// ... And move!
			if (panelToMoveTo == null)
				return;

			Vector3 newPos = panelToMoveTo.transform.position;
			newPos.y = mover.transform.position.y;
			Debug.Log("Moving " + mover.name + " to panel at coords" + panelToMoveTo.posOnGrid);
			mover.transform.position = newPos;
			
		}



	}

	#endregion

	static List<EnemyController> mettsInField = new List<EnemyController>();
	static EnemyController activeMett = null;
	static int activeMettIndex = 0;
	static bool mettsOrdered = false;

	
	protected float shockwaveDamage = 10;
	protected float shockwaveSpeed = 1f;

	GameObject shockWaveObject = null;
	MettaurMovement _movementStyle = new MettaurMovement();
	protected override EnemyMovementState movementStyle
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

		baseAttackDelay = 2.5f;
		attackDelay = baseAttackDelay;

		mett.mBEvents.Destroy.AddListener(OnDestroy);
		movementStyle.Moved.AddListener(ResetAttackDelay);
	}

	public override void Execute()
	{
		if (!mettsOrdered)
			OrderMetts();
		
		// Do nothing if it isn't this mett's turn to act
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
		// Makes it so that only one of the metts acts at a time, just like in the original BN 
		// games.

		// Order the metts to the ones further to the left act first
		mettsInField = new List<EnemyController>(mettsInField.OrderBy(mett => mett.transform.position.x));

		// set the first one as the active mett, so it acts first
		activeMett = mettsInField[activeMettIndex];

		mettsOrdered = true;
	}

	void OnDestroy()
	{
		// Avoid messing up the action order. Remove this instance from the 
		// metts in the field, so the next mett can act as it should.
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
		// Sets another mett to be the active one, so it can get its turn to act.
		activeMettIndex++;

		if (activeMettIndex >= mettsInField.Count)
			activeMettIndex = 0;

		activeMett = mettsInField[activeMettIndex];
		activeMett.Unpause();
	}
	
}
