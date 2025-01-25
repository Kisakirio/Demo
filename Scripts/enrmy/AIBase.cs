
	public abstract class AIBase
	{
		public EnemyController enc;

		public float TILESIZE=2f;

		public CharacterBase player = EventManager.instance.mainCharacter;



		public abstract Type type { get; }

		public virtual void Init()
		{

		}

		public virtual void AI()
		{

		}



	}

