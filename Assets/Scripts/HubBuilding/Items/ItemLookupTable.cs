using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace HubBuilding
{
    public class ItemLookupTable : MonoBehaviour
    {
        public static ItemLookupTable Instance {get; private set; }
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
        
        public SO_HubItem GetHubItem(string id)
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
