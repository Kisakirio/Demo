using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuSelect : MonoBehaviour
{
	[SerializeField]
	public List<MenuSelect> menuList;

	public PlayControl ui_control;


	public int currentLine;


	private void Awake()
	{
		ui_control = new PlayControl();
		ui_control.UI.up.started += ctx =>
		{
			currentLine--;
			if (currentLine <= 0)
			{

				currentLine = 0;
			}
		};
		ui_control.UI.down.started += ctx =>
		{
			currentLine++;
			if (currentLine > 1)
			{
				currentLine = 1;
			}
		};
		ui_control.UI.Enter.started += ctx =>
		{
			if (currentLine == 0)
			{
				GameManager.instace.ChangeScene("main");
			}

			if (currentLine == 1)
			{
				DoExit();
			}
		};

	}

	private void DoExit()
	{
		Application.Quit();
	}

	private void Update()
	{
		for (int i = 0; i < menuList.Count; i++)
		{
			if (currentLine == i)
			{
				menuList[i].UpdateMe(true);
			}
			else
			{
				menuList[i].UpdateMe(false);
			}

		}


	}

	private void OnEnable()
	{
		ui_control.UI.Enable();
	}

	private void OnDisable()
	{
		ui_control.UI.Disable();
	}
}
