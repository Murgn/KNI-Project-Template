using System;
using Microsoft.Xna.Framework.Audio;

namespace Core;

public class AudioGenerator
{
    private const int MaxSamples = 512;
    private const int MaxSamplesPerUpdate = 4096;

    public float Frequency
    {
        get => frequency;
        set
        {
            if (value == frequency) return;
            frequency = value;
            BuildWave();
        }
    }
    
    private float frequency = -1;
    private float oldFrequency = 1;

    private short[] data = new short[MaxSamples];
    private static short[] writeBuf = new short[MaxSamplesPerUpdate];
    private byte[] byteBuf = new byte[MaxSamplesPerUpdate * sizeof(short)];

    private int waveLength = 1;
    private int readCursor = 0;

    private DynamicSoundEffectInstance instance;

    public AudioGenerator(float frequency, float volume)
    {
        instance = new DynamicSoundEffectInstance(48000, AudioChannels.Mono);
        instance.Volume = volume;
        instance.Play();

        Frequency = frequency;    }

    public void Update()
    {
        // Keep a small queue of buffers topped up so playback doesn't stall or crackle
        while (instance.PendingBufferCount < 3)
        {
            int writeCursor = 0;

            while (writeCursor < MaxSamplesPerUpdate)
            {
                int writeLength = MaxSamplesPerUpdate - writeCursor;
                int readLength = waveLength - readCursor;
                if (writeLength > readLength) writeLength = readLength;

                Array.Copy(data, readCursor, writeBuf, writeCursor, writeLength);

                readCursor = (readCursor + writeLength) % waveLength;
                writeCursor += writeLength;
            }

            Buffer.BlockCopy(writeBuf, 0, byteBuf, 0, byteBuf.Length);
            instance.SubmitBuffer(byteBuf);
        }
    }
    
    public void Destroy()
    {
        instance.Stop();
        instance.Dispose();
    }
    
    private void BuildWave()
    {
        waveLength = (int)(24000  / frequency);
        if (waveLength > MaxSamples / 2) waveLength = MaxSamples / 2;
        if (waveLength < 1) waveLength = 1;

        for (int i = 0; i < waveLength * 2; i++)
        {
            data[i] = (short)(MathF.Sin((2 * MathF.PI * i) / waveLength) * 32000);
        }
        for (int i = waveLength * 2; i < MaxSamples; i++)
        {
            data[i] = 0;
        }

        readCursor = (int)(readCursor * (waveLength / oldFrequency));
        oldFrequency = frequency;
    }
    public void SetFrequency(float frequency) => Frequency = frequency;
}