using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BattlefieldManager : MonoBehaviour 
{
	public static BattlefieldManager instance;
	[Tooltip("Dimensions of the battlefield grid")]
	[SerializeField] Vector2 _dimensions;
	[Tooltip("Drag all panels in battlefield here")]
	[SerializeField] List<PanelController> panels = new List<PanelController>();
	public PanelController[,] panelGrid { get; protected set; }

	public Vector2 dimensions { get { return _dimensions; } }


	void Awake()
	{
		instance = this;
		SetupPanelGrid();
	}

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	/// <summary>
	/// Returns a panel positioned one away from the passed one, based on the passed direction.
	/// </summary>
	/// <param name="panel"></param>
	/// <param name="dir"></param>
	/// <returns></returns>
	public PanelController GetPanelRelativeTo(PanelController panel, Direction dir)
	{
		Vector2 panelGridPos = panel.posOnGrid;
		Vector2 resultGridPos = panelGridPos;

		bool noPanel = false;

		switch (dir)
		{
			case Direction.up:
				noPanel = panelGridPos.y == dimensions.y - 1;

				if (noPanel)
					return null;
				else 
					resultGridPos.y += 1;
					break;

			case Direction.down:
				noPanel = panelGridPos.y == 0;

				if (noPanel)
					return null;
				else 
					resultGridPos.y -= 1;
					break;

			case Direction.left:
				noPanel = panelGridPos.x == 0;

				if (noPanel)
					return null;
				else 
					resultGridPos.x -= 1;
					break;

			case Direction.right:
				noPanel = panelGridPos.x == dimensions.x - 1;

				if (noPanel)
					return null;
				else 
					resultGridPos.x += 1;
					break;

		}

		return panelGrid[ (int) resultGridPos.x, (int) resultGridPos.y];

	}

	public void ApplyPanelEffectTo(PanelController panel, PanelEffect effect)
	{
		panel.effect = effect;
		if (effect == null)
			Debug.LogWarning("Applied a null PanelEffect to " + panel.name + ", thus making it have no effect.");
	}

	public void ApplyPanelEffectToAll(PanelEffect effect)
	{
		foreach (PanelController panel in panels)
			ApplyPanelEffectTo(panel, effect);
	}

	#region Helper funcs

	void SetupPanelGrid()
	{
		panelGrid = new PanelController[(int)dimensions.x, (int)dimensions.y];

		Vector2 panelPos; 
		bool posTaken = false;

		foreach (PanelController panel in panels)
		{
			panelPos = panel.posOnGrid;
			posTaken = panelGrid[(int)panelPos.x, (int)panelPos.y] != null;

			// see if this panel's pos on the grid is already taken. If so, you need to 
			// double-check and fix the panel positions on the inspector
			if (posTaken)
			{
				string message = "Two or more panels in the battlefield prefab share a position. ";
				message += "\nPlease double-check and make sure none of the panels share a position on the grid.";

				throw new System.InvalidOperationException(message);
			}
			else 
				panelGrid[ (int) panelPos.x, (int) panelPos.y] = panel;
			
		}
	}

	#endregion
	
}
