using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CrackedPanel : PanelEffectContainer
{

	CrackedPanelEffect eff = new CrackedPanelEffect();

	public override PanelEffect effect { get { return eff; } }
	
}
