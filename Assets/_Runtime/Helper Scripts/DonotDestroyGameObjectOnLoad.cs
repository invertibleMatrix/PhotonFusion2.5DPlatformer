using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UM.EngineExtensions
{
    public class DonotDestroyGameObjectOnLoad : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}