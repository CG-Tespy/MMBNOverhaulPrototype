using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public class PanelDatabase : MonoBehaviour 
{
	[SerializeField] List<PanelInfo> panels = new List<PanelInfo>();
	public static PanelDatabase instance;

	void Awake()
	{
		instance = this;
		EnforceUniqueIDs();
	}
	
	void EnforceUniqueIDs()
	{
		List<PanelInfo> panelsCopy = panels.OrderBy(panel => panel.id).ToList();

		PanelInfo currentPanel, previousPanel;
		for (int i = 1; i < panelsCopy.Count; i++)
		{
			currentPanel = panelsCopy[i];
			previousPanel = panelsCopy[i - 1];
			if (currentPanel.id == previousPanel.id)
			{
				string messageFormat = "Panels {0} and {1} have the same ID. Please make sure all panels in the ";
				messageFormat = string.Concat(messageFormat, "panel database have distinct IDs.");
				throw new System.ArgumentException(string.Format(messageFormat, 
																currentPanel.name, 
																previousPanel.name));
			}
		}
	}

	public PanelInfo GetPanel(int id)
	{
		PanelInfo panelGotten = (from p in panels
								where p.id == id 
								select p).First();

		if (panelGotten != null)
		{
			PanelInfo panelCopy = panelGotten.Copy();
			return panelCopy;
		}
		else
			throw new System.ArgumentException("There is no panel with id " + id + " in the panel database.");
	}

	/// <summary>
	/// Searches the database for a panel with the passed name. Be careful not to have 
	/// any panels share names.
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	public PanelInfo GetPanel(string name)
	{
		PanelInfo panelGotten = (from p in panels
								where p.name == name 
								select p).First();

		if (panelGotten != null)
			return panelGotten.Copy();
		else
			throw new System.ArgumentException("There is no panel with the name " + name + " in the panel database.");
	}

	public string GetPanelNameById(int id)
	{
		PanelInfo panelGotten = (from p in panels
								where p.id == id
								select p).First();

		if (panelGotten != null)
			return panelGotten.name;
		else
			throw new System.ArgumentException("There is no panel with id " + id + " in the panel database.");
	}
}
