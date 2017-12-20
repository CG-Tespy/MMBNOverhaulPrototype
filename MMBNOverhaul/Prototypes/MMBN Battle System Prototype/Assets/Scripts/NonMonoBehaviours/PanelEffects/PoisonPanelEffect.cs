using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class PoisonPanelEffect : PanelEffect
{

	float damagePerSecond = 3f; 

	public override void Execute()
	{
		base.Execute();

		//Debug.Log("Executing poison panel effect!");

		// check if there's anything standing on the current panel
		Ray upwards = new Ray(controller.transform.position, controller.transform.up);
		RaycastHit[] hits = Physics.RaycastAll(upwards, 5f);

		LivingEntityController livingThing;
		RaycastHit currentHit;

		// if there is, damage it with poison damage. 
		for (int i = 0; i < hits.Length; i++)
		{
			currentHit = hits[i];
			livingThing = currentHit.transform.GetComponent<LivingEntityController>();

			if (livingThing != null)
				livingThing.TakeDamage(damagePerSecond * Time.deltaTime, DamageType.poisonPanel);
		}

	}

	
}
