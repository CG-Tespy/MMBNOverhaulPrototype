using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameController : MonoBehaviour 
{
	#region Events
	public UnityEvent Paused { get; protected set; }
	public UnityEvent Unpaused { get; protected set; }
	// ^ Modules hooked up to these events should pause and unpause their own actions
	// when these events are invoked

	#endregion

	#region Fields
	public static GameController instance { get; protected set; }
	[SerializeField] int _frameRate = 60;
	[SerializeField] GameObject pauseMenu = null;

	Controls.GeneralControls generalControls;

	#endregion


	#region Properties
	public bool gamePaused { get; protected set; }
	public int frameRate 
	{
		get { return _frameRate; }
		set 
		{
			_frameRate = value;
			ApplyFrameRate();
		}
	}

	#endregion


	#region Methods

	#region Initialization

	void Awake()
	{
		instance = this;
		gamePaused = 	false;
		Paused = 		new UnityEvent();
		Unpaused = 		new UnityEvent();
		ApplyFrameRate();
	}

	// Use this for initialization
	void Start () 
	{
		generalControls = Controls.GeneralControls.instance;
	}

	#endregion
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (Input.GetKeyDown(generalControls.pauseGame) && !gamePaused)
			PauseGame();
		else if (Input.GetKeyDown(generalControls.pauseGame) && gamePaused)
			UnpauseGame();
	}

	public void PauseGame()
	{
		gamePaused = true;
		Paused.Invoke();

		if (pauseMenu != null && pauseMenu.activeSelf == false)
			pauseMenu.SetActive(true);
	}

	public void UnpauseGame()
	{
		gamePaused = false;
		Unpaused.Invoke();

		if (pauseMenu != null && pauseMenu.activeSelf == true)
			pauseMenu.SetActive(false);
	}

	#region Helper Functions
	void ApplyFrameRate()
	{
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = _frameRate;
	}

	#endregion

	#endregion
	
}
