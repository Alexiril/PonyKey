using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.BaseSystems;

public struct AnimationInformation
{
    public List<Texture2D> Frames;
    public float Framerate;

    public AnimationInformation(List<Texture2D> frames, float framerate)
    {
        Frames = frames;
        Framerate = framerate;
    }
}
