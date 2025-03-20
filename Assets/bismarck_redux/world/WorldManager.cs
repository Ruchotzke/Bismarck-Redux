using System;
using System.Collections.Generic;
using bismarck_redux.population;
using UnityEngine;

namespace bismarck_redux.world
{
    /// <summary>
    /// The collection of provinces and information making up the world.
    /// </summary>
    public class WorldManager : MonoBehaviour
    {
        
        /// <summary>
        /// The instance of this manager.
        /// </summary>
        public static WorldManager Instance;
        
        /// <summary>
        /// All provinces in this game world.
        /// </summary>
        public List<Province> provinces = new List<Province>();

        private void Awake()
        {
            /* Handle the singleton */
            if (Instance != null) Destroy(this.gameObject);
            Instance = this;
            
            /* Generate the world */
            WorldGen wg = new WorldGen();
            provinces = wg.Generate(new Rect(0.0f, 0.0f, 30.0f, 20.0f), 1.0f);
            Debug.Log("Generated " + provinces.Count + " provinces");
            
            /* Initialize population */
            var template = new Pop()
            {
                Size = 30.0,
            };
            
        }
    }
}