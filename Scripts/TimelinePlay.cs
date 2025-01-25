using System;
using UnityEngine.Playables;
using UnityEngine;

public class TimelinePlay : MonoBehaviour
{
	public PlayableDirector pd;

	public Camera camera;

	public GameObject gm;

	private bool flag;
	public void Start()
	{


	}

	public void Stop()
	{
		pd.Pause();
		DialogueSystem.instance.StartDialog("chapter0_opening");
	}

	public void Update()
	{
		if (!SaveManager.instance.savedata.firststartstory&& flag==false)
		{
			gm.SetActive(false);
			pd.Play();
			EventManager.instance.mainCharacter.m_Controls.GamePlayer.Disable();
			flag = true;
		}
		else if(SaveManager.instance.savedata.firststartstory)
		{
			base.gameObject.SetActive(false);
			camera.gameObject.SetActive(false);
		}
		if (DialogueSystem.instance.GetStatus() == Status.OFF)
		{
			pd.Play();
		}
	}

	public void ContinueChat()
	{
		pd.Pause();
		DialogueSystem.instance.StartDialog("chapter0_opening1");
		return;
	}

	public void Finish()
	{
		pd.Stop();
		gm.SetActive(true);
		base.gameObject.SetActive(false);
		camera.gameObject.SetActive(false);
		TipsUI.instance.StartTips("Tips.BasicMovement",3f);
		EventManager.instance.mainCharacter.m_Controls.GamePlayer.Enable();
		SaveManager.instance.savedata.firststartstory=true;

	}
}
