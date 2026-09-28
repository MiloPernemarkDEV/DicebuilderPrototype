using System.Collections.Generic;
using UnityEngine;

namespace HubBuilding
{
    public class ItemLookup : MonoBehaviour
    {
        public static ItemLookup Instance{get; private set;}
        
        [SerializeField] private List<SO_HubItem> items;

        public void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public SO_HubItem Get(string id)
        {
            foreach (var item in items)
            {
                if (item.ID == id)
                {
                    return item; 
                }
            }
            return null; 
        }
    }
}