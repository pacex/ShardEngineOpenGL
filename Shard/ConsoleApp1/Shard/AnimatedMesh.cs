using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    enum AnimationMode
    {
        Loop,
        End
    }

    class AnimatedMesh : Mesh
    {
        public AnimationMode Mode;

        private Texture texture;
        private int frameCount;
        private float animationSpeed;
        private long startTime;

        public AnimatedMesh(Texture texture, int frameCount, float animationSpeed) : base() {
            this.texture = texture;
            this.frameCount = frameCount;
            this.animationSpeed = animationSpeed;
            Mode = AnimationMode.Loop;
            StartAnimation();
        }

        public AnimatedMesh(float[] vert, uint[] ind, Texture texture, int frameCount, float animationSpeed) : base(vert, ind) {
            this.texture = texture;
            this.frameCount = frameCount;
            this.animationSpeed = animationSpeed;
            Mode = AnimationMode.Loop;
            StartAnimation();
        }

        public override void Draw()
        {
            long millisSinceStart = Bootstrap.getCurrentMillis() - startTime;
            long framesSinceStart = (long)(millisSinceStart * 0.001f * animationSpeed);

            int frameIndex = 0;

            switch (Mode)
            {
                case AnimationMode.Loop:
                    frameIndex = (int)(framesSinceStart % frameCount);
                    break;
                case AnimationMode.End:
                    frameIndex = (int)Math.Min(framesSinceStart, frameCount - 1);
                    break;
            }

            Shader.ApplyAnimatedShader(texture, frameCount, frameIndex);
            base.Draw();
            Shader.Reset();
        }

        public void StartAnimation()
        {
            startTime = Bootstrap.getCurrentMillis();
        }

        public void ChangeAnimation(Texture texture, int frameCount, float animationSpeed, AnimationMode mode)
        {
            this.texture = texture;
            this.frameCount = frameCount;
            this.animationSpeed = animationSpeed;
            this.Mode = mode;
            StartAnimation();
        }
    }
}
