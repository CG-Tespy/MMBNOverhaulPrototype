using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public abstract class PanelEffect
{
	protected GameController gameController;
	protected PanelDatabase panelDatabase;
	protected PanelController panel;

	public virtual bool isActive { get; set; }

	public virtual void Init(PanelController panel)
	{
		this.panel = panel;
		gameController = GameController.instance;
		panelDatabase = PanelDatabase.instance;
	}

	public virtual void Execute()
	{
		if (!isActive)
			return;
	}
	
}
