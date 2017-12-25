using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Helps you simply plug in panel effects into panelInfos in the inspector.
/// </summary>
[System.Serializable]
public abstract class PanelEffectContainer : MonoBehaviour
{
	public virtual PanelEffect effect { get; protected set; }

	protected virtual void Awake() {}
}
