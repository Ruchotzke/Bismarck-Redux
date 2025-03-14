using System;
using System.Collections;
using System.Collections.Generic;
using bismarck_redux;
using UnityUtilities.DelaunayVoronoi;
using UnityEngine;
using UnityUtilities.General;
using UnityUtilities.Meshing;
using Random = UnityEngine.Random;

public class TestRun : MonoBehaviour
{
    private List<Province> _provinces;

    public MeshFilter pf_Mesh;
    
    // Start is called before the first frame update
    void Start()
    {
        WorldGen wg = new WorldGen();
        _provinces = wg.Generate(new Rect(0.0f, 0.0f, 30.0f, 20.0f), 1.0f);
        
        /* Generate a mesh for each province */
        foreach (var province in _provinces)
        {
            var instance = Instantiate(pf_Mesh);

            Mesher mesher = new Mesher();
            Color provColor = Color.HSVToRGB(Random.Range(0.0f, 1.0f), 1.0f, 1.0f, true);
            float height = Random.Range(0.0f, .5f);
            
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
