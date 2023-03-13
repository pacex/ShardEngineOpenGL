using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;


namespace Shard
{

    
    class ColliderCube : Collider3D
    {
      
        Vector3 boundingBoxMin;
        Vector3 boundingBoxMax;
        private float widht;
        private float height;
        private float depth;
        private bool checkOffset = true;
        private Transform3DNew myCube;
        public ColliderCube(CollisionHandler gob, Transform3DNew t,float wdt, float hgt, float dpt) : base(gob)
        {
            
            this.widht = wdt;
            this.height = hgt;
            this.depth = dpt;
            myCube = t;
            myCube.Translation = t.Translation;

            calculateBoundingBox();
        }

        public override bool areColliding(Vector3 c)
        {
            throw new NotImplementedException();
        }
        
        public override bool areColliding(ColliderCube c)
        {
            return areColliding(c, Vector2.Zero);
        }
        public override void calculateBoundingBox()
        {
            boundingBoxMax = (myCube.Translation.X + (widht / 2), myCube.Translation.Y + (height / 2), myCube.Translation.Z + (depth / 2));
            boundingBoxMin = (myCube.Translation.X - (widht / 2), myCube.Translation.Y - (height / 2), myCube.Translation.Z - (depth / 2));
        }
        //public override Vector3? checkCollision(ColliderCube c)
        //{
        //    throw new NotImplementedException();
        //}

        //public override Vector3? checkCollision(Vector3 c)
        //{
        //    throw new NotImplementedException();
        //}

        public override bool areColliding(ColliderSphere c)
        {
            return areColliding(c, Vector2.Zero);
        }

        public override void DrawMe(Color col)
        {
            float[] vertices = new float[] {    getMinX(), getMinY(), getMinZ(),     getMinX(), getMinY(), getMaxZ(),
                                                getMinX(), getMaxY(), getMinZ(),     getMinX(), getMaxY(), getMaxZ(),
                                                getMaxX(), getMinY(), getMinZ(),     getMaxX(), getMinY(), getMaxZ(),
                                                getMaxX(), getMaxY(), getMinZ(),     getMaxX(), getMaxY(), getMaxZ() };

            uint[] indices = new uint[] { 0, 1, 0, 2, 0, 4,
                                          1, 3, 1, 5,
                                          2, 3, 2, 6,
                                          3, 7,
                                          4, 5, 4, 6,
                                          5, 7,
                                          6, 7};

            int vertexBufferObject, vertexArrayObject;

            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);

            vertexBufferObject = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            Shader.ApplyWireframeShader(new OpenTK.Mathematics.Color4(col.R, col.G, col.B, col.A));
            GL.BindVertexArray(vertexArrayObject);
            GL.DrawElements(PrimitiveType.Lines, indices.Length, DrawElementsType.UnsignedInt, indices);
            GL.BindVertexArray(0);
            Shader.Reset();

            GL.DeleteBuffer(vertexBufferObject);
            GL.DeleteVertexArray(vertexArrayObject);
        }

        public override Vector3 getCentre()
        {
            return myCube.Translation;
        }

        public override float getCentreX()
        {
            return myCube.Translation.X;
        }

        public override float getCentreY()
        {
            return myCube.Translation.Y;
        }

        public override float getCentreZ()
        {
            return myCube.Translation.Z;
        }
        public float getWidth()
        {
            return widht;
        }
        public float getHeight()
        {
            return height;
        }
        public float getDepth()
        {
            return depth;
        }

        public Vector3 getBoundingBoxMin()
        {
            return boundingBoxMin;
        }
        public Vector3 getBoundingBoxMax()
        {
            return boundingBoxMax;
        }
        public override float getMinX()
        {
            return boundingBoxMin.X;
        }
        public override float getMaxX()
        {
            return boundingBoxMax.X;
        }
        public override float getMinY()
        {
            return boundingBoxMin.Y;
        }
        public override float getMaxY()
        {
            return boundingBoxMax.Y;
        }

        public override float getMinZ()
        {
            return boundingBoxMin.Z;
        }
        public override float getMaxZ()
        {
            return boundingBoxMax.Z;
        }

        public override void recalculate()
        {
            calculateBoundingBox();
        }

        public override bool areColliding(ColliderCube c, Vector2 offset)
        {
            /*
            float xDistance = Math.Abs(myCube.Translation.X + offset.X - c.getCentreX());
            float yDistance = Math.Abs(myCube.Translation.Y + offset.Y - c.getCentreX());
            float zDistance = Math.Abs(myCube.Translation.Z - c.getCentreX());

            if (xDistance <= ((c.getWidth() + getWidth()) / 2) && yDistance <= ((c.getHeight() + getHeight()) / 2) && zDistance <= ((c.getDepth() + getDepth()) / 2))
            {
                return true;
            }
            //TODO might not work.
            //float cornerDistance_sq = ((sphereXDistance - c.getWidth()) * (sphereXDistance - c.getWidth())) +
            //                      ((sphereYDistance - c.getHeight()) * (sphereYDistance - c.getHeight()) +
            //                      ((sphereYDistance - c.getDepth()) * (sphereYDistance - c.getDepth())));

            //return (cornerDistance_sq < (radius * radius));

            */
            Box3 own = new Box3(new Vector3(getMinX() + offset.X, getMinY() + offset.Y, getMinZ()), new Vector3(getMaxX() + offset.X, getMaxY() + offset.Y, getMaxZ()));
            Box3 other = new Box3(new Vector3(c.getMinX(), c.getMinY(), c.getMinZ()), new Vector3(c.getMaxX(), c.getMaxY(), c.getMaxZ()));

            return own.Contains(other);
        }

        public override bool areColliding(ColliderSphere c, Vector2 offset)
        {
            float xDistance = Math.Abs(myCube.Translation.X + offset.X - c.getCentreX());
            float yDistance = Math.Abs(myCube.Translation.Y + offset.Y - c.getCentreX());
            float zDistance = Math.Abs(myCube.Translation.Z - c.getCentreX());

            if (xDistance <= (getWidth() / 2 + c.getRadius()) && yDistance <= (getHeight() / 2 + c.getRadius()) && zDistance <= (getDepth() / 2 + c.getRadius()))
            {
                return true;
            }
            float cornerDistance_sq = ((xDistance - getWidth()) * (xDistance - getWidth())) +
                                  ((yDistance - getHeight()) * (yDistance - getHeight()) +
                                  ((yDistance - getDepth()) * (yDistance - getDepth())));

            return (cornerDistance_sq < (c.getRadius() * c.getRadius()));
        }

        public override bool areColliding(Vector3 c, Vector2 offset)
        {
            throw new NotImplementedException();
        }

        public override bool GetCheckOffset()
        {
            return checkOffset;
        }

        public override void SetCheckOffset(bool value)
        {
            checkOffset = value;
        }
    }
}
