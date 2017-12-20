using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class CrackedPanelEffect : PanelEffect
{
	bool wasSteppedOn = false;

	public override void Execute()
	{
		// check if there is anything above this panel
		Ray above = new Ray(controller.transform.position, controller.transform.up);
		RaycastHit[] hits = Physics.RaycastAll(above, 5);

		LivingEntityController livingThing = null;
		bool foundLivingThing = false;

		for (int i = 0; i < hits.Length; i++)
		{
			livingThing = hits[i].transform.GetComponent<LivingEntityController>();

			if (livingThing != null)
			{
				wasSteppedOn = true;
				foundLivingThing = true;
				break;
			}
		}

		if (wasSteppedOn && !foundLivingThing)
		{
			// apparently something stepped off of this panel, so it's time to break it
			PanelInfo brokenPanel = PanelDatabase.instance.GetPanel("Broken Panel");
			controller.ChangeTo(brokenPanel);
		}
	}
	
}
