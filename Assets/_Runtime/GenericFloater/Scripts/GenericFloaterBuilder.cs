using System.Collections.Generic;
using UnityEngine;

namespace Views
{
    public static class GenericFloaterBuilder
    {
        private static List<GenericFloater> _pool = new List<GenericFloater>();
        private static GenericFloater       GenericFloater;
        private static int                  _spawnedObjectsCount;

        public static void Show(string message, float startingPosition = 0)
        {
            Init();

            if (_pool.Count > 0)
            {
                _pool[0].gameObject.SetActive(true);
                _pool[0].Init(startingPosition, message);
                _pool.RemoveAt(0);
            }

            else if (_spawnedObjectsCount <= 5)
            {
                var floater = MonoBehaviour.Instantiate(GenericFloater);
                floater.Init(startingPosition, message);
                _spawnedObjectsCount++;
            }
        }

        public static void AddToPool(GenericFloater floater)
        {
            _pool.Add(floater);
            floater.gameObject.SetActive(false);
        }

        private static void Init()
        {
            if (GenericFloater == null)
            {
                GenericFloater = Resources.Load<GenericFloater>("GenericFloater");
            }
        }
    }
}