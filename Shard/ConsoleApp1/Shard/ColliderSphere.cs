﻿
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

    class ColliderSphere : Collider3D
    {
        private float radius;
        Vector3 centre;
        Vector3 boundingBoxMin;
        Vector3 boundingBoxMax;
        Transform3DNew mySphere;
        private bool checkOffset;
        public ColliderSphere(CollisionHandler gob, Transform3DNew t, float radius) : base(gob)
        {
            this.radius = radius;
            mySphere = t;
            centre = t.Translation;
            calculateBoundingBox();
        }
        public override void calculateBoundingBox()
        {
            boundingBoxMax = new Vector3(radius + centre.X, radius + centre.Y, radius + centre.Z);
            boundingBoxMin = new Vector3(radius - centre.X, radius - centre.Y, radius - centre.Z);
        }



        public override void recalculate()
        {
            calculateBoundingBox();
        }
        public override bool areColliding(ColliderCube c)
        {
            return areColliding(c, Vector2.Zero);
        }

        public override bool areColliding(ColliderSphere c)
        {
            return areColliding(c, Vector2.Zero);
        }

        public override bool areColliding(OpenTK.Mathematics.Vector3 c)
        {
            throw new NotImplementedException();
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
            return centre;
        }

        public override float getCentreX()
        {
            return centre.X;
        }

        public override float getCentreY()
        {
            return centre.Y;
        }

        public override float getCentreZ()
        {
            return centre.Z;
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

        public float getRadius()
        {
            return radius;
        }

        public override bool areColliding(ColliderCube c, Vector2 offset)
        {
            float xDistance = Math.Abs(centre.X + offset.X - c.getCentreX());
            float yDistance = Math.Abs(centre.Y + offset.Y - c.getCentreX());
            float zDistance = Math.Abs(centre.Z - c.getCentreX());

            if (xDistance <= (c.getWidth() / 2 + getRadius()) && yDistance <= (c.getHeight() / 2 + getRadius()) && zDistance <= (c.getDepth() / 2 + getRadius()))
            {
                return true;
            }
            float cornerDistance_sq = ((xDistance - c.getWidth()) * (xDistance - c.getWidth())) +
                                  ((yDistance - c.getHeight()) * (yDistance - c.getHeight()) +
                                  ((yDistance - c.getDepth()) * (yDistance - c.getDepth())));

            return (cornerDistance_sq < (getRadius() * getRadius()));
        }

        public override bool areColliding(ColliderSphere c, Vector2 offset)
        {
            double dis = Math.Sqrt(Math.Pow(centre.X + offset.X - c.centre.X, 2) + Math.Pow(centre.Y + offset.Y - c.centre.Y, 2) + Math.Pow(centre.Z - c.centre.Z, 2));
            return dis <= (radius + c.getRadius());
        }

        public override bool areColliding(Vector3 c, Vector2 offset)
        {
            throw new NotImplementedException();
        }

        public override bool GetCheckOffset()
        {
            throw new NotImplementedException();
        }

        public override void SetCheckOffset(bool value)
        {
            throw new NotImplementedException();
        }

        public new bool CheckOffset { get => checkOffset; set => checkOffset = value; }
    }
}
