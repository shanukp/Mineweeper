using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    /// <summary>
    /// Uses Unity's Update loop to fire events each frame.
    /// A single Update loop is more efficient than multiple Update loops.
    /// </summary>
    public class UpdateManager : MonoBehaviour
    {

        public event Action OnUpdate;
        public event Action OnLateUpdate;

        private void Update()
        {
            if (OnUpdate != null)
            {
                OnUpdate();
            }
        }

        private void LateUpdate()
        {
            if (OnLateUpdate != null)
            {
                OnLateUpdate();
            }
        }
    }

}
