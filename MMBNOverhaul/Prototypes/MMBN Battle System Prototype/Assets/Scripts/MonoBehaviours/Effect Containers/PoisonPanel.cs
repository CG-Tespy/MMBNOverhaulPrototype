using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class PoisonPanel : PanelEffectContainer
{
	PoisonPanelEffect eff = new PoisonPanelEffect();

	public override PanelEffect effect 
	{
		get { return eff; }
	}

	public PoisonPanel()
	{
		if (eff == null)
			eff = new PoisonPanelEffect();
			
	}
	
}
