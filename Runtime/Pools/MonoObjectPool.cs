using UnityEngine;
using UnityEngine.Pool;

namespace BasicUtilities
{
	/// <summary>
	/// Add this interface onto a <see cref="MonoBehaviour"/> to access the parent <see cref="MonoObjectPool{T}"/>.
	/// </summary>
	/// <typeparam name="T">Type here should be equal to whatever type the parent Pool is.</typeparam>
	public interface IPoolable<T> where T : Component
	{
		/// <summary>
		/// Use this to access this object's pool.
		/// </summary>
		public MonoObjectPool<T> PoolAccess { get; set; }
	}

	/// <summary>
	/// This is a container class of an <seealso cref="ObjectPool{T}"/> that takes care of the basic required functions.
	/// </summary>
	/// <typeparam name="T">Type should be what you want to access when <see cref="Get"/>ing an object.</typeparam>
	public class MonoObjectPool<T> where T : Component
	{
		private readonly T _prefab;
		private readonly Transform _poolParent;

		private int _max;
		private bool _allowingMaxPlus;

		/// <summary>
		/// Use this to directly access the pool.
		/// </summary>
		public ObjectPool<T> Pool { get; }

		/// <summary>
		/// Construct a new pool to pull objects from.
		/// </summary>
		/// <param name="prefab">Prefab to be duplicated.</param>
		/// <param name="parent">Transform to parent objects in heirarchy.</param>
		/// <param name="size">Initial allocated size.</param>
		/// <param name="maxSize">Maximum allocated size.</param>
		/// <param name="allowMoreThanMax">Uncapped size.</param>
		public MonoObjectPool(T prefab, Transform parent, int size = 50, int maxSize = 1000, bool allowMoreThanMax = true)
		{
			_prefab = prefab;
			_poolParent = parent;
			_allowingMaxPlus = allowMoreThanMax;
			_max = maxSize;

			Pool = new ObjectPool<T>(
				CreatePooledObject, GetFromPool, ReturnToPool, DestroyPooledObject,
				true, size, maxSize);
		}

		#region Quick Funcs

		/// <summary>
		/// Use this to get an object.
		/// </summary>
		/// <returns>An object of type <typeparamref name="T"/> or the default if unable.</returns>
		public T Get()
		{
			if (_allowingMaxPlus || Pool.CountActive < _max)
			{
				return Pool.Get();
			}
			return default;
		}

		/// <summary>
		/// Use this to release a given entity.
		/// </summary>
		/// <param name="entity">The object desired to be released.</param>
		public void Release(T entity) => Pool.Release(entity);

		/// <summary>
		/// Clear the pool of all objects.
		/// </summary>
		public void Clear() => Pool.Clear();

		/// <summary>
		/// Dispose the pool to clean up.
		/// </summary>
		public void Dispose() => Pool.Dispose();
		#endregion

		#region Pool Funcs
		private T CreatePooledObject()
		{
			var obj = GameObject.Instantiate<T>(_prefab, new InstantiateParameters { parent = _poolParent });
			if (obj.TryGetComponent<IPoolable<T>>(out var poolable))
			{
				poolable.PoolAccess = this;
			}
			return obj;
		}
		private void GetFromPool(T pooledObject) => pooledObject.gameObject.SetActive(true);
		private void ReturnToPool(T pooledObject) => pooledObject.gameObject.SetActive(false);
		private void DestroyPooledObject(T pooledObject) => GameObject.Destroy(pooledObject);
		#endregion
	}
}