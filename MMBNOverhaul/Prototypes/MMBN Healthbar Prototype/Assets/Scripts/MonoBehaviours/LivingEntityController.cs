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

	#endregion
	#region Fields
	[SerializeField] LivingEntityInfo _entityInfo;
	GameController gameController;

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
	#endregion
	
	#endregion

	#region Methods
	#region Initialization

	protected virtual void Awake()
	{
		
	}
	protected virtual void Start () 
	{
		stats.Init();
		gameController = GameController.instance;
	}

	#endregion
	#region Implemented through entityInfo
    public virtual void Die()
    {
        ((ILivingEntity)entityInfo).Die();
    }

    public virtual void Revive(int amount)
    {
        ((ILivingEntity)entityInfo).Revive(amount);
    }

    public virtual void Revive(float healthPercent)
    {
        ((ILivingEntity)entityInfo).Revive(healthPercent);
    }

    public virtual void TakeDamage(int amount)
    {
        ((ILivingEntity)entityInfo).TakeDamage(amount);
    }

    public virtual void TakeHealing(int amount)
    {
        ((ILivingEntity)entityInfo).TakeHealing(amount);
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
