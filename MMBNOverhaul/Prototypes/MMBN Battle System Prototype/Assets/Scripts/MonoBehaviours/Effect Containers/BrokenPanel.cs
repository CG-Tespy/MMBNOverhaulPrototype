using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BrokenPanel : PanelEffectContainer
{
	BrokenPanelEffect eff = new BrokenPanelEffect();

	public override PanelEffect effect { get { return eff; } }
	
}
