using Game.BaseTypes;
using Microsoft.Xna.Framework.Audio;

namespace Game.BuiltInComponents;

internal class SoundSource : Component
{
    internal string Name { get; private set; }

    internal SoundSource Play() { Sound.Play(); return this; }

    internal SoundSource Stop() { Sound.Stop(); return this; }

    internal SoundSource Pause() { Sound.Pause(); return this; }

    internal SoundSource Resume() { Sound.Resume(); return this; }

    internal SoundSource SetVolume(float volume) { Sound.Volume = volume; return this; }

    internal SoundSource SetPan(float pan) { Sound.Pan = pan; return this; }

    internal SoundSource SetIsLooped(bool isLooped) { Sound.IsLooped = isLooped; return this; }

    internal SoundSource SetPlayAtStart(bool playAtStart) { _playAtStart = playAtStart; return this; }

    internal SoundSource SetSound(SoundEffect sound)
    {
        Sound = sound.CreateInstance();
        Name = sound.Name;
        return this;
    }

    internal SoundState GetState() => Sound.State;

    internal override void Start()
    {
        if (_playAtStart) Sound.Play();
    }

    internal SoundEffectInstance Sound;

    private bool _playAtStart;
}
