/*
*
*   The specific collider for rectangles.   Handles rect/circle, rect/rect and rect/vector.
*   @author Michael Heron
*   @version 1.0
*   
*/

using System;
using System.Drawing;
using System.Numerics;
using OpenTK.Graphics.OpenGL;

namespace Shard
{
    class ColliderRect : Collider
    {
        private Transform3DNew transform;
        private float width, height;

        public ColliderRect(CollisionHandler gob, Transform3DNew t, float wid, float ht) : base(gob)
        {

            width = wid;
            height = ht;
            RotateAtOffset = true;

            this.Transform = t;


        }

        public void calculateBoundingBox()
        {
            MinAndMaxX[0] = Transform.Translation.X - Width / 2;
            MinAndMaxX[1] = Transform.Translation.X + Width / 2;
            MinAndMaxY[0] = Transform.Translation.Y - Height / 2;
            MinAndMaxY[1] = Transform.Translation.Y + Height / 2;
        }

        internal Transform3DNew Transform { get => transform; set => transform = value; }
        public float Left { get => MinAndMaxX[0]; set => MinAndMaxX[0] = value; }
        public float Right { get => MinAndMaxX[1]; set => MinAndMaxX[1] = value; }
        public float Top { get => MinAndMaxY[0]; set => MinAndMaxY[0] = value; }
        public float Bottom { get => MinAndMaxY[1]; set => MinAndMaxY[1] = value; }
        public float Width { get => width; set => width = value; }
        public float Height { get=> height; set => height = value; }


        public override void recalculate()
        {
            calculateBoundingBox();
        }

        public ColliderRect calculateMinkowskiDifference(ColliderRect other)
        {
            float left, right, top, bottom, width, height;
            ColliderRect mink = new ColliderRect(null, null, 0.0f, 0.0f);

            // A set of calculations that gives us the Minkowski difference
            // for this intersection.
            left = Left - other.Right;
            top = other.Top - Bottom;
            width = Width + other.Width;
            height = Height + other.Height;
            right = Right - other.Left;
            bottom = other.Bottom - Top;

            mink.Width = width;
            mink.Height = height;

            mink.MinAndMaxX = new float[2] { left, right };
            mink.MinAndMaxY = new float[2] { top, bottom };

            return mink;
        }

        public Vector2? calculatePenetration(Vector2 checkPoint)
        {
            Vector2? impulse;
            float coff = 0.2f;

            // Check the right edge
            float min;

            min = Math.Abs(Right - checkPoint.X);
            impulse = new Vector2(-1 * min - coff, checkPoint.Y);


            // Now compare against the Left edge
            if (Math.Abs(checkPoint.X - Left) <= min)
            {
                min = Math.Abs(checkPoint.X - Left);
                impulse = new Vector2(min + coff, checkPoint.Y);
            }

            // Now the bottom
            if (Math.Abs(Bottom - checkPoint.Y) <= min)
            {
                min = Math.Abs(Bottom - checkPoint.Y);
                impulse = new Vector2(checkPoint.X, min + coff);
            }

            // And now the top
            if (Math.Abs(Top - checkPoint.Y) <= min)
            {
                min = Math.Abs(Top - checkPoint.Y);
                impulse = new Vector2(checkPoint.X, -1 * min - coff);
            }

            return impulse;
        }

        public override Vector2? checkCollision(ColliderRect other)
        {
            ColliderRect cr;

            cr = calculateMinkowskiDifference(other);

            if (cr.Left <= 0 && cr.Right >= 0 && cr.Top <= 0 && cr.Bottom >= 0)
            {
                return cr.calculatePenetration(new Vector2(0, 0));
            }



            return null;

        }

        public override void drawMe(Color col)
        {
            /*
             * LEGACY
             * 
             * Display d = Bootstrap.getDisplay();

                d.drawLine((int)MinAndMaxX[0], (int)MinAndMaxY[0], (int)MinAndMaxX[1], (int)MinAndMaxY[0], col);
                d.drawLine((int)MinAndMaxX[0], (int)MinAndMaxY[0], (int)MinAndMaxX[0], (int)MinAndMaxY[1], col);
                d.drawLine((int)MinAndMaxX[1], (int)MinAndMaxY[0], (int)MinAndMaxX[1], (int)MinAndMaxY[1], col);
                d.drawLine((int)MinAndMaxX[0], (int)MinAndMaxY[1], (int)MinAndMaxX[1], (int)MinAndMaxY[1], col);

                d.drawCircle((int)Transform.Translation.X, (int)Transform.Translation.Y, 2, col);
             * 
             */
            float[] vertices = new float[] {    MinAndMaxX[0], MinAndMaxY[0], 0.0f,     MinAndMaxX[0], MinAndMaxY[0], 2.0f,
                                                MinAndMaxX[0], MinAndMaxY[1], 0.0f,     MinAndMaxX[0], MinAndMaxY[1], 2.0f,
                                                MinAndMaxX[1], MinAndMaxY[0], 0.0f,     MinAndMaxX[1], MinAndMaxY[0], 2.0f,
                                                MinAndMaxX[1], MinAndMaxY[1], 0.0f,     MinAndMaxX[1], MinAndMaxY[1], 2.0f };

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

        public override Vector2? checkCollision(ColliderCircle c)
        {
            Vector2? possibleV = c.checkCollision(this);

            if (possibleV is Vector2 v)
            {
                v.X *= -1;
                v.Y *= -1;
                return v;
            }

            return null;
        }

        public override float[] getMinAndMaxX()
        {
            calculateBoundingBox();
            return MinAndMaxX;
        }

        public override float[] getMinAndMaxY()
        {
            calculateBoundingBox();
            return MinAndMaxY;
        }

        public override Vector2? checkCollision(Vector2 other)
        {

            if (other.X >= Left &&
                other.X <= Right &&
                other.Y >= Top &&
                other.Y <= Bottom)
            {
                return new Vector2(0, 0);
            }

            return null;
        }

    }


}
