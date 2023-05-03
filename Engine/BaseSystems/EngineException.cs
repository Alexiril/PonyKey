using System;

namespace Engine.BaseSystems;

public class EngineException : Exception
{
    public EngineException(string message) : base(message) {}
}
