using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class LivingEntityStats
{

	public Stat damage, health;

	public void Init()
	{
		damage.effectiveMaxVal = damage.maxVal;
		damage.effectiveMinVal = damage.minVal;
		damage.effectiveVal = damage.val;
		
		health.effectiveMaxVal = health.maxVal;
		health.effectiveMinVal = health.minVal;
		health.effectiveVal = health.val;
		
	}

	
}
