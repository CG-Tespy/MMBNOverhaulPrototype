using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TriggerEvent : UnityEvent<Collider> {}
public class CollisionEvent : UnityEvent<Collision> {}

public class MonoBehaviourEvents
{
	public TriggerEvent OnTriggerEnter 	{ get; protected set; }
	public TriggerEvent OnTriggerStay 	{ get; protected set; }
	public TriggerEvent OnTriggerExit 	{ get; protected set; }
	public CollisionEvent OnCollisionEnter 	{ get; protected set; }
	public CollisionEvent OnCollisionStay 	{ get; protected set; }
	public CollisionEvent OnCollisionExit 	{ get; protected set; }

	// TODO: Add the other events all MonoBehaviours have


	public MonoBehaviourEvents()
	{
		OnTriggerEnter = new TriggerEvent();
		OnTriggerStay = new TriggerEvent();
		OnTriggerExit = new TriggerEvent();

		OnCollisionEnter = new CollisionEvent();
		OnCollisionStay = new CollisionEvent();
		OnCollisionExit = new CollisionEvent();
	}
}


public abstract class ObservableMonoBehaviour : MonoBehaviour 
{
	/*
	Lets other objects subscribe to and respond to the built-in Monobehaviour events 
	of this class.
	 */

	public MonoBehaviourEvents mBEvents = new MonoBehaviourEvents();
	new public Transform transform { get; protected set; }
	new public Renderer renderer   { get; protected set; }

	protected virtual void Awake()
	{
		this.transform = GetComponent<Transform>();
		renderer = GetComponent<Renderer>();
	}

	protected virtual void Start()
	{}

	protected virtual void EarlyUpdate()
	{}

	protected virtual void Update()
	{}

	protected virtual void LateUpdate()
	{}

	protected virtual void FixedUpdate()
	{}


	// TODO: Add all the methods that fire the events

	public virtual void OnTriggerEnter(Collider other)
	{
		mBEvents.OnTriggerEnter.Invoke(other);
	}

	public virtual void OnTriggerStay(Collider other)
	{
		mBEvents.OnTriggerStay.Invoke(other);
	}

	public virtual void OnTriggerExit(Collider other)
	{
		mBEvents.OnTriggerExit.Invoke(other);
	}

	public virtual void OnCollisionEnter(Collision other)
	{
		mBEvents.OnCollisionEnter.Invoke(other);
	}

	public virtual void OnCollisionStay(Collision other)
	{
		mBEvents.OnCollisionStay.Invoke(other);
	}

	public virtual void OnCollisionExit(Collision other)
	{
		mBEvents.OnCollisionExit.Invoke(other);
	}
	
}
