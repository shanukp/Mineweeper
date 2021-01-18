using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utility
{
    public class ObjectPoolHolder : SingletonComponent<ObjectPoolHolder>
    {
        private ObjectPool cellPool;

        [SerializeField] private GameObject cellPrefab;

        public void CreateCellPool()
        {
            cellPool = new ObjectPool(cellPrefab, 5, ObjectPool.CreatePoolContainer(transform, "cell_item_pool"), ObjectPool.PoolBehaviour.GameObject, false);
        }

        public ObjectPool GetCellPool()
        {
            return cellPool;
        }
    }
}
