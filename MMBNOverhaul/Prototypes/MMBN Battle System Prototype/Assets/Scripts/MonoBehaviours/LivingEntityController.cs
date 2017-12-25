using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public abstract class LivingEntityController : ObservableMonoBehaviour, ILivingEntity, IPausable
{
	#region Events

	public UnityEvent Paused { get; protected set; }
	public UnityEvent Unpaused { get; protected set; }
	public UnityEvent Died
	{
		get { return entityInfo.Died; }
	}

    public UnityEvent Revived
	{
		get { return entityInfo.Revived; }
	}

	public UnityEvent TookDamage 
	{
		get { return ((ILivingEntity)entityInfo).TookDamage; }
	}

    public UnityEvent TookHealing
	{
		get { return ((ILivingEntity)entityInfo).TookHealing; }
	}

	#endregion
	#region Fields
	[SerializeField] Combatant _onSideOf;
	[SerializeField] LivingEntityInfo _entityInfo;
	GameController gameController;
	public BattleMovementModule movementHandler { get; protected set; }
	protected Animator animator;

	#endregion
	#region Properties
	#region Implemented through entityInfo
	public virtual LivingEntityInfo entityInfo 
	{
		get { return _entityInfo; }
		protected set { _entityInfo = value; }

	}
    public virtual string description
	{
		get { return entityInfo.description; }
		set { entityInfo.description = value; }
	}

    public virtual LivingEntityStats stats 
	{
		get { return entityInfo.stats; }
	}

    public virtual int id 
	{
		get { return entityInfo.id; }
	}

    public virtual bool isInvincible 
	{
		get { return entityInfo.isInvincible; }
		set { entityInfo.isInvincible = value; }
	}

    public virtual bool isDead
	{
		get { return entityInfo.isDead; }
	}

	public float health 
	{
		get { return stats.health.val; }
		protected set { stats.health.val = value; }
	}

	public float effectiveHealth
	{
		get { return stats.health.effectiveVal; }
		protected set { stats.health.effectiveVal = value; }
	}

	public float maxHealth 
	{
		get { return stats.health.maxVal; }
		protected set { stats.health.maxVal = value; }
	}

	public float effectiveMaxHealth 
	{
		get { return stats.health.effectiveMaxVal; }
		protected set { stats.health.effectiveMaxVal = value; }
	}

    public virtual List<DamageType> resistances 
	{
		get { return ((ILivingEntity)_entityInfo).resistances; }
		set { _entityInfo.resistances = value; }
	}

    public virtual List<DamageType> weaknesses
	{
		get { return ((ILivingEntity)_entityInfo).weaknesses; }
		set { _entityInfo.weaknesses = value; }
	}

    

    #endregion

	public PanelController panelCurrentlyOn { get; protected set; }

	public Combatant onSideOf { get { return _onSideOf; } }
	public bool isPaused { get; protected set; }
    #endregion

    #region Methods
    #region Initialization

    protected override void Awake()
	{
		base.Awake();
		animator = GetComponent<Animator>();
		stats.Init();
		Paused = new UnityEvent();
		Unpaused = new UnityEvent();
		isPaused = false;
	}
	protected override void Start () 
	{
		base.Start();
		gameController = GameController.instance;
		WatchForGamePause();

		// the movement handler needs access to things like the game controller, 
		// hence it being initialized here
		movementHandler = new BattleMovementModule();
		movementHandler.Init(this);
		panelCurrentlyOn = GetPanelCurrentlyOn();
	}

	#endregion
	
	#region Implemented through entityInfo
    public virtual bool Die()
    {
        return ((ILivingEntity)entityInfo).Die();

    }

    public virtual bool Revive(int amount)
    {
        return ((ILivingEntity)entityInfo).Revive(amount);
    }

    public virtual bool Revive(float healthPercent)
    {
        return entityInfo.Revive(healthPercent);
    }

    public virtual bool TakeDamage(float amount, DamageType type = DamageType.none)
    {
		bool tookDamage = entityInfo.TakeDamage(amount, type);

		// don't grant invincibility for panel damage
        if (tookDamage && type == DamageType.poisonPanel)
			isInvincible = false;

		return tookDamage;
		
    }

    public virtual bool TakeHealing(float amount)
    {
        return ((ILivingEntity)entityInfo).TakeHealing(amount);
    }
	#endregion
	
    #region Other MonoBehaviour funcs

	protected override void FixedUpdate()
	{
		base.FixedUpdate();

		if (!isPaused)
			panelCurrentlyOn = GetPanelCurrentlyOn();
	}

	// Update is called once per frame
	protected override void Update () 
	{
		base.Update();
	}

	#endregion

	#region IPausable
	public virtual void Pause()
	{
		isPaused = true;
	}

	public virtual void Unpause()
	{
		isPaused = false;
	}
	#endregion

	#region Helper Funcs

	protected virtual void WatchForGamePause()
	{
		gameController.Paused.AddListener(Pause);
		gameController.Unpaused.AddListener(Unpause);
	}

	protected virtual PanelController GetPanelCurrentlyOn()
	{
		Ray down = new Ray(transform.position, -transform.up);
		RaycastHit[] hits = Physics.RaycastAll(down, 5f);

		PanelController panel = null;

		foreach (RaycastHit hit in hits)
		{
			panel = hit.transform.GetComponent<PanelController>();

			if (panel != null)
				break;
		}

		if (panel == null)
			throw new System.NullReferenceException(name + " is for some reason not on a panel.");
		
		return panel;
	}
	#endregion

	#endregion
	
}
