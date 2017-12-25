using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class EnemyDatabase : MonoBehaviour 
{
	public static EnemyDatabase instance;

	[SerializeField] List<EnemyInfo> enemies = new List<EnemyInfo>();

	void Awake()
	{
		instance = this;
		EnforceUniqueIDs();
		InitializeEnemies();
	}

	public EnemyInfo GetEnemyInfo(int id)
	{
		
		for (int i = 0; i < enemies.Count; i++)
			if (enemies[i].id == id)
				return enemies[i].Copy();
		

		throw new System.ArgumentException("There is no enemy with the id " + id + " in the enemy database.");
	}

	public EnemyInfo GetEnemyInfo(string name)
	{
		for (int i = 0; i < enemies.Count; i++)
			if (enemies[i].name == name)
				return enemies[i].Copy();
		

		throw new System.ArgumentException("There is no enemy with the name " + name + " in the enemy database.");
	}

	void EnforceUniqueIDs()
	{
		EnemyInfo currentEnemy = null;
		EnemyInfo previousEnemy = null;

		for (int i = 1; i < enemies.Count; i++)
		{
			currentEnemy = enemies[i];
			previousEnemy = enemies[i - 1];

			if (currentEnemy.Equals(previousEnemy))
			{
				string messageFormat = "{0} and {1} have the same id. Please make sure ";
				messageFormat += "all enemies in the database have unique IDs.";
				throw new System.ApplicationException(string.Format(messageFormat, 
																	currentEnemy.name, 
																	previousEnemy.name));
			}
				
		}
	}

	void InitializeEnemies()
	{
		for (int i = 0; i < enemies.Count; i++)
		{
			enemies[i].stats.Init();
		}
	}
	
}
