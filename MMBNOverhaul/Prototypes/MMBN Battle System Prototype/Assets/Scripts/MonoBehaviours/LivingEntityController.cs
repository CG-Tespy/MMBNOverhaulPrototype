using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public abstract class LivingEntityController : MonoBehaviour, ILivingEntity
{
	#region Events
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
	protected BattleMovementModule movementHandler;
	protected Animator animator;

	#endregion
	#region Properties
	#region Implemented through entityInfo
	public virtual LivingEntityInfo entityInfo 
	{
		get { return entityInfo; }

	}
    public string description
	{
		get { return ((ILivingEntity)entityInfo).description; }
		set { entityInfo.description = value; }
	}

    public LivingEntityStats stats 
	{
		get { return ((ILivingEntity)entityInfo).stats; }
	}

    public int id 
	{
		get { return ((ILivingEntity)entityInfo).id; }
	}

    public bool isInvincible 
	{
		get { return ((ILivingEntity)entityInfo).isInvincible; }
	}

    public bool isDead
	{
		get { return ((ILivingEntity)entityInfo).isDead; }
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

    public List<DamageType> resistances 
	{
		get { return ((ILivingEntity)_entityInfo).resistances; }
		set { _entityInfo.resistances = value; }
	}

    public List<DamageType> weaknesses
	{
		get { return ((ILivingEntity)_entityInfo).weaknesses; }
		set { _entityInfo.weaknesses = value; }
	}

    

    #endregion

	public PanelController panelCurrentlyOn
	{
		get 
		{
			// raycast down to find the panel
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
				throw new System.ApplicationException(name + " is for some reason not on a panel.");
			return panel;
		}
	}

	public Combatant onSideOf { get { return _onSideOf; } }
    #endregion

    #region Methods
    #region Initialization

    protected virtual void Awake()
	{
		animator = GetComponent<Animator>();
	}
	protected virtual void Start () 
	{
		stats.Init();
		gameController = GameController.instance;

		// the movement handler needs access to things like the game controller, 
		// hence it being initialized here
		movementHandler = new BattleMovementModule();
		movementHandler.Init(this);
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
        return entityInfo.TakeDamage(amount, type);
		
    }

    public virtual bool TakeHealing(float amount)
    {
        return ((ILivingEntity)entityInfo).TakeHealing(amount);
    }
	#endregion
	
    #region Other MonoBehaviour funcs
	// Update is called once per frame
	protected virtual void Update () 
	{
		
	}

	#endregion

	#endregion
	
}
