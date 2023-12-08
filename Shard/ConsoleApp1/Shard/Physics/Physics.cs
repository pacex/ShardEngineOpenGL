using OpenTK.Mathematics;
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
        private List<StaticBody>[,,] staticBodies;
        private Box3 bounds;
        private Vector3i cellCounts;

        private Physics() { }

        // Initializes and clears static body grid
        public void Initialize(Box3 bounds, Vector3 cellSize)
        {
            this.bounds = bounds;
            cellCounts = (Vector3i)(bounds.Size / cellSize);
            staticBodies = new List<StaticBody>[cellCounts.X, cellCounts.Y, cellCounts.Z];
        }

        public void AddStaticBody(StaticBody body)
        {
            Box3i cells = getIntersectingCells(body.Collider.TranslatedBounds());
            for(int i = cells.Min.X; i <= cells.Max.X; i++)
            {
                for (int j = cells.Min.Y; j <= cells.Max.Y; j++)
                {
                    for (int k = cells.Min.Z; k <= cells.Max.Z; k++)
                    {
                        if (staticBodies[i, j, k] == null)
                            staticBodies[i, j, k] = new List<StaticBody>();
                        staticBodies[i,j,k].Add(body);
                    }
                }
            }
        }

        public bool IntersectsStatic(Collider collider)
        {
            Box3i cells = getIntersectingCells(collider.TranslatedBounds());
                    
            foreach(StaticBody b in getStaticBodies(cells))
            {
                if (b.Collider.Intersects(collider))
                    return true;
            }

            return false;
        }

        public Vector3 ResponseStatic(Collider collider)
        {
            Box3i cells = getIntersectingCells(collider.TranslatedBounds());
            Vector3 response = Vector3.Zero;

            foreach (StaticBody b in getStaticBodies(cells))
            {
                response += b.Collider.Response(collider);
            }
            return response;
        }

        private List<StaticBody> getStaticBodies(Box3i cells)
        {
            List<StaticBody> r = new List<StaticBody>();
            for (int i = cells.Min.X; i <= cells.Max.X; i++)
            {
                for (int j = cells.Min.Y; j <= cells.Max.Y; j++)
                {
                    for (int k = cells.Min.Z; k <= cells.Max.Z; k++)
                    {
                        if (staticBodies[i, j, k] == null)
                            continue;

                        r.AddRange(staticBodies[i, j, k]);
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

    }
}
