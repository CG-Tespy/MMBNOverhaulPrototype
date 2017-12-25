using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

class NaviBattleStates
{
	NaviBattleController player;
	public PlayerBattleMovement playerControlled;

	public NaviBattleStates(NaviBattleController player)
	{
		this.player = player;
		playerControlled = new PlayerBattleMovement();
		playerControlled.Init(player);
	}


}
public class NaviBattleController : LivingEntityController
{

	#region Fields
	NaviBattleStates states;
	[SerializeField] NaviInfo _naviInfo;

	#endregion

	#region Properties

	override public LivingEntityInfo entityInfo 
	{
		get { return _naviInfo as LivingEntityInfo; }
	}

	BattlefieldManager battleField { get { return BattlefieldManager.instance; } }

	#endregion
	protected override void Awake()
	{
		base.Awake();
		animator = GetComponent<Animator>();
		
		
	}
	// Use this for initialization
	protected override void Start () 
	{
		base.Start();
		states = new NaviBattleStates(this);
		movementHandler.ChangeState(states.playerControlled);
		MoveToCenterOfOwnSide();
		StartCoroutine(HandleBlinking());
		
		
	}
	
	// Update is called once per frame
	protected override void Update () 
	{
		base.Update();
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();

		if (!isPaused)
			movementHandler.Execute();
	}

	/// <summary>
	/// Moves this navi to the center of its own side of the battlefield.
	/// </summary>
	void MoveToCenterOfOwnSide()
	{
		// TODO: Maybe at some point, change algorithm so it counts the size of the player's own 
		// side of the battlefield
		int centerX = (int) battleField.dimensions.x / 4;
		int centerY = (int) battleField.dimensions.y / 2;
		//Debug.Log("center panel x: " + centerX + " Center y: " + centerY);
		PanelController centerPanel = battleField.panelGrid[centerX, centerY];
		Vector3 centerPos = centerPanel.transform.position;
		centerPos.y = transform.position.y;
		transform.position = centerPos;
	}

	protected override void WatchForGamePause()
	{
		base.WatchForGamePause();
	}

	IEnumerator HandleBlinking()
	{
		float timer = entityInfo.invincibilityTime;

		while (true)
		{
			if (!isPaused)
			{
				if (isInvincible)
				{
					renderer.enabled = !renderer.enabled;
					timer -= Time.deltaTime;
				}
				
				if (timer <= 0)
				{
					isInvincible = false;
					renderer.enabled = true;
					timer = entityInfo.invincibilityTime;
				}
			}

			yield return null;
		}
	}

	#region IPausable

	public override void Pause()
	{
		base.Pause();
		animator.enabled = false;
	}

	public override void Unpause()
	{
		base.Unpause();
		animator.enabled = true;
	}

	#endregion
	
}
