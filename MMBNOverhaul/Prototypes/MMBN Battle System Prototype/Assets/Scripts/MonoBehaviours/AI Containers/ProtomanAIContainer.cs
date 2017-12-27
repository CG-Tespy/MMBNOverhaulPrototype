using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ProtomanAIContainer : EnemyAIContainer
{

	ProtomanAI protomanAI = new ProtomanAI();
	public override EnemyAI ai { get { return protomanAI; } }
	
}
