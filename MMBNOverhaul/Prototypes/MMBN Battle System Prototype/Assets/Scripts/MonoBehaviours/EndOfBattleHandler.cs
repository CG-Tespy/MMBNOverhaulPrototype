using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EndOfBattleHandler : MonoBehaviour 
{
	[SerializeField] GameObject gameOverText;
	[SerializeField] GameObject victoryText;
	[SerializeField] string gameStartScene;
	[SerializeField] string nextSceneToLoad;
	[SerializeField] int enemiesInField;
	public List<EnemyController> enemies;
	GameController gameController;


	// Use this for initialization
	void Start () 
	{
		gameController = GameController.instance;
		enemiesInField = enemies.Count;

		foreach (EnemyController enemy in enemies)
			enemy.mBEvents.Disable.AddListener(OnEnemyDeath);

		gameController.navi.mBEvents.Disable.AddListener(OnPlayerDeath);
	}

	void OnEnemyDeath()
	{
		enemiesInField--;

		if (enemiesInField <= 0)
			VictorySequence(nextSceneToLoad);
	}

	void OnPlayerDeath()
	{
		GameOverSequence(gameStartScene);

	}

	void Update()
	{
		foreach (EnemyController enemy in enemies)
			if (enemy == null)
				enemies.Remove(enemy);
	}

	void VictorySequence(string sceneName)
	{
		victoryText.SetActive(true);
		StartCoroutine(ToNextScene(sceneName));
	}

	void GameOverSequence(string sceneName)
	{
		gameOverText.SetActive(true);
		StartCoroutine(ToNextScene(sceneName));
	}

	IEnumerator ToNextScene(string sceneName)
	{
		
		yield return new WaitForSeconds(3f);

		// load the next scene
		SceneManager.LoadScene(sceneName);
	}
	
}
