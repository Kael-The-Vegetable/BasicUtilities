using UnityEngine;

namespace BasicUtilities
{
	/// <summary>
	/// Replace the inhered <see cref="MonoBehaviour"/> with <see cref="AnnexSingleton{T}"/>.
	/// <br />
	/// Use this if you need global data that exists between scenes and when a scene that holds an existing <see cref="AnnexSingleton{T}"/> comes along, that singleton takes over the role of singleton. Use <seealso cref="Singleton{T}"/> if you do not need the script to be available between scenes or <seealso cref="PersistentSingleton{T}"/> if you do not need new objects to take over the old ones.
	/// </summary>
	/// <typeparam name="T">T refers to the class that you desire to inherent <see cref="PersistentSingleton{T}"/>.</typeparam>
	public abstract class AnnexSingleton<T> : MonoBehaviour where T : Component
	{
		#region Properties
		[field: SerializeField] public bool AutoUnparentOnAwake { get; protected set; } = true;

		protected static T instance;
		public static bool HasInstance => instance != null;
		public static T TryGetInstance() => HasInstance ? instance : null;

		/// <summary>
		/// Use this to access non-static methods and properties.
		/// </summary>
		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindAnyObjectByType<T>();
					if (instance == null)
					{
						var go = new GameObject(typeof(T).Name + " Auto-Generated");
						instance = go.AddComponent<T>();
					}
				}

				return instance;
			}
		}
		#endregion

		private void Awake()
		{
			if (!Application.isPlaying) return;

			if (AutoUnparentOnAwake)
			{
				transform.SetParent(null);
			}

			if (instance != this)
			{
				Destroy(instance.gameObject);
			}

			instance = this as T;
			DontDestroyOnLoad(gameObject);
			Initialize();
		}

		/// <summary>
		/// Use this method for initialization on <see cref="Awake"/>.
		/// </summary>
		protected abstract void Initialize();
	}
}