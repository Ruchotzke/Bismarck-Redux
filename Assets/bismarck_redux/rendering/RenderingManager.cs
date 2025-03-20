using System;
using bismarck_redux.world;
using UnityEngine;
using UnityUtilities.General;
using UnityUtilities.Meshing;
using Random =  UnityEngine.Random; 

namespace bismarck_redux.rendering
{
    /// <summary>
    /// The manager responsible for rendering the world.
    /// </summary>
    public class RenderingManager : MonoBehaviour
    {
        /// <summary>
        /// The singleton instance of this manager.
        /// </summary>
        public static RenderingManager Instance;

        /// <summary>
        /// The mesh prefab for generated provinces.
        /// </summary>
        public MeshFilter pf_Mesh;

        private void Awake()
        {
            /* Singleton setup */
            if (Instance != null) Destroy(this.gameObject);
            Instance = this;
        }

        private void Start()
        {
            /* Get a handle to the world */
            var worldManager = WorldManager.Instance;
             
            /* Render each province */
            foreach (var province in worldManager.provinces)
            {
                var instance = Instantiate(pf_Mesh, transform);
                instance.gameObject.name = province.CityCenter.ToString();

                Mesher mesher = new Mesher(false);
                Color provColor = Color.HSVToRGB(Random.Range(0.0f, 1.0f), 1.0f, 1.0f, true);
                float height = province.CityCenter.y;
                
                /* Triangulate the top */
                for (int i = 1; i < province.Borders.Count - 1; i++)
                {
                    mesher.AddTriangle(province.Borders[0].ToVector3(height), province.Borders[i].ToVector3(height),
                        province.Borders[i + 1].ToVector3(height), provColor);
                }
                
                /* Triangulate the sides */
                for (int i = 0; i < province.Borders.Count - 1; i++)
                {
                    mesher.AddQuad(province.Borders[i+1].ToVector3(height), province.Borders[i].ToVector3(height),
                        province.Borders[i].ToVector3(0.0f), province.Borders[i+1].ToVector3(0.0f), provColor);
                }
                mesher.AddQuad(province.Borders[0].ToVector3(height), province.Borders[^1].ToVector3(height),
                    province.Borders[^1].ToVector3(0.0f), province.Borders[0].ToVector3(0.0f), provColor);
    
                instance.mesh = mesher.GenerateMesh();
            }
        }
    }
}