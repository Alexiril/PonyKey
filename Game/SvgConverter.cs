using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AForge.Imaging.Filters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpVectors.Dom.Svg;
using SharpVectors.Renderers;
using SharpVectors.Renderers.Gdi;

namespace Game;

internal static class SvgConverter
{
#if DEBUG

    private static int _assetDebugNumber;

#endif

    internal static Texture2D TransformSvgToTexture2D(
        GraphicsDevice graphicsDevice,
        Stream svgStream,
        Vector2 size,
        List<IFilter> filters = null)
    {
        var renderer = new GdiGraphicsRenderer { BackColor = System.Drawing.Color.Transparent };
        renderer.Window = new GdiSvgWindow((int)size.X, (int)size.Y, renderer);
        var svgDocument = new SvgDocument(renderer.Window);
        svgDocument.Load(svgStream);
        var view = svgDocument.RootElement.Viewport;
        renderer.Window.Resize((int)Math.Max(view.Width, 1d), (int)Math.Max(view.Height, 1d));
        renderer.Render(svgDocument);
        var resultBitmap = renderer.RasterImage;
        if (filters != null) resultBitmap = filters.Aggregate(resultBitmap, (current, filter) => filter.Apply(current));
        var bufferSize = resultBitmap.Height * resultBitmap.Width * 4;
        var memoryStream = new MemoryStream(bufferSize);
        resultBitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
#if DEBUG
        if (!Directory.Exists("assetSvgDebug")) Directory.CreateDirectory("assetSvgDebug");
        var debugStream = new FileStream($"assetSvgDebug/temp{_assetDebugNumber}.png", FileMode.OpenOrCreate);
        resultBitmap.Save(debugStream, System.Drawing.Imaging.ImageFormat.Png);
        _assetDebugNumber++;
        debugStream.Dispose();
#endif
        svgStream.Dispose();
        return Texture2D.FromStream(graphicsDevice, memoryStream);
    }
}
