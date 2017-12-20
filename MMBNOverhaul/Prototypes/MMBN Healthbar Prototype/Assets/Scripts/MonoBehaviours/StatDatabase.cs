using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class StatDatabase : MonoBehaviour 
{
	/* 
	Where you can set the stats that are in the game by default, as well as 
	what their default paramaters are. 
	*/
	public static StatDatabase instance;
	public Stat[] stats;

	void Awake()
	{
		instance = this;
	}
	
}
