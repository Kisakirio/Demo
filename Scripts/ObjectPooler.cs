using System;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;

	public class ObjectPooler:MonoBehaviour
	{
		public static ObjectPooler instance;

		public List<GameObject> poolobject;

		public List<List<GameObject>> poolobjectlist;

		[SerializeField]
		private GameObject WeakHit;

		[SerializeField]
		private GameObject StrongHit;

		[SerializeField]
		private GameObject StarGlow;

		[SerializeField]
		private AudioSource audio_prefab;

		[SerializeField]
		private GameObject StrongH;

		[SerializeField]
		private GameObject Bullet;

		[SerializeField]
		private GameObject Smog;

		[SerializeField]
		private GameObject Death;

	public void Awake()
	{
		if (instance == null)
		{
			instance = this;
			poolobjectlist = new List<List<GameObject>>();
			poolobject = new List<GameObject>();
		}
	}


	public AudioSource PlaySound(AllSound ID, Vector3 pos, float pitch = 1f, float volume = 1f)
	{

		AudioSource audioSource = CreateAudio();
		audioSource.clip = SoundList.Instance.GetSound((int)ID);
		audioSource.panStereo = 0f;
		audioSource.pitch = ((pitch > 0f) ? pitch : 1f);
		audioSource.volume = volume;
		audioSource.Play();
		return audioSource;
	}


	public Transform CreateWeakHit()
	{
		return PoolManager.Pools["WeakHit"].Spawn(WeakHit.transform);
	}
	public Transform CreateStrongHit()
	{
		return PoolManager.Pools["StrongHit"].Spawn(StrongHit.transform);
	}

	public Transform CreateStarGlow()
	{
		return PoolManager.Pools["StarGlow"].Spawn(StarGlow.transform);
	}

	public AudioSource CreateAudio()
	{
		return PoolManager.Pools["Audio"].Spawn(audio_prefab);
	}

	public Transform CreateStrongH()
	{
		return PoolManager.Pools["StrongH"].Spawn(StrongH.transform);
	}

	public BulletScripts CreateBullet()
	{
		return PoolManager.Pools["Bullet"].Spawn(Bullet.transform).GetComponent<BulletScripts>();
	}
	public Transform CreateSmog()
	{
		return PoolManager.Pools["Smog"].Spawn(Smog.transform);
	}

	public EnemyDeath CreateEnemyDeath()
	{
		return PoolManager.Pools["Death"].Spawn(Death.transform).GetComponent<EnemyDeath>();
	}

	}
