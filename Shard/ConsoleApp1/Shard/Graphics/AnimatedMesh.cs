using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard.Graphics
{
    enum AnimationMode
    {
        Loop,
        LoopOnCall,
        End
    }

    class AnimatedMesh : Mesh
    {
        public AnimationMode Mode;

        private Texture texture;
        private int frameCount;
        private float animationSpeed;
        private long startTime;

        public AnimatedMesh(Texture texture, int frameCount, float animationSpeed) : base()
        {
            this.texture = texture;
            this.frameCount = frameCount;
            this.animationSpeed = animationSpeed;
            Mode = AnimationMode.Loop;
            startTime = -1;
        }

        public AnimatedMesh(float[] vert, uint[] ind, Texture texture, int frameCount, float animationSpeed) : base(vert, ind)
        {
            this.texture = texture;
            this.frameCount = frameCount;
            this.animationSpeed = animationSpeed;
            Mode = AnimationMode.Loop;
            startTime = -1;
        }

        public override void Draw()
        {
            int frameIndex = getFrameIndex();

            Shader.ApplyAnimatedShader(texture, frameCount, frameIndex);
            base.Draw();
            Shader.Reset();
        }

        private int getFrameIndex()
        {
            long millisSinceStart = Bootstrap.getCurrentMillis() - startTime;
            long framesSinceStart = (long)(millisSinceStart * 0.001f * animationSpeed);
            if (startTime == -1)
            {
                framesSinceStart = 0;
            }

            int frameIndex = 0;

            switch (Mode)
            {
                case AnimationMode.Loop:
                    frameIndex = (int)(framesSinceStart % frameCount);
                    break;
                case AnimationMode.LoopOnCall:
                    frameIndex = framesSinceStart >= frameCount ? 0 : (int)framesSinceStart;
                    break;
                case AnimationMode.End:
                    frameIndex = (int)Math.Min(framesSinceStart, frameCount - 1);
                    break;
            }

            return frameIndex;
        }

        public float GetAnimationProgress()
        {
            return getFrameIndex() / (float)frameCount;
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
            Mode = mode;
        }
    }
}
