using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BrokenPanelEffect : PanelEffect 
{

	float duration = 10f;
	const float baseBlinkSpacing = 0.1f;
	float blinkSpacing; // time between each blink

	public override void Init(PanelController panel)
	{
		base.Init(panel);
		blinkSpacing = baseBlinkSpacing;
	}

	public override void Execute()
	{
		duration -= Time.deltaTime;

		if (duration <= 3f)
			MakePanelBlink();

		if (duration <= 0)
			ChangeToNormalPanel();
	}

	protected virtual void MakePanelBlink()
	{
		blinkSpacing -= Time.deltaTime;

		if (blinkSpacing <= 0)
		{
			panel.centerRenderer.enabled = !panel.centerRenderer.enabled;
			blinkSpacing = baseBlinkSpacing;
		}
	}

	protected virtual void ChangeToNormalPanel()
	{
		// make sure the panel's center is visible
		panel.centerRenderer.enabled = true;

		// make sure it doesn't still have that hole
		Material baseMaterial;
		string materialPath = "Materials/";

		if (panel.owner == Combatant.player)
			materialPath += "Blue";
		else 
			materialPath += "Red";
		
		baseMaterial = Resources.Load<Material>(materialPath);
		panel.material = baseMaterial;

		// get and apply the normal panel data
		PanelInfo normalPanel = panelDatabase.GetPanel("Normal Panel");
		panel.ChangeTo(normalPanel);
	}
}
