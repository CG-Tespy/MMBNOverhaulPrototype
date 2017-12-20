using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public static class Controls 
{
	[System.Serializable]
	public class BattleControls
	{
		public static BattleControls instance;

		public BattleControls()
		{
			instance = this;
		}
		public KeyCode moveUp = KeyCode.W;
		public KeyCode moveLeft = KeyCode.A;
		public KeyCode moveDown = KeyCode.S;
		public KeyCode moveRight = KeyCode.D;

		public KeyCode useChip = KeyCode.Mouse0;

	}

	[System.Serializable]
	public class GeneralControls 
	{
		public static GeneralControls instance;
		public GeneralControls()
		{
			instance = this;
		}
		public KeyCode pauseGame = KeyCode.T;
	}
}
public class ControlsHandler : MonoBehaviour 
{
	
	public static ControlsHandler instance;
	public Controls.GeneralControls generalControls;
	public Controls.BattleControls battleControls;

	void Awake()
	{
		instance = this;
	}
	
}
