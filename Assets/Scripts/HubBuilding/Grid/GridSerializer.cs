using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Utilities;

// Not sure how we will go about save and load so just keeping this class open to change 
// Making it a monobehavior works too, but gonna leave it for now since we dont need for the pitch 
namespace HubBuilding
{
    public static class GridSerializer 
    {
        public const string GridJsonSavePath = "TempSave/grid_entries.json";
        
        private static GridEntryList RuntimeEntries = new()
        {
            entries = new List<GridEntry>()
        };
        
        [Serializable]
        public class GridEntryList
        {
            public List<GridEntry> entries;
        }
        
        public static void AddEntry(GridEntry entry)
        {
           
            RuntimeEntries.entries.Add(entry);
            Debug.Log($"New Entry Added: ID: {entry.itemId}, Position: {entry.position.x}, {entry.position.y}");
        }

        public static void TestSave()
        {
            string directory = Path.Combine(Application.persistentDataPath, "TempSave");
            string filePath = Path.Combine(directory, "grid_entries.json");

            Directory.CreateDirectory(directory);

            var data = new GridEntryList { entries = RuntimeEntries.entries };
            string json = JsonUtility.ToJson(data, true);

            File.WriteAllText(filePath, json);
            Debug.Log($"Saved grid to: {filePath}\n{json}");
        }

        public static GridEntryList TestLoad()
        {
            RuntimeEntries.entries.Clear();
            string directory = Path.Combine(Application.persistentDataPath, "TempSave");
            string filePath = Path.Combine(directory, "grid_entries.json");

            return JsonUtility.FromJson<GridEntryList>(filePath); 
        }

        // If we want to handle the save and load elsewhere 
        public static string GetJson()
        {
            return JsonUtility.ToJson(RuntimeEntries);
        }
    }
}