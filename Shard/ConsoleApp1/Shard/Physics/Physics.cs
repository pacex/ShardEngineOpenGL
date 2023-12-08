using OpenTK.Mathematics;
using Shard.Shard.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.Physics
{
    class Physics
    {
        // Singleton
        private static Physics physics = null;

        public static Physics GetInstance() { 
            if (physics == null)
                physics = new Physics();
            return physics;
        }

        // Properties
        private List<Collider>[,,] staticColliders;
        private List<Collider> allStaticColliders;
        private Box3 bounds;
        private Vector3i cellCounts;

        private Physics() { }

        // Initializes and clears static body grid
        public void Initialize(Box3 bounds, Vector3 cellSize)
        {
            this.bounds = bounds;
            cellCounts = (Vector3i)(bounds.Size / cellSize);
            staticColliders = new List<Collider>[cellCounts.X, cellCounts.Y, cellCounts.Z];
            allStaticColliders = new List<Collider>();
        }

        public void AddStatic(Collider collider)
        {
            Box3i cells = getIntersectingCells(collider.TranslatedBounds());
            for(int i = cells.Min.X; i <= cells.Max.X; i++)
            {
                for (int j = cells.Min.Y; j <= cells.Max.Y; j++)
                {
                    for (int k = cells.Min.Z; k <= cells.Max.Z; k++)
                    {
                        if (staticColliders[i, j, k] == null)
                            staticColliders[i, j, k] = new List<Collider>();
                        staticColliders[i,j,k].Add(collider);
                    }
                }
            }
            allStaticColliders.Add(collider);
        }

        public void AddStaticMesh(Mesh mesh, Vector3 offset)
        {
            List<Collider> colliders = mesh.ExportTriangleColliders(offset);
            foreach(Collider c in colliders)
            {
                AddStatic(c);
            }
        }

        public bool IntersectsStatic(Collider collider)
        {
            Box3i cells = getIntersectingCells(collider.TranslatedBounds());
                    
            foreach(Collider c in getStaticBodies(cells))
            {
                if (c.Intersects(collider))
                    return true;
            }

            return false;
        }

        public Vector3 ResponseStatic(Collider collider)
        {
            Box3i cells = getIntersectingCells(collider.TranslatedBounds());
            Vector3 response = Vector3.Zero;

            List<Collider> l = getStaticBodies(cells);
            //Console.WriteLine(l.Count);

            int n = 0;
            foreach (Collider c in l)
            {
                Vector3 r = c.Response(collider);
                
                if (r.LengthSquared > 0.0f)
                    n++;

                response += r;
            }
            return n > 1 ? response / (float)n : response;
        }

        private List<Collider> getStaticBodies(Box3i cells)
        {
            List<Collider> r = new List<Collider>();
            for (int i = cells.Min.X; i <= cells.Max.X; i++)
            {
                for (int j = cells.Min.Y; j <= cells.Max.Y; j++)
                {
                    for (int k = cells.Min.Z; k <= cells.Max.Z; k++)
                    {
                        if (staticColliders[i, j, k] == null)
                            continue;

                        r.AddRange(staticColliders[i, j, k]);
                    }
                }
            }
            return r;
        }

        private Box3i getIntersectingCells(Box3 b)
        {
            return new Box3i(getIntersectingCell(b.Min), getIntersectingCell(b.Max));
        }

        private Vector3i getIntersectingCell(Vector3 p)
        {
            Vector3i nonClamped = (Vector3i)(((p - bounds.Min) / bounds.Size) * (Vector3)cellCounts);
            return new Vector3i(Math.Clamp(nonClamped.X, 0, cellCounts.X - 1), Math.Clamp(nonClamped.Y, 0, cellCounts.Y - 1), Math.Clamp(nonClamped.Z, 0, cellCounts.Z - 1));
        }



        public void DebugDraw()
        {
            foreach(Collider c in allStaticColliders)
            {
                c.Draw(Color4.Green);
            }
        }

    }
}
