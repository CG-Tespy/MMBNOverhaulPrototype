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

	public UnityEvent TookDamage { get; protected set; }
	public UnityEvent TookHealing { get; protected set; }

	#endregion

	#region Fields

	#region Backing Fields
	[SerializeField] protected string _name;
	[SerializeField] protected int _id;
	[SerializeField] protected string _description;
	[SerializeField] protected List<DamageType> _resistances;
	[SerializeField] protected List<DamageType> _weaknesses;
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
	public int id
	{
		get { return _id; }
	}
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

	public List<DamageType> resistances 
	{
		get { return _resistances; }
		set { _resistances = value; }
	}

	public List<DamageType> weaknesses 
	{
		get {return _weaknesses; }
		set { _weaknesses = value; }
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
	public float health 
	{
		get { return stats.health.val; }
		protected set { stats.health.val = (float)value; }
	}

	public float effectiveHealth
	{
		get { return stats.health.effectiveVal; }
		protected set { stats.health.effectiveVal = (float)value; }
	}

	public float maxHealth 
	{
		get { return stats.health.maxVal; }
		protected set { stats.health.maxVal = (float)value; }
	}

	public float effectiveMaxHealth 
	{
		get { return stats.health.effectiveMaxVal; }
		protected set { stats.health.effectiveMaxVal = (float)value; }
	}
	#endregion
	

	#endregion

	#region Methods

	public LivingEntityInfo()
	{
		Died = 			new UnityEvent();
		Revived =  		new UnityEvent();
		TookDamage = 	new UnityEvent();
		TookHealing = 	new UnityEvent();
	}

    public virtual bool Die()
	{
		health = 0;
		effectiveHealth = health;
		Died.Invoke();

		return true;
	}

    public virtual bool Revive(float revPercent)
	{
		health = (maxHealth * revPercent);
		effectiveHealth = health;
		Revived.Invoke();
		return true;
	}

    public virtual bool Revive(int healthLeft)
    {
        health = healthLeft;
		effectiveHealth = health;
		Revived.Invoke();

		return true;
    }

    public virtual bool TakeDamage(float amount, DamageType type = DamageType.none)
    {
        if (!isDead && !isInvincible)
		{
			float amountToTake = amount;

			if (resistances.Contains(type))
				amountToTake *= 0.5f;
			else if (weaknesses.Contains(type))
				amountToTake *= 2f;

			//Debug.Log("Took " + amount + " damage this frame.");
			health -= amount;
			effectiveHealth -= amount;

			isInvincible = true;

			TookDamage.Invoke();

			return true;
		}

		return false;
    }

    public bool TakeHealing(float amount)
    {
        if (!isDead)
		{
			health += amount;
			effectiveHealth += amount;
			TookHealing.Invoke();
			return true;
		}

		return false;
    }

	#endregion
	
}
