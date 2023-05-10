using Engine.BaseTypes;
using Microsoft.Xna.Framework.Audio;

namespace Engine.BaseComponents;

public class SoundSource : Component
{
    public SoundSource() { }

    public SoundSource(SoundSource source) : base(source)
    {
        SoundInstance = source.SoundInstance;
        _playAtStart = source._playAtStart;
    }

    public SoundEffect Sound
    {
        set => SoundInstance = value.CreateInstance();
    }

    public SoundSource Play() { SoundInstance?.Play(); return this; }

    public SoundSource Stop() { SoundInstance?.Stop(); return this; }

    public SoundSource Pause() { SoundInstance?.Pause(); return this; }

    public SoundSource Resume() { SoundInstance?.Resume(); return this; }

    public SoundSource SetVolume(float volume)
    {
        if (SoundInstance != null) SoundInstance.Volume = volume;
        return this;
    }

    public SoundSource SetPan(float pan)
    {
        if (SoundInstance != null) SoundInstance.Pan = pan;
        return this;
    }

    public SoundSource SetIsLooped(bool isLooped)
    {
        if (SoundInstance != null) SoundInstance.IsLooped = isLooped;
        return this;
    }

    public SoundSource SetPlayAtStart(bool playAtStart) { _playAtStart = playAtStart; return this; }

    public SoundSource SetSound(SoundEffect sound)
    {
        SoundInstance = sound.CreateInstance();
        return this;
    }

    public SoundState GetState() => SoundInstance.State;

    public override void Start()
    {
        if (_playAtStart) SoundInstance?.Play();
    }

    public override void Unload()
    {
        SoundInstance?.Stop();
        SoundInstance?.Dispose();
        SoundInstance = null;
    }

    public SoundEffectInstance SoundInstance;

    private bool _playAtStart;
}
