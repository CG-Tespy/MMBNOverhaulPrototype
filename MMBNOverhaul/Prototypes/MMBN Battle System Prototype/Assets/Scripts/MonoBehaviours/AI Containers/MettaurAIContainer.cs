using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MettaurAIContainer : EnemyAIContainer
{
	MettaurAI mettaurAI = new MettaurAI();

	public override EnemyAI ai { get { return mettaurAI; } }
	
}
