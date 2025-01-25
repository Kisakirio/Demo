using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager instace;

	public GameObject loadingprefab;

	private float loadTime;
	private void Awake()
	{
		if (instace == null)
		{
			instace = this;
			DontDestroyOnLoad(instace);
		}
	}

	public void ChangeScene(string name)
	{
		Instantiate(loadingprefab);
		StartCoroutine(ChangeSceneTask(name));

	}



	IEnumerator ChangeSceneTask(String name)
	{
		var asyncload = SceneManager.LoadSceneAsync(name);
		asyncload.allowSceneActivation = false;

		while(!asyncload.isDone)
		{
			if (asyncload.progress >= 0.9f)
			{
				while (loadTime<1f)
				{
					loadTime += Time.deltaTime;
					yield return null;
				}
				asyncload.allowSceneActivation = true;
			}

			yield return null;
		}
		yield return null;
	}
}
