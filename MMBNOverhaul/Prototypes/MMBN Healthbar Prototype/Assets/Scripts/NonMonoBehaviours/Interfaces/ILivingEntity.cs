using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public interface ILivingEntity
{
	string name { get; }
	string description { get; }
	LivingEntityStats stats { get; }
	int id { get; }
	bool isInvincible { get; }
	bool isDead { get; }

	void TakeDamage(int amount);

	void TakeHealing(int amount);

	void Revive(int amount);
	void Revive(float healthPercent);

	void Die();	
}
