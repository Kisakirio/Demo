using UnityEngine;

public class SoundList : MonoBehaviour
{
	public static SoundList Instance;

	[SerializeField]
	private AudioClip[] soundlist;

	public AudioClip GetSound(int id)
	{
		if (id >= soundlist.Length)
		{
			return null;
		}
		return soundlist[id];
	}

	[ContextMenu("listSound")]
	public void ListSound()
	{
		for (int i = 0; i < soundlist.Length; i++)
		{
			Debug.Log(i + " : " + ((AllSound)i).ToString() + " : " + ((soundlist[i] == null) ? "null" : soundlist[i].name));
		}
	}

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}
}
