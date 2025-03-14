using System.Collections.Generic;
using UnityUtilities.DelaunayVoronoi;
using UnityUtilities.PoissonSampling;
using UnityEngine;

namespace bismarck_redux
{
    /// <summary>
    /// World generation functions.
    /// </summary>
    public class WorldGen
    {
        /// <summary>
        /// Construct a new world generator.
        /// </summary>
        /// <param name="bounds"></param>
        /// <param name="radius"></param>
        public WorldGen()
        {
            
        }

        public List<Province> Generate(Rect bounds, float radius)
        {
            /* Partition the area into city points */
            List<Vector2> poispoints = PoissonSampler.PoissonDiscSample(bounds, radius, 30) as List<Vector2>;
        
            /* Generate a triangulator */
            DelaunayTriangulator triangulator = new DelaunayTriangulator();
            var points = triangulator.GeneratePoints(poispoints, bounds);
            var tris = triangulator.BowyerWatson(points);

            /* Generate province borders */
            Voronoi v = new Voronoi();
            var voronoiFaces = v.GenerateEdgesFromDelaunayFaces(tris);
            
            /* Now, generate a province for each city point */
            Dictionary<Point, Province> provs = new Dictionary<Point, Province>();
            foreach (var city in points)
            {
                Vector2 nc = new Vector2((float)city.X, (float)city.Y);
                provs.Add(city, new Province(nc));
            }
            
            /* Assign borders */
            foreach (var city in provs.Keys)
            {
                /* Coarse assignment */
                Province p = provs[city];
                foreach (var edge in voronoiFaces[city])
                {
                    Vector2 a = new Vector2((float)edge.Point1.X, (float)edge.Point1.Y);
                    Vector2 b = new Vector2((float)edge.Point2.X, (float)edge.Point2.Y);
                    if (!p.Borders.Contains(a)) p.Borders.Add(a);
                    if (!p.Borders.Contains(b)) p.Borders.Add(b);
                }
                
                /* Clean up (order them) */
                p.Borders = OrderVerts(p.Borders);
            }
            
            /* Update neighbors */
            foreach (var triangle in tris)
            {
                Province a = provs[triangle.Vertices[0]];
                Province b = provs[triangle.Vertices[1]];
                Province c = provs[triangle.Vertices[2]];

                if (!a.Neighbors.Contains(b)) a.Neighbors.Add(b);
                if (!a.Neighbors.Contains(c)) a.Neighbors.Add(c);
                if (!b.Neighbors.Contains(a)) b.Neighbors.Add(a);
                if (!b.Neighbors.Contains(c)) b.Neighbors.Add(c);
                if (!c.Neighbors.Contains(a)) c.Neighbors.Add(a);
                if (!c.Neighbors.Contains(b)) c.Neighbors.Add(b);
            }
            
            /* Return the provinces */
            List<Province> ret = new List<Province>();
            foreach (var p in provs.Values)
            {
                ret.Add(p);
            }
            
            /* Cull away any provinces which have borders outside of the map */
            List<Province> byebye = new List<Province>();
            foreach (var province in ret)
            {
                foreach (var vert in province.Borders)
                {
                    if (!bounds.Contains(vert))
                    {
                        byebye.Add(province);
                        break;
                    }
                }
            }

            foreach (var bad in byebye)
            {
                bad.CleanupProvince();
                ret.Remove(bad);
            }

            return ret;
        }

        /// <summary>
        /// Order the provided verts in CCW order.
        /// </summary>
        /// <param name="verts"></param>
        private static List<Vector2> OrderVerts(List<Vector2> verts)
        {
            /* First compute the centroid */
            var centroid = Vector2.zero;
            foreach(var vert in verts) {
                centroid += vert;
            }
            centroid /= verts.Count * 1.0f;

            /* Compute the relative angle of all verts to the centroid */
            List<(Vector2, float)> angles = new List<(Vector2, float)>();
            foreach (var vert in verts)
            {
                var angle = Mathf.Atan2(vert.y - centroid.y, vert.x - centroid.x);
                angles.Add((vert, angle));
            }

            /* Sort based on angle */
            angles.Sort((a, b) => b.Item2.CompareTo(a.Item2)); //backwards because unity uses a clockwise winding order, so sort from high to low

            /* Generate an array to return */
            List<Vector2> ret = new List<Vector2>();
            foreach (var set in angles)
            {
                ret.Add(set.Item1);
            }
            return ret;
        }
    }
}