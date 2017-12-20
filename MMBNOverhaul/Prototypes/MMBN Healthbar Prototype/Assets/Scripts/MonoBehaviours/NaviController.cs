using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class NaviController : LivingEntityController
{
	[SerializeField] NaviInfo _naviInfo;

	override public LivingEntityInfo entityInfo 
	{
		get { return _naviInfo as LivingEntityInfo; }
	}

	protected override void Awake()
	{
		base.Awake();
	}
	// Use this for initialization
	protected override void Start () 
	{
		base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () 
	{
		
	}
	
}
