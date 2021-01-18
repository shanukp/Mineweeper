using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Game.Core;

namespace Game.Utility
{

	public class ObjectPool
	{
		#region Member Variables

		private readonly GameObject _objectPrefab;
		private readonly List<PoolObject> _poolObjects = new List<PoolObject>();
		private readonly Transform _parent;
		private readonly PoolBehaviour _poolBehaviour = PoolBehaviour.GameObject;

		#endregion

		#region Enums

		public enum PoolBehaviour
		{
			GameObject,
			CanvasGroup
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Initializes a new instance of the ObjectPooler class.
		/// </summary>
		public ObjectPool(GameObject objectPrefab, int initialSize, Transform parent = null, PoolBehaviour poolBehaviour = PoolBehaviour.GameObject, bool instantiateImmediately = true)
		{

			_objectPrefab = objectPrefab;
			_parent = parent;
			_poolBehaviour = poolBehaviour;

			if (instantiateImmediately)
			{
				for (int i = 0; i < initialSize; i++)
				{
					CreateObject();
				}
			}
			else
			{
				ServiceLocator.CoroutineManager.StartCoroutine(CreateObjects(initialSize));
			}
		}

		private IEnumerator CreateObjects(int size)
		{
			yield return null;

			for (int i = 0; i < size; ++i)
			{
				CreateObject();
				yield return null;
			}
		}

		public static Transform CreatePoolContainer(Transform containerParent, string name = "pool_container")
		{
			GameObject container = new GameObject(name);

			container.SetActive(false);
			container.transform.SetParent(containerParent);

			return container.transform;
		}

		public int GetCountInPool()
		{
			return _poolObjects.Count;
		}

		public static void ReturnObjectToPool(GameObject gameObject)
		{
			PoolObject poolObject = gameObject.GetComponent<PoolObject>();

			if (poolObject == null)
			{
				return;
			}

			poolObject.pool.ReturnObjectToPool(poolObject);
		}

		/// <summary>
		/// Returns an object, if there is no object that can be returned from instantiatedObjects then it creates a new one.
		/// Objects are returned to the pool by setting their active state to false.
		/// bool temp -> not used as of now - gives an indication whether initial pool was enough or not
		/// </summary>
		public GameObject GetObject()
		{
			bool temp;

			return GetObject(out temp);
		}

		/// <summary>
		/// Returns an object, if there is no object that can be returned from instantiatedObjects then it creates a new one.
		/// Objects are returned to the pool by setting their active state to false.
		/// </summary>
		public GameObject GetObject(out bool instantiated)
		{
			instantiated = false;

			PoolObject poolObject = null;

			for (int i = 0; i < _poolObjects.Count; i++)
			{
				if (_poolObjects[i].isInPool)
				{
					poolObject = _poolObjects[i];

					break;
				}
			}

			if (poolObject == null)
			{
				poolObject = CreateObject();
				instantiated = true;
			}

			switch (_poolBehaviour)
			{
				case PoolBehaviour.GameObject:
					poolObject.gameObject.SetActive(true);
					break;
				case PoolBehaviour.CanvasGroup:
					poolObject.canvasGroup.alpha = 1f;
					poolObject.canvasGroup.interactable = true;
					poolObject.canvasGroup.blocksRaycasts = true;
					break;
			}

			poolObject.isInPool = false;

			return poolObject.gameObject;
		}

		/// <summary>
		/// Returns an object, if there is no object that can be returned from instantiatedObjects then it creates a new one.
		/// Objects are returned to the pool by setting their active state to false.
		/// </summary>
		public GameObject GetObject(Transform parent)
		{
			bool temp;

			return GetObject(parent, out temp);
		}

		/// <summary>
		/// Returns an object, if there is no object that can be returned from instantiatedObjects then it creates a new one.
		/// Objects are returned to the pool by setting their active state to false.
		/// </summary>
		public GameObject GetObject(Transform parent, out bool instantiated)
		{
			GameObject obj = GetObject(out instantiated);

			obj.transform.SetParent(parent, false);

			return obj;
		}

		/// <summary>
		/// Returns an object, if there is no object that can be returned from instantiatedObjects then it creates a new one.
		/// Objects are returned to the pool by setting their active state to false.
		/// </summary>
		public T GetObject<T>(Transform parent) where T : Component
		{
			return GetObject(parent).GetComponent<T>();
		}

		/// <summary>
		/// Returns an object, if there is no object that can be returned from instantiatedObjects then it creates a new one.
		/// Objects are returned to the pool by setting their active state to false.
		/// </summary>
		public T GetObject<T>(Transform parent, out bool instantiated) where T : Component
		{
			return GetObject(parent, out instantiated).GetComponent<T>();
		}

		/// <summary>
		/// Returns an object, if there is no object that can be returned from instantiatedObjects then it creates a new one.
		/// Objects are returned to the pool by setting their active state to false.
		/// </summary>
		public T GetObject<T>() where T : Component
		{
			return GetObject().GetComponent<T>();
		}

		/// <summary>
		/// Returns an object, if there is no object that can be returned from instantiatedObjects then it creates a new one.
		/// Objects are returned to the pool by setting their active state to false.
		/// </summary>
		public T GetObject<T>(out bool instantiated) where T : Component
		{
			return GetObject(out instantiated).GetComponent<T>();
		}

		/// <summary>
		/// Sets all instantiated GameObjects to de-active
		/// </summary>
		public void ReturnAllObjectsToPool()
		{
			for (int i = 0; i < _poolObjects.Count; i++)
			{
				ReturnObjectToPool(_poolObjects[i]);
			}
		}

		/// <summary>
		/// Returns the object to pool.
		/// </summary>
		public void ReturnObjectToPool(PoolObject poolObject)
		{
			poolObject.transform.SetParent(_parent, false);

			switch (_poolBehaviour)
			{
				case PoolBehaviour.GameObject:
					poolObject.gameObject.SetActive(false);
					break;
				case PoolBehaviour.CanvasGroup:
					poolObject.canvasGroup.alpha = 0f;
					poolObject.canvasGroup.interactable = false;
					poolObject.canvasGroup.blocksRaycasts = false;
					break;
			}

			poolObject.isInPool = true;
		}

		/// <summary>
		/// Destroies all objects.
		/// </summary>
		public void DestroyAllObjects()
		{
			for (int i = 0; i < _poolObjects.Count; i++)
			{
				Object.Destroy(_poolObjects[i]);
			}

			_poolObjects.Clear();
		}

		#endregion

		#region Private Methods

		private PoolObject CreateObject()
		{
			GameObject obj = Object.Instantiate(_objectPrefab);

			PoolObject poolObject = obj.AddComponent<PoolObject>();

			poolObject.pool = this;

			if (_poolBehaviour == PoolBehaviour.CanvasGroup)
			{
				poolObject.canvasGroup = obj.GetComponent<CanvasGroup>();

				if (poolObject.canvasGroup == null)
				{
					poolObject.canvasGroup = obj.AddComponent<CanvasGroup>();
				}
			}

			_poolObjects.Add(poolObject);

			ReturnObjectToPool(poolObject);

			return poolObject;
		}

		#endregion
	}
}
