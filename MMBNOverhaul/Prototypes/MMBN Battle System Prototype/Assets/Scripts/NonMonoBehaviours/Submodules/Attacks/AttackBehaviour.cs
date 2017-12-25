using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AttackBehaviour : ObservableMonoBehaviour 
{

	[SerializeField] float _damage = 				10;
	[SerializeField] DamageType _damageType;
	[SerializeField] float _moveSpeed = 			5f;
	[SerializeField] float _duration = 				-1f;
	[SerializeField] Combatant _onSideOf;

	[SerializeField] AnimationClip[] _actionAnimations;
	[SerializeField] AnimationClip[] _destructionAnimations;

	protected List<LivingEntityController> entitiesDamaged = new List<LivingEntityController>();

	#region Properties
	#region Basic components
	public BoxCollider boxCollider { get; protected set; }
	public Animator animator { get; protected set; }
	new public Rigidbody rigidbody { get; protected set; }
	#endregion
	#region For Backing Fields
	public float damage 
	{
		get { return _damage; }
		set { _damage = value; }
	}

	public DamageType damageType 
	{
		get { return _damageType; }
		set { _damageType = value; }
	}

	public float moveSpeed 
	{
		get { return _moveSpeed; }
		set { _moveSpeed = value; }
	}

	public float duration 
	{
		get { return _duration; }
		protected set { _duration = value; }
	}

	public Combatant onSideOf 
	{
		get { return _onSideOf; }
		set { _onSideOf = value; }
	}

	public AnimationClip[] actionAnimations 
	{
		get { return _actionAnimations; }
		protected set { _actionAnimations = value; }
	}

	public AnimationClip[] destructionAnimations
	{
		get { return _destructionAnimations; }
		protected set { _destructionAnimations = value; }
	}

	#endregion
	public bool hasLimitedDuration
	{
		get { return _duration == -1f; }
	}

	public virtual PanelController panelCurrentlyOn { get; protected set; }

	#endregion

	protected override void Awake()
	{
		base.Awake();
		boxCollider = 		GetComponent<BoxCollider>();
		animator = 			GetComponent<Animator>();
		rigidbody = 		GetComponent<Rigidbody>();
	}

	public virtual void Init(LivingEntityController spawner)
	{
		this.onSideOf = spawner.onSideOf;

		if (onSideOf == Combatant.enemy)
		{
			// make the attack face the player's side of the field
			Vector3 reversedScale = transform.localScale;
			reversedScale.x = -reversedScale.x;
			transform.localScale = reversedScale;
		}

		panelCurrentlyOn = GetPanelCurrentlyOn();
	}

	protected override void FixedUpdate()
	{
		panelCurrentlyOn = GetPanelCurrentlyOn();
	}

	protected override void LateUpdate()
	{
		base.LateUpdate();
		entitiesDamaged.Clear();
	}

	public virtual void Move(Vector3 dir)
	{
		rigidbody.velocity = dir.normalized * moveSpeed;
	}

	protected override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);

		LivingEntityController livingEntity = other.gameObject.GetComponent<LivingEntityController>();
		
		// only hurt the target if it wasn't hurt already by this attack in this frame
		if (livingEntity.onSideOf != this.onSideOf && !entitiesDamaged.Contains(livingEntity))
		{
			livingEntity.TakeDamage(damage, damageType);
			entitiesDamaged.Add(livingEntity);
		}

	}

	protected PanelController GetPanelCurrentlyOn()
	{
		Ray down = new Ray(transform.position, -transform.up);
		RaycastHit[] hits = Physics.RaycastAll(down, 5f);

		PanelController panel = null;

		foreach (RaycastHit hit in hits)
		{
			panel = hit.transform.GetComponent<PanelController>();

			if (panel != null)
				break;
		}

		/* Attacks are allowed to be over nonexistent space, so yeah.
		if (panel == null)
			throw new System.NullReferenceException(name + " is for some reason not on a panel.");
		*/
		return panel;
	}

	
}
