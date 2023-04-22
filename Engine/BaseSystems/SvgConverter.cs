using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpCompress.Compressors;
using SharpCompress.Compressors.BZip2;
using SharpVectors.Dom.Svg;
using SharpVectors.Renderers;
using SharpVectors.Renderers.Gdi;
using static System.Drawing.Imaging.ImageFormat;

namespace Engine.BaseSystems;

public static class SvgConverter
{
    public static Texture2D LoadSvg(
        Master master,
        string assetName,
        Vector2 size,
        string assets = "assets") =>
        TransformSvgToTexture2D(
            master.GraphicsDevice,
            ArchivedContent.LoadFile($"{assetName}.svg", assets),
            size
        );

    public static AnimationInformation LoadSvgAnimation(
        Master master,
        string assetName,
        Vector2 size,
        string assets = "assets")
    {
        using var file = new MemoryStream();
        using var fileStream = new MemoryStream();
        ArchivedContent.LoadFile($"{assetName}.asvg", assets, file);
        file.Position = 0;
        using (var svgAnimation = new BZip2Stream(file, CompressionMode.Decompress, true))
            svgAnimation.CopyTo(fileStream);
        fileStream.Position = 0;
        if (BinaryIO.DeserializeString(fileStream) != "PonyKey")
            throw new FileFormatException("Not correct svg animation asset.");
        var framerate = BinaryIO.DeserializeFloat(fileStream);
        var framesAmount = BinaryIO.DeserializeNumber(fileStream);
        var textures = new List<Texture2D>();
        for (var i = 0; i < framesAmount; i++)
            textures.Add(
                TransformSvgToTexture2D(
                    master.GraphicsDevice,
                    new MemoryStream(Encoding.ASCII.GetBytes(BinaryIO.DeserializeString(fileStream))),
                    size)
            );
        return new AnimationInformation(textures, framerate);
    }

    private static Texture2D TransformSvgToTexture2D(
        GraphicsDevice graphicsDevice,
        Stream svgStream,
        Vector2 size)
    {
        var renderer = new GdiGraphicsRenderer { BackColor = System.Drawing.Color.Transparent };
        renderer.Window = new GdiSvgWindow((int)size.X, (int)size.Y, renderer);
        var svgDocument = new SvgDocument(renderer.Window);
        svgDocument.Load(svgStream);
        var view = svgDocument.RootElement.Viewport;
        renderer.Window.Resize((int)Math.Max(view.Width, 1d), (int)Math.Max(view.Height, 1d));
        renderer.Render(svgDocument);
        var bufferSize = renderer.RasterImage.Height * renderer.RasterImage.Width * 4;
        var memoryStream = new MemoryStream(bufferSize);
        renderer.RasterImage.Save(memoryStream, Png);
        svgStream.Dispose();
        return Texture2D.FromStream(graphicsDevice, memoryStream);
    }
}
