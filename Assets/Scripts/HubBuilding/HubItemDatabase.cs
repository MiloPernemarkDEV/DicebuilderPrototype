using System.Collections.Generic;
using UnityEngine;

namespace HubBuilding
{
    public class HubItemDatabase : MonoBehaviour
    {
        public static HubItemDatabase Instance {get; private set; }
    
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
    
        [SerializeField] private List<SO_HubItem> HubItems = new List<SO_HubItem>();

        public SO_HubItem GetHubItem(string id)
        {
            foreach (var item in HubItems)
            {
                return item.ID == id ? item : null;
            }
            return null; 
        }
    }
}
