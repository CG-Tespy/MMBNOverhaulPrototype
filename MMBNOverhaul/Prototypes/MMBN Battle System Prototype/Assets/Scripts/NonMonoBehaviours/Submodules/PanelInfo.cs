using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

//public class PanelTypeEvent : UnityEvent<PanelType> {}
public class PanelEvent : UnityEvent<PanelInfo> {}

[System.Serializable]
public class PanelInfo : IBattlefieldPanel, System.IEquatable<PanelInfo>
{

	#region Fields
	[SerializeField] public string _name = "";
	[SerializeField] int _id = 0;
	[SerializeField] public string _description = "";
	[SerializeField] PanelType _type = PanelType.normal;
	[SerializeField] bool _traversable = true;
	[SerializeField] Material _material = null;
	[SerializeField] PanelEffectContainer effectContainer = null;	
	PanelEffect _effect = null;

	PanelController rep = null;
	// ^ Represents this panel as a game object in the scene


	#endregion
	
	#region Properties

	#region For Backing Fields
	public string name 
	{
		get { return _name; }
		set { _name = value; }
	}

	public int id 
	{
		get { return _id; }
	}

	public string description
	{
		get { return _description; }
		set { _description = value; }
	}
	public Material material
	{
		get { return _material; }
		set { _material = value; }
	}
	public PanelType type
	{
		get { return _type; }
		set { _type = value; }
	}

	public bool traversable
	{
		get { return _traversable; }
		set { _traversable = value; }
	}

	public PanelEffect effect 
	{
		get { return _effect; }
		set { _effect = value; }
	}

	#endregion
	#endregion

	#region Methods

	#region Initialization
	public PanelInfo()
	{
		
	}



	public void Init(PanelController rep)
	{
		this.rep = rep;
		RegisterEffect();
	}

	#endregion

	public bool Equals(PanelInfo other)
	{
		return other.id == this.id;
	}

	public PanelInfo Copy()
	{
		/*
		PanelInfo copy = new PanelInfo(this);
		return copy;
		*/
		PanelInfo copy = new PanelInfo();
		copy.name = 				string.Copy(this.name);
		copy._id = 					this._id;
		copy.description = 			string.Copy(this.description);
		copy.material = 			this.material;
		copy.traversable = 			this.traversable;
		copy.type = 				this.type;
		
		if (effectContainer != null)
		{
			string effContainerPath = 	"Prefabs/";
			effContainerPath += 		this.effectContainer.name;

			GameObject effContainerPrefab;
			effContainerPrefab = 		MonoBehaviour.Instantiate<GameObject>(this.effectContainer.gameObject);
			copy.effectContainer = 		effContainerPrefab.GetComponent<PanelEffectContainer>();
		}

		return copy;
	}


	void RegisterEffect()
	{
		if (effectContainer != null)
		{
			effect = effectContainer.effect;
			effect.Init(rep);

			// we won't need this prefab taking up space anymore
			MonoBehaviour.Destroy(effectContainer.gameObject);
			//Debug.Log("This panel registered the effect " + effect.ToString());
		}

		
	}

	#endregion
	
}
