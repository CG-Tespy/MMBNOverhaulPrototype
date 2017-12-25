using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Shockwave : AttackBehaviour 
{

	[SerializeField] Material _shockwaveMaterial = null;
	public Material shockwaveMaterial
	{
		get { return _shockwaveMaterial; }
		protected set { _shockwaveMaterial = value; }
	}

	Material previousPanelMaterial = null;
	PanelController panelPreviouslyOn = null;

	Dictionary<PanelController, Material> panelMats = new Dictionary<PanelController, Material>();
	Dictionary<PanelController, Texture> panelTextures = new Dictionary<PanelController, Texture>();

	protected override void FixedUpdate()
	{
		base.FixedUpdate();

		// shockwaves can't travel over holes or nonexistent space, so...
		if (panelCurrentlyOn == null || panelCurrentlyOn.type == PanelType.broken)
		{
			Destroy(this.gameObject);
			return;
		}

		ApplyShockwaveEffect();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		panelPreviouslyOn.material = panelMats[panelPreviouslyOn];
	}

	void ApplyShockwaveEffect()
	{
		bool applyEffect = panelCurrentlyOn.material != shockwaveMaterial;

		if (applyEffect)
		{
			// log the panel's original material so we can switch it back when 
			// this shockwave moves onto a new panel. Otherwise the shockwave will
			// just paint everything yellow
			if (!panelMats.ContainsKey(panelCurrentlyOn))
			{
				panelMats.Add(panelCurrentlyOn, panelCurrentlyOn.material);
				panelCurrentlyOn.material = shockwaveMaterial;
			}
		}

		/*
		// can't really see the values and keys in the debugger, so yeah
		if (panelMats.Values.Count >= 3)
		{
			List<Material> values = new List<Material>(panelMats.Values);
			List<PanelController> keys = new List<PanelController>(panelMats.Keys);
			Debug.Log("");
		}
		*/
		
		bool movedToNewPanel = panelPreviouslyOn != null && panelPreviouslyOn != panelCurrentlyOn;

		if (movedToNewPanel)
		{
			Debug.Log(this.name + ": Moved to new panel!");
			panelPreviouslyOn.material = panelMats[panelPreviouslyOn];
			panelMats.Remove(panelPreviouslyOn);
		}

		panelPreviouslyOn = panelCurrentlyOn;


	}

	protected override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);
	}
}
