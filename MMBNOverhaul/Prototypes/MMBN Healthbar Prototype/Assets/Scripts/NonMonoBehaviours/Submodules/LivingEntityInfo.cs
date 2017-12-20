using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public abstract class LivingEntityInfo : ILivingEntity
{

	#region Events
    public UnityEvent Died { get; protected set; }

    public UnityEvent Revived { get; protected set; }

	#endregion

	#region Fields

	#region Backing Fields
	[SerializeField] protected string _name;
	[SerializeField] protected string _description;
	[SerializeField] protected int _id;
	[SerializeField] protected LivingEntityStats _stats;
	[SerializeField] protected float _invincibilityTime = 0;
	#endregion
	public AnimationClip[] animations;

	#region Backing fields for debugging 
	[Header("For debugging")]
	[SerializeField] protected bool _isDead = false;
	[SerializeField] protected bool _isInvincible = false;
	#endregion
	

	#endregion

	#region Properties

	#region For Backing Fields
	public string name 
	{
		get { return _name; }
		set { _name = value; }
	}

	public string description 
	{
		get { return _description; }
		set { _description = value; } 
	}

	public int id
	{
		get { return _id; }
	}
	
	public LivingEntityStats stats
	{
		get { return _stats; }
		protected set { _stats = value; }
	}
    public float invincibilityTime
	{
		get { return _invincibilityTime; }
		protected set { _invincibilityTime = value; }
	}

    public bool isInvincible 
	{
		get { return _isInvincible; }
		set { _isInvincible = value; }
	}

	public bool isDead
	{
		get
		{
			_isDead = stats.health.effectiveVal <= 0; 
			return _isDead;
		}
	}
	#endregion

	#region Health interface
	public int health 
	{
		get { return (int)stats.health.val; }
		protected set { stats.health.val = (float)value; }
	}

	public int effectiveHealth
	{
		get { return (int)stats.health.effectiveVal; }
		protected set { stats.health.effectiveVal = (float)value; }
	}

	public int maxHealth 
	{
		get { return (int)stats.health.maxVal; }
		protected set { stats.health.maxVal = (float)value; }
	}

	public int effectiveMaxHealth 
	{
		get { return (int)stats.health.effectiveMaxVal; }
		protected set { stats.health.effectiveMaxVal = (float)value; }
	}
	#endregion
	

	#endregion

	#region Methods

	public LivingEntityInfo()
	{
		Died = 		new UnityEvent();
		Revived =  	new UnityEvent();
	}

    public virtual void Die()
	{
		health = 0;
		effectiveHealth = health;
	}

    public virtual void Revive(float revPercent)
	{
		health = (int)(maxHealth * revPercent);
		effectiveHealth = health;
	}

    public virtual void Revive(int healthLeft)
    {
        health = healthLeft;
		effectiveHealth = health;
    }

    public virtual void TakeDamage(int amount)
    {
        if (!isDead && !isInvincible)
		{
			health -= amount;
			effectiveHealth -= amount;
		}
    }

    public void TakeHealing(int amount)
    {
        if (!isDead)
		{
			health += amount;
			effectiveHealth += amount;
		}
    }

	#endregion
	
}
