using Engine.BaseTypes;
using Microsoft.Xna.Framework.Audio;

namespace Engine.BaseComponents;

public class SoundSource : Component
{
    public string Name { get; private set; }

    public SoundSource Play() { Sound.Play(); return this; }

    public SoundSource Stop() { Sound.Stop(); return this; }

    public SoundSource Pause() { Sound.Pause(); return this; }

    public SoundSource Resume() { Sound.Resume(); return this; }

    public SoundSource SetVolume(float volume) { Sound.Volume = volume; return this; }

    public SoundSource SetPan(float pan) { Sound.Pan = pan; return this; }

    public SoundSource SetIsLooped(bool isLooped) { Sound.IsLooped = isLooped; return this; }

    public SoundSource SetPlayAtStart(bool playAtStart) { _playAtStart = playAtStart; return this; }

    public SoundSource SetSound(SoundEffect sound)
    {
        Sound = sound.CreateInstance();
        Name = sound.Name;
        return this;
    }

    public SoundState GetState() => Sound.State;

    public override void Start()
    {
        if (_playAtStart) Sound.Play();
    }

    public SoundEffectInstance Sound;

    private bool _playAtStart;
}
