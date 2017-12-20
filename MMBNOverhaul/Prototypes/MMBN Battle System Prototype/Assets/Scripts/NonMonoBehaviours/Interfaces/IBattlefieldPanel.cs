using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public interface IBattlefieldPanel
{
	bool traversable { get; }
	Material material { get; }
	PanelType type { get; }
	PanelEffect effect { get; }

	
}
