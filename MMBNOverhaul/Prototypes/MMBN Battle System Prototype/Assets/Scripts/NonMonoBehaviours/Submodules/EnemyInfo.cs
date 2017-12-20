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
	[SerializeField] float _moveSpeed;
	[SerializeField] float _sightRange = 5f;
	[SerializeField] float _sightAngle = 50f;

	[SerializeField] int _contactDamage = 1;
	[SerializeField] float _pushbackForce;
	[SerializeField] Vector3 _extraPushback;
	[SerializeField] Sprite _sprite = null;
	[SerializeField] Mesh _mesh = null;

	#endregion

	#endregion

	#region Properties

	#region For Backing Fields
	public float moveSpeed { get { return _moveSpeed; } }
	public float sightRange { get { return _sightRange; } }
	public float sightAngle { get { return _sightAngle; } }

	public int contactDamage
	{
		get { return _contactDamage; }
		protected set { _contactDamage = value; }
	}

	public float pushbackForce
	{
		get { return _pushbackForce; }
		protected set { _pushbackForce = value; }
	}

	public Vector3 extraPushback 
	{
		get { return _extraPushback; }
		protected set { _extraPushback = value; }
	}
	public Sprite sprite 
	{
		get { return _sprite; }
	}

	public Mesh mesh 
	{
		get { return _mesh; }
	}

	#endregion

	#endregion

	#region Methods
    public EnemyInfo() : base() {}

	public bool Equals(EnemyInfo other)
	{
		bool bothNullSprites = this.sprite == null && other.sprite == null;
		bool bothNullMeshes = this.mesh == null && other.mesh == null;
		
		bool sameSprites = bothNullSprites || (this.sprite.name == other.sprite.name);
		bool sameMeshes = bothNullMeshes || (this.mesh.name == other.mesh.name);
		bool sameGraphics = sameSprites && sameMeshes;

		bool sameMaxHealth = this.maxHealth == other.maxHealth;
		bool sameInvincibilityTime = this.invincibilityTime == other.invincibilityTime;

		return (sameGraphics && sameMaxHealth && sameInvincibilityTime);

	}

	#endregion
}
