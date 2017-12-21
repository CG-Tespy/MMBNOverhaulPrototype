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
public class NaviBattleController : LivingEntityController, IPausable
{
	#region Events
	public UnityEvent Paused { get; protected set; }
	public UnityEvent Unpaused { get; protected set; }

	#endregion

	#region Fields
	NaviBattleStates states;
	[SerializeField] NaviInfo _naviInfo;

	#endregion

	#region Properties

	override public LivingEntityInfo entityInfo 
	{
		get { return _naviInfo as LivingEntityInfo; }
	}

	public bool isPaused { get; protected set; }
	BattlefieldManager battleField { get { return BattlefieldManager.instance; } }

	#endregion
	protected override void Awake()
	{
		base.Awake();
		isPaused = false;
		animator = GetComponent<Animator>();
		
	}
	// Use this for initialization
	protected override void Start () 
	{
		base.Start();
		states = new NaviBattleStates(this);
		movementHandler.ChangeState(states.playerControlled);
		MoveToCenterOfOwnSide();
		
		WatchForGamePause();
		
	}
	
	// Update is called once per frame
	protected override void Update () 
	{
		base.Update();
	}

	protected virtual void FixedUpdate()
	{
		if (!isPaused)
		{
			movementHandler.Execute();
		}
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
		Debug.Log("center panel x: " + centerX + " Center y: " + centerY);
		PanelController centerPanel = battleField.panelGrid[centerX, centerY];
		Vector3 centerPos = centerPanel.transform.position;
		centerPos.y = transform.position.y;
		transform.position = centerPos;
	}

	void WatchForGamePause()
	{
		GameController gameController = GameController.instance;
		gameController.Paused.AddListener(Pause);
		gameController.Unpaused.AddListener(Unpause);
	}

	#region IPausable

	public void Pause()
	{
		isPaused = true;
		animator.enabled = false;
	}

	public void Unpause()
	{
		isPaused = false;
		animator.enabled = true;
	}

	#endregion
	
}
