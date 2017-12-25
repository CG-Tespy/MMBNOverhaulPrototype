using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HammerAIContainer : EnemyAIContainer
{

	HammerAI hammerAI = new HammerAI();

	public override EnemyAI ai { get { return hammerAI; } }
	
}
