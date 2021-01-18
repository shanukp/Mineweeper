using UnityEngine;
using System.Collections;

namespace Game.Utility
{
	/// <summary>
	/// Gets a static instance of the Component that extends this class and makes it accessible through the Instance property.
	/// </summary>
	public class SingletonComponent<T> : MonoBehaviour where T : Object
	{
		#region Member Variables

		private static T _instance;

		private bool _isInitialized;

		#endregion

		#region Properties

		public static T Instance
		{
			get
			{
				// If the instance is null then either Instance was called to early or this object is not active.
				if (_instance == null)
				{
					_instance = GameObject.FindObjectOfType<T>();
				}

				if (_instance == null)
				{
					Debug.LogWarningFormat("[SingletonComponent] Returning null instance for component of type {0}.", typeof(T));
				}

				return _instance;
			}
		}

		#endregion

		#region Unity Methods

		protected virtual void Awake()
		{
			SetInstance();
		}

		#endregion

		#region Public Methods

		public static bool Exists()
		{
			return _instance != null;
		}

		public bool SetInstance()
		{
			if (_instance != null && _instance != gameObject.GetComponent<T>())
			{
				Debug.LogWarning("[SingletonComponent] Instance already set for type " + typeof(T));
				return false;
			}

			_instance = gameObject.GetComponent<T>();

			return true;
		}

		#endregion
	}
}
