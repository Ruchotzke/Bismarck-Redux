using System.Collections;
using System.Collections.Generic;
using bismarck_redux.population;
using UnityEngine;

namespace bismarck_redux{
    
    /// <summary>
    /// A single province on the map.
    /// </summary>
    public class Province
    {
        /// <summary>
        /// The location of a city center.
        /// </summary>
        public Vector3 CityCenter;

        /// <summary>
        /// Neighboring provinces.
        /// </summary>
        public List<Province> Neighbors;

        /// <summary>
        /// The borders of this province.
        /// </summary>
        public List<Vector2> Borders;

        /// <summary>
        /// The pops contained within this province.
        /// </summary>
        public PopContainer Pops;

        public Province(Vector3 city)
        {
            CityCenter = city;
            Neighbors = new List<Province>();
            Borders = new List<Vector2>();
            Pops = new PopContainer();
        }

        /// <summary>
        /// A helper used to delete references to this province.
        /// </summary>
        public void CleanupProvince()
        {
            foreach (var neighbor in Neighbors)
            {
                neighbor.Neighbors.Remove(this);
            }
        }
    }
}

