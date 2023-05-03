using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Engine.BaseSystems;

// ReSharper disable once InconsistentNaming
public static class BinaryIO
{
    public static bool DeserializeBool(Stream file) => file.ReadByte() == 1;

    public static double DeserializeDouble(Stream file)
    {
        var buffer = new byte[8];
        if (file.Read(buffer, 0, 8) < 8)
            throw new EngineException(
                "Sorry, the stream is not correct. Couldn't read float.");
        return BitConverter.ToDouble(buffer.Reverse().ToArray());
    }

    public static float DeserializeFloat(Stream file)
    {
        var buffer = new byte[4];
        if (file.Read(buffer, 0, 4) < 4)
            throw new EngineException(
                "Sorry, the stream is not correct. Couldn't read float.");
        return BitConverter.ToSingle(buffer.Reverse().ToArray());
    }

    public static int DeserializeNumber(Stream file)
    {
        var buffer = new byte[4];
        if (file.Read(buffer, 0, 4) < 4)
            throw new EngineException(
                "Sorry, the stream is not correct. Couldn't read int.");
        return BitConverter.ToInt32(buffer.Reverse().ToArray());
    }

    public static string DeserializeString(Stream file)
    {
        var length = DeserializeNumber(file);
        var buffer = new byte[length];
        if (file.Read(buffer, 0, length) < length)
            throw new EngineException(
                "Sorry, the stream is not correct. Couldn't read string.");
        return Encoding.UTF8.GetString(buffer);
    }
}
