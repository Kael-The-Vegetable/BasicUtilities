using UnityEngine;
using UnityEngine.Pool;

namespace BasicUtilities
{
	public interface IPoolable<T> where T : Component
	{
		public MonoObjectPool<T> PoolAccess { get; set; }
	}

	public class MonoObjectPool<T> where T : Component
	{
		private readonly T _prefab;
		private readonly Transform _poolParent;

		private int _max;
		private bool _allowingMaxPlus;
		public ObjectPool<T> Pool { get; }

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
		public T Get()
		{
			if (_allowingMaxPlus || Pool.CountActive < _max)
			{
				return Pool.Get();
			}
			return default;
		}
		public void Release(T entity) => Pool.Release(entity);
		public void Clear() => Pool.Clear();
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