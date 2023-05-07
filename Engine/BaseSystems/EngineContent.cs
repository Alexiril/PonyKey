using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Engine.BaseTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpCompress.Compressors;
using SharpCompress.Compressors.BZip2;
using SharpCompress.Readers;
using SharpVectors.Dom.Svg;
using SharpVectors.Renderers;
using SharpVectors.Renderers.Gdi;
using static System.Drawing.Imaging.ImageFormat;
using Color = System.Drawing.Color;


namespace Engine.BaseSystems;

public static class EngineContent
{

    public static List<string> GetFilesNames(string folder, string assets = "assets")
    {
        var result = new List<string>();
        using Stream stream = File.OpenRead($"{assets}.dat");
        using var reader = ReaderFactory.Open(stream);
        while (reader.MoveToNextEntry())
        {
            if (reader.Entry.IsDirectory || reader.Entry.Key[..folder.Length] != folder) continue;
            result.Add(reader.Entry.Key);
        }

        return result;
    }

    public static MemoryStream LoadFile(string filename, string assets = "assets", Stream copyTo = null)
    {
        using Stream stream = File.OpenRead($"{assets}.dat");
        using var reader = ReaderFactory.Open(stream);
        while (reader.MoveToNextEntry())
        {
            if (reader.Entry.IsDirectory ||
                !string.Equals(reader.Entry.Key, filename, StringComparison.CurrentCultureIgnoreCase)) continue;
            if (copyTo == null)
            {
                var bufferStream = new MemoryStream();
                using (var entryStream = reader.OpenEntryStream()) entryStream.CopyTo(bufferStream);
                bufferStream.Position = 0;
                return bufferStream;
            }
            using (var entryStream = reader.OpenEntryStream()) entryStream.CopyTo(copyTo);
            return new MemoryStream();
        }

        throw new FileNotFoundException($"File {filename} wasn't found.");
    }

    public static T LoadContent<T>(string filename, string assets = "assets")
    {
        var randomKey = new Random().Next(0x1000, 0xFFFFFF);
        var resultFileName = $"{Path.GetTempPath()}tmp_asf{randomKey}";
        using (var temporaryFile = File.OpenWrite($"{resultFileName}.xnb"))
            LoadFile($"{filename}.xnb", assets, temporaryFile).Dispose();
        var result = Game.Content.Load<T>(resultFileName);
        File.Delete($"{resultFileName}.xnb");
        return result;
    }

    public static Texture2D LoadSvgTexture(
        string assetName,
        Vector2 size,
        string assets = "assets") =>
        TransformSvgToTexture2D(EngineContent.LoadFile($"{assetName}.svg", assets), size);

    public static AnimationInformation LoadSvgAnimation(
        string assetName,
        Vector2 size,
        string assets = "assets")
    {
        using var file = new MemoryStream();
        using var fileStream = new MemoryStream();
        EngineContent.LoadFile($"{assetName}.asvg", assets, file);
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
            textures.Add(TransformSvgToTexture2D(
                new MemoryStream(Encoding.ASCII.GetBytes(BinaryIO.DeserializeString(fileStream))),
                size));
        return new AnimationInformation(textures, framerate);
    }

    private static Texture2D TransformSvgToTexture2D(
        Stream svgStream,
        Vector2 size)
    {
        if (size.X == 0 || size.Y == 0)
            return new Texture2D(Game.GraphicsDevice, 0, 0);
        var renderer = new GdiGraphicsRenderer { BackColor = Color.Transparent };
        renderer.Window = new GdiSvgWindow((int)size.X, (int)size.Y, renderer);
        var svgDocument = new SvgDocument(renderer.Window);
        svgDocument.Load(svgStream);
        var view = svgDocument.RootElement.Viewport;
        renderer.Window.Resize((int)Math.Max(view.Width, 1d), (int)Math.Max(view.Height, 1d));
        renderer.Render(svgDocument);
        var bufferSize = renderer.RasterImage.Height * renderer.RasterImage.Width * 4;
        var memoryStream = new MemoryStream(bufferSize);
        renderer.RasterImage.Save(memoryStream, Png);
#if DEBUG
        _rendered++;
        if (!Directory.Exists("svgdebug")) Directory.CreateDirectory("svgdebug");
        using (var stream = new FileStream($"svgdebug/{_rendered}.png", FileMode.OpenOrCreate))
            renderer.RasterImage.Save(stream, Png);
#endif
        svgStream.Dispose();
        return Texture2D.FromStream(Game.GraphicsDevice, memoryStream);
    }

#if DEBUG
    private static int _rendered;
#endif
}
