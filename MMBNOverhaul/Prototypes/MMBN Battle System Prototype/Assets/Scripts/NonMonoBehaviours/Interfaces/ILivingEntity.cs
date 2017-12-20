using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public interface ILivingEntity
{
	UnityEvent Died { get; }
	UnityEvent Revived { get; }
	UnityEvent TookDamage { get; }
	UnityEvent TookHealing { get; }
	
	string name { get; }
	string description { get; }
	LivingEntityStats stats { get; }
	List<DamageType> resistances { get; }
	List<DamageType> weaknesses { get; }
	int id { get; }
	bool isInvincible { get; }
	bool isDead { get; }

	bool TakeDamage(float amount, DamageType type);

	bool TakeHealing(float amount);

	bool Revive(int amount);
	bool Revive(float healthPercent);

	bool Die();	
}
