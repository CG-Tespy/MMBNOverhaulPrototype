using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HealthbarController : MonoBehaviour 
{
	public LivingEntityController entity;

	[SerializeField] Text healthText;
	[SerializeField] Image frame;

	int healthToDisplay = 0;

	[Tooltip("Affects how fast the numbers scroll. Higher value means slower scrolling.")]
	[Range(1, 100)]
	public float updateTime = 1f;

	IEnumerator healthScrollCoroutine;

	GameController controller;
	int displayedHealth 
	{
		get { return System.Convert.ToInt32(healthText.text); }
		set { healthText.text = value.ToString(); }
	}

	int entityCurrentHealth
	{
		get { return (int) entity.effectiveHealth; }
	}

	// Use this for initialization
	void Start () 
	{
		controller = GameController.instance;
		healthScrollCoroutine = HealthScroll();
		StartCoroutine(healthScrollCoroutine);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	IEnumerator HealthScroll()
	{
		float timer = 0f;
		float framesToPass = (controller.frameRate * updateTime * updateTime ); 

		while (true)
		{
			
			if (Mathf.Abs(displayedHealth - entityCurrentHealth) <= 2 )
			{
				healthToDisplay = entityCurrentHealth;
				timer = 0;
			}
			
			else
			{
				healthToDisplay = (int)( Mathf.Sign(entityCurrentHealth - displayedHealth) + Mathf.Lerp(displayedHealth, entityCurrentHealth, timer / framesToPass));
				// ^ Mathf.Sign part is to avoid a weird effect where the healthbar can get close to 
				// the target value, but slows down way too much before getting there. 
				timer++;
			}
				
			displayedHealth = healthToDisplay;
			yield return null;
		}

	}
	
}
