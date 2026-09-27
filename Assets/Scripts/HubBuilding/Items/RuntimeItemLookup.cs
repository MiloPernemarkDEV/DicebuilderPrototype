using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace HubBuilding
{
    public class RuntimeItemLookup : MonoBehaviour
    {
        public static RuntimeItemLookup Instance {get; private set; }
        [SerializeField] private List<SO_HubItem> hubItems = new List<SO_HubItem>();
    
        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        public SO_HubItem GetHubItem(int id)
        {
            foreach (var item in hubItems)
            {
                return item.ID == id ? item : null;
            }
            Debug.Log("Can't find item " + id);
            return null; 
        }
    }
}
