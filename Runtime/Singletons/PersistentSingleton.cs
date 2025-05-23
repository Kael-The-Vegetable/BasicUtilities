using UnityEngine;

namespace BasicUtilities
{
	/// <summary>
	/// Replace the inhered <see cref="MonoBehaviour"/> with <see cref="PersistentSingleton{T}"/>.
	/// <br />
	/// Use this if you need global data that exists between scenes. Use <seealso cref="Singleton{T}"/> if you do not need the script to be available between scenes.
	/// </summary>
	/// <typeparam name="T">T refers to the class that you desire to inherent <see cref="PersistentSingleton{T}"/>.</typeparam>
	public abstract class PersistentSingleton<T> : MonoBehaviour where T : Component
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

			if (instance == null)
			{
				instance = this as T;
				DontDestroyOnLoad(gameObject);
			}
			else if (instance != this)
			{
				Destroy(gameObject);
				return;
			}
			Initialize();
		}

		/// <summary>
		/// Use this method for initialization on <see cref="Awake"/>.
		/// </summary>
		protected abstract void Initialize();
	}
}