using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Engine.Audio;

public class AudioController : IDisposable
{
    private readonly List<SoundEffectInstance> activeSfxInstances = new();

    private float previousSongVolume;
    private float previousSfxVolume;
    
    public bool IsMuted { get; private set; }

    public float SongVolume
    {
        get => IsMuted ? 0.0f : MediaPlayer.Volume;
        
        set
        {
            if (IsMuted) return;
            MediaPlayer.Volume = Math.Clamp(value, 0.0f, 1.0f);
        }
    }
    
    public float SfxVolume
    {
        get => IsMuted ? 0.0f : SoundEffect.MasterVolume;
        
        set
        {
            if (IsMuted) return;
            SoundEffect.MasterVolume = Math.Clamp(value, 0.0f, 1.0f);
        }
    }
    
    public bool IsDisposed { get; private set; }

    ~AudioController() => Dispose(false);

    public void Update()
    {
        for (int i = activeSfxInstances.Count - 1; i >= 0; i--)
        {
            SoundEffectInstance instance = activeSfxInstances[i];
            
            if(instance.State == SoundState.Stopped)
                instance.Dispose();
            
            activeSfxInstances.RemoveAt(i);
        }
    }

    public SoundEffectInstance Play(SoundEffect sfx) => Play(sfx, 1.0f, 0.0f, 0.0f, false);

    public SoundEffectInstance Play(SoundEffect sfx, float volume, float pitch, float pan, bool isLooped)
    {
        SoundEffectInstance sfxInstance = sfx.CreateInstance();

        sfxInstance.Volume = volume;
        sfxInstance.Pitch = pitch;
        sfxInstance.Pan = pan;
        sfxInstance.IsLooped = isLooped;
        
        sfxInstance.Play();
        
        activeSfxInstances.Add(sfxInstance);
        return sfxInstance;
    }

    public void PlaySong(Song song, bool isRepeating = true)
    {
        if(MediaPlayer.State == MediaState.Playing) MediaPlayer.Stop();
        MediaPlayer.Play(song);
        MediaPlayer.IsRepeating = isRepeating;
    }

    public void SetVolume(float volume)
    {
        SongVolume = volume;
        SfxVolume = volume;
    }
    
    public void ModifyVolume(float adjustment)
    {
        SongVolume += adjustment;
        SfxVolume += adjustment;
    }

    public void PauseAudio()
    {
        MediaPlayer.Pause();

        foreach (var sfxInstance in activeSfxInstances)
            sfxInstance.Pause();
    }
    
    public void ResumeAudio()
    {
        MediaPlayer.Resume();

        foreach (var sfxInstance in activeSfxInstances)
            sfxInstance.Resume();
    }

    public void MuteAudio()
    {
        previousSongVolume = MediaPlayer.Volume;
        previousSfxVolume = SoundEffect.MasterVolume;

        MediaPlayer.Volume = 0.0f;
        SoundEffect.MasterVolume = 0.0f;
        
        IsMuted = true;
    }

    public void UnmuteAudio()
    {
        MediaPlayer.Volume = previousSongVolume;
        SoundEffect.MasterVolume = previousSfxVolume;
        IsMuted = false;
    }

    public void ToggleMute()
    {
        if(IsMuted) UnmuteAudio();
        else MuteAudio();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected void Dispose(bool disposing)
    {
        if (IsDisposed) return;

        if (disposing)
        {
            foreach (var sfxInstance in activeSfxInstances)
                sfxInstance.Dispose();
            
            activeSfxInstances.Clear();
        }

        IsDisposed = true;
    }
}