using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public abstract class PanelEffect
{
	protected GameController gameController;
	protected PanelController controller;

	public virtual bool isActive { get; set; }

	public virtual void Init(PanelController controller)
	{
		this.controller = controller;
		gameController = GameController.instance;
	}

	public virtual void Execute()
	{
		if (!isActive)
			return;
	}
	
}
