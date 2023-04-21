using System;
using System.IO;
using SharpCompress.Readers;

namespace Engine.BaseSystems;

internal static class ArchivedContent
{
    internal static MemoryStream LoadFile(string filename, string assets = "assets", Stream copyTo = null)
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

    internal static T LoadContentFile<T>(Master master, string filename, string assets = "assets")
    {
        var resultFileName = $"{Path.GetTempPath()}tmp_asf";
        using (var temporaryFile = File.OpenWrite($"{resultFileName}.xnb"))
            LoadFile($"{filename}.xnb", assets, temporaryFile).Dispose();
        var result = master.Content.Load<T>(resultFileName);
        File.Delete($"{resultFileName}.xnb");
        return result;
    }
}
