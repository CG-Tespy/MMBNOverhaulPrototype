using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HealthbarController : MonoBehaviour, IPausable
{
	public UnityEvent Paused { get; protected set; }
	public UnityEvent Unpaused { get; protected set; }
	public LivingEntityController entity;

	[SerializeField] Text healthText;
	[SerializeField] Image frame;

	int healthToDisplay = 0;

	[Tooltip("Affects how fast the numbers scroll. Higher value means slower scrolling.")]
	[Range(1, 100)]
	public float updateTime = 1f;

	[Tooltip("Color when the entity isn't being hurt.")]
	public Color normalColor = Color.white;
	[Tooltip("Color when the entity gets hurt.")]
	public Color hurtColor = Color.red;
	[Tooltip("Color when the entity gets healed.")]
	public Color healColor = Color.green;

	IEnumerator healthScrollCoroutine;
	bool gotHurtThisFrame = false;
	bool gotHealedThisFrame = false;

	public bool isPaused { get; protected set; }

	GameController controller;
	int displayedHealth 
	{
		get { return System.Convert.ToInt32(healthText.text); }
		set { healthText.text = value.ToString(); }
	}

	int entityCurrentHealth
	{
		get { return Mathf.CeilToInt(entity.effectiveHealth); }
	}

	void Awake()
	{
		isPaused = false;
		healthToDisplay = displayedHealth;
	}
	// Use this for initialization
	void Start () 
	{
		controller = GameController.instance;
		healthScrollCoroutine = HealthScroll();
		StartCoroutine(healthScrollCoroutine);
		WatchForHealthChanges();
		WatchForGamePause();
		
	}
	

	// Update is called once per frame
	void Update () 
	{
		
	}

	void LateUpdate()
	{
		if (!isPaused)
		{
			if (!gotHurtThisFrame && !gotHealedThisFrame)
				PlayerNormalColorChange();

			gotHurtThisFrame = false;
			gotHealedThisFrame = false;
		}

	}

	IEnumerator HealthScroll()
	{
		float timer = 0f;
		float framesToPass = (controller.frameRate * updateTime); 
		float rawHealthToDisplay;

		int healthAnchor = displayedHealth;
		// ^ Helps keep the scrolling at a pretty consistent speed

		int healthDiff = 0;

		bool lerpingDown = false;
		bool lerpingUp = false;

		while (true)
		{
			if (!isPaused)
			{
				healthDiff = (int)Mathf.Abs(entityCurrentHealth - displayedHealth);
				lerpingDown = displayedHealth > entityCurrentHealth;
				lerpingUp = displayedHealth < entityCurrentHealth;

				if (lerpingDown)
					PlayerHurtColorChange();
				else if (lerpingUp)
					PlayerHealedColorChange();
					
				if (healthDiff <= 10 )
				{
					healthToDisplay = entityCurrentHealth;
					healthAnchor = displayedHealth;
					// ^ So the next time the player's HP changes, the scrolling can start again 
					// smoothly 
					timer = 0;
					
				}
				
				else
				{
					rawHealthToDisplay = Mathf.Lerp(healthAnchor, entityCurrentHealth, timer / framesToPass);

					// make health lerping go faster if there is small-enough difference between the 
					// displayed health and actual health
					
					if (healthDiff > 0 && healthDiff <= 100)
						rawHealthToDisplay += Mathf.Sign(entityCurrentHealth - displayedHealth);
					
					// don't let displayed health go into the negatives
					healthToDisplay = Mathf.Max(Mathf.CeilToInt(rawHealthToDisplay), 0);

					timer++;
				}
					
				displayedHealth = healthToDisplay;
			}
			yield return new WaitForEndOfFrame();
		}

	}

	void PlayerHurtColorChange()
	{
		healthText.color = hurtColor;
		gotHurtThisFrame = true;
	}

	void PlayerHealedColorChange()
	{
		healthText.color = healColor;
		gotHealedThisFrame = true;
	}

	void PlayerNormalColorChange()
	{
		healthText.color = normalColor;
		gotHurtThisFrame = false;
		gotHealedThisFrame = false;
	}

	#region IPausable
	public void Pause()
	{
		isPaused = true;
	}

	public void Unpause()
	{
		isPaused = false;
	}

	#endregion

	/// <summary>
	/// Makes the health numbers change colors when the entity gets hurt or healed.
	/// </summary>
	void WatchForHealthChanges()
	{
		entity.TookDamage.AddListener( PlayerHurtColorChange);
		entity.TookHealing.AddListener( PlayerHealedColorChange );
	}

	void WatchForGamePause()
	{
		GameController.instance.Paused.AddListener(Pause);
		GameController.instance.Unpaused.AddListener(Unpause);
	}
	
}
