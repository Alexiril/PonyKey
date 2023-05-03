using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SharpCompress.Readers;

namespace Engine.BaseSystems;

public static class ArchivedContent
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

        throw new EngineException($"File {filename} wasn't found.");
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

    public static string LoadScene(string filename, string assets = "assets")
    {
        var file = new MemoryStream();
        LoadFile($"{filename}.yaml", assets, file);
        return Encoding.UTF8.GetString(file.GetBuffer());
    }
}
