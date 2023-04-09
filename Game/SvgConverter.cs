using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpVectors.Dom.Svg;
using SharpVectors.Renderers;
using SharpVectors.Renderers.Gdi;
using AForge.Imaging.Filters;

namespace Game;

internal static class SvgConverter
{
    public static Texture2D TransformSvgToTexture2D(
        GraphicsDevice graphicsDevice,
        Stream svgStream,
        Vector2 size,
        IFilter filter = null)
    {
        var renderer = new GdiGraphicsRenderer();
        renderer.BackColor = System.Drawing.Color.Transparent;
        renderer.Window = new GdiSvgWindow((int)size.X, (int)size.Y, renderer);
        var svgDocument = new SvgDocument(renderer.Window);
        svgDocument.Load(svgStream);
        var view = svgDocument.RootElement.Viewport;
        foreach (var node in svgDocument.GetElementsByTagName("text"))
            if (node is ISvgTextElement itext)
            {
                itext.SetAttribute("y", "0");
                itext.SetAttribute("x", "0");
            }
        renderer.Window.Resize((int)Math.Max(view.Width, 1d), (int)Math.Max(view.Height, 1d));
        renderer.Render(svgDocument);
        var resultBitmap = renderer.RasterImage;
        if (filter != null)
            resultBitmap = filter.Apply(resultBitmap);
        var bufferSize = resultBitmap.Height * resultBitmap.Width * 4;
        var memoryStream = new MemoryStream(bufferSize);
        resultBitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
        return Texture2D.FromStream(graphicsDevice, memoryStream);
    }
}
