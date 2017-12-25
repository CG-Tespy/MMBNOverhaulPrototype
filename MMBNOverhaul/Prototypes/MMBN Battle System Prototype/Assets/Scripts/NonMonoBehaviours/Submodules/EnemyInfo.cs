using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class EnemyInfo : LivingEntityInfo, System.IEquatable<EnemyInfo>
{

	#region Fields

	#region Backing Fields

	[SerializeField] int _contactDamage = 1;
	[SerializeField] Sprite _sprite = null;
	[SerializeField] Mesh _mesh = null;


	[SerializeField] protected EnemyAIContainer _aiContainer;
	protected EnemyAI aI; 

	#endregion

	#endregion

	#region Properties

	#region For Backing Fields

	public int contactDamage
	{
		get { return _contactDamage; }
		protected set { _contactDamage = value; }
	}

	public Sprite sprite 
	{
		get { return _sprite; }
	}

	public Mesh mesh 
	{
		get { return _mesh; }
	}

	public EnemyAIContainer aiContainer 
	{
		get { return _aiContainer; }
		protected set { _aiContainer = value; }
	}

	#endregion

	#endregion

	#region Methods
    public EnemyInfo() : base() {}


	public bool Equals(EnemyInfo other)
	{
		return (this.id == other.id);
	}

	public EnemyInfo Copy()
	{
		EnemyInfo copy = 			new EnemyInfo();
		copy._contactDamage = 		this._contactDamage;

		copy._name = 				string.Copy(this._name);
		copy._description = 		this._description;

		copy._id = 					this._id;
		copy._invincibilityTime = 	this._invincibilityTime;
		
		
		copy._stats = 				new LivingEntityStats(this.stats);
		copy._resistances = 		new List<DamageType>(this._resistances);
		copy._weaknesses = 			new List<DamageType>(this._weaknesses);

		copy._sprite = 				this.sprite;
		copy._mesh = 				this._mesh;
		copy.animations = 			this.animations;

		copy.aiContainer = 			this.aiContainer;

		return copy;
		
	}

	#endregion
}
