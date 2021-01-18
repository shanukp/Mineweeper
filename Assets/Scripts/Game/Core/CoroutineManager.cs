using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    /// <summary>
    /// Allows non-MonoBehaviour objects to run Coroutines
    /// </summary>
    public class CoroutineManager : MonoBehaviour
    {

        /// <summary>
        /// Starts a new Coroutine
        /// </summary>
        public new Coroutine StartCoroutine(IEnumerator coroutine)
        {
            return base.StartCoroutine(coroutine);
        }

        /// <summary>
        /// Stops an existing Coroutine
        /// </summary>
        public new void StopCoroutine(Coroutine coroutine)
        {
            if (coroutine != null)
            {
                base.StopCoroutine(coroutine);
            }
        }
    }
}
