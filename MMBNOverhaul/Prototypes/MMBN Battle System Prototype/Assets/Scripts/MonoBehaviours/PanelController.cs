using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PanelController : ObservableMonoBehaviour, IBattlefieldPanel, IPausable
{
	#region Events
	public UnityEvent Paused { get; protected set; }
	public UnityEvent Unpaused { get; protected set; }


	#endregion

	#region Fields
	[SerializeField] protected int panelTypeId = 0;
	[SerializeField] protected Vector2 _posOnGrid;
	[SerializeField] protected Combatant _owner; // is it the player? The enemy?

	[Header("Set programmatically based on id")]
	[SerializeField] PanelInfo panelInfo;

	GameController gameController;
	
	#endregion 
	
	#region Properties
	public BoxCollider boxCollider { get; protected set; }

	public bool isPaused { get; protected set; }

	#region Implemented through panelInfo
    public bool traversable 
	{
		get { return ((IBattlefieldPanel)panelInfo).traversable; }
		set { panelInfo.traversable = value; }
	}

    public Material material
	{
		get { return panelInfo.material; }
		set 
		{ 
			panelInfo.material = value; 
			//renderer.material = value; 
			centerRenderer.material = value;
		}
	}

    public PanelType type
	{
	 	get {return ((IBattlefieldPanel)panelInfo).type; }
	 	set { panelInfo.type = value; }
	}


    public PanelEffect effect
	{
		get { return panelInfo.effect; }
		set { panelInfo.effect = value; }
	}


	#endregion

	public GameObject frame { get; protected set; }	
	public GameObject center { get; protected set; }
	public Vector2 posOnGrid
	{
		get { return _posOnGrid; }
		protected set { _posOnGrid = value; }
	}

	public Combatant owner { get { return _owner; } }
	
	Renderer centerRenderer;
	// the frame is what this script should be attached to, the center is the 
	// child of the frame where the material is applied

	#endregion

	#region Methods

	#region Initialization

	protected override void Awake()
	{
		frame = this.gameObject;
		
		base.Awake();
		isPaused = 		false;
		Paused = 		new UnityEvent();
		Unpaused = 		new UnityEvent();
		boxCollider = 	GetComponent<BoxCollider>();
	}

    // Use this for initialization
    protected override void Start () 
	{
		base.Start();
		
		center = transform.Find("PanelCenter").gameObject;
		centerRenderer = center.GetComponent<Renderer>();

		GetPanelData();
		ApplyPanelData();

		gameController = GameController.instance;
		gameController.Paused.AddListener(Pause);
		gameController.Unpaused.AddListener(Unpause);
		
	}
	
	#endregion

	#region Update
	// Update is called once per frame
	protected override void Update () 
	{
		base.Update();

		// Because of the update execution order, this needs to be in Update for the healthbar 
		// to change color properly when an effect changes the player's HP. 
		if (!isPaused)
		{
			if (effect != null)
				effect.Execute();
		}
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();

		
	}

	#endregion

	/// <summary>
	/// For things like changing a normal panel to a broken one, etc.
	/// </summary>
	/// <param name="newPanel"></param>
	public void ChangeTo(PanelInfo newPanel)
	{
		if (this.panelInfo.Equals(newPanel))
			Debug.LogWarning(name + ": this panel is already a " + newPanel.name);
		else 
		{
			panelInfo = newPanel;
			ApplyPanelData();
		}
	}


	#region IPausable

	public void Pause()
	{
		isPaused = true;
	}

	public void Unpause()
	{
		isPaused = false;
	}


	#endregion

	#region Helper funcs

	void GetPanelData()
	{
		panelInfo = PanelDatabase.instance.GetPanel(panelTypeId);
	}

	void ApplyPanelData()
	{
		if (panelInfo.name.Contains("Poi"))
		{
			Debug.Log("Initializing poison panel.");
		}
		panelInfo.Init(this);
		if (panelInfo.material != null)
			centerRenderer.material = panelInfo.material;
	}

	#endregion

	#endregion
	
}
