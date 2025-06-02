//	Copyright (c) 2012 Calvin Rien
//        http://the.darktable.com
//
//	This software is provided 'as-is', without any express or implied warranty. In
//	no event will the authors be held liable for any damages arising from the use
//	of this software.
//
//	Permission is granted to anyone to use this software for any purpose,
//	including commercial applications, and to alter it and redistribute it freely,
//	subject to the following restrictions:
//
//	1. The origin of this software must not be misrepresented; you must not claim
//	that you wrote the original software. If you use this software in a product,
//	an acknowledgment in the product documentation would be appreciated but is not
//	required.
//
//	2. Altered source versions must be plainly marked as such, and must not be
//	misrepresented as being the original software.
//
//	3. This notice may not be removed or altered from any source distribution.
//
//  =============================================================================
//
//  derived from Gregorio Zanon's script
//  http://forum.unity3d.com/threads/119295-Writing-AudioListener.GetOutputData-to-wav-problem?p=806734&viewfull=1#post806734

using System;
using System.IO;
using UnityEngine;

public static class WavUtility
{
    const int HEADER_SIZE = 44;

    public static byte[] FromAudioClip(AudioClip clip, out string filepath, bool trimSilence = false, float silenceThreshold = 0.01f)
    {
        var samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        if (trimSilence)
            samples = TrimSilence(samples, silenceThreshold);

        byte[] wav = ConvertAudioClipDataToWav(samples, clip.channels, clip.frequency);

        filepath = Path.Combine(Application.dataPath, clip.name + ".wav");
        return wav;
    }

    private static float[] TrimSilence(float[] samples, float threshold)
    {
        int start = 0;
        int end = samples.Length - 1;

        while (start < samples.Length && Mathf.Abs(samples[start]) < threshold)
            start++;

        while (end > start && Mathf.Abs(samples[end]) < threshold)
            end--;

        int length = end - start + 1;
        float[] trimmed = new float[length];
        Array.Copy(samples, start, trimmed, 0, length);
        return trimmed;
    }

    private static byte[] ConvertAudioClipDataToWav(float[] samples, int channels, int frequency)
    {
        int sampleCount = samples.Length;
        byte[] wav = new byte[sampleCount * 2 + HEADER_SIZE];

        // Header
        WriteHeader(wav, sampleCount, channels, frequency);

        // Audio Data
        int offset = HEADER_SIZE;
        for (int i = 0; i < samples.Length; i++)
        {
            short intData = (short)(samples[i] * short.MaxValue);
            byte[] byteArr = BitConverter.GetBytes(intData);
            wav[offset++] = byteArr[0];
            wav[offset++] = byteArr[1];
        }

        return wav;
    }

    private static void WriteHeader(byte[] stream, int sampleCount, int channels, int frequency)
    {
        int byteRate = frequency * channels * 2;

        // Chunk ID "RIFF"
        Array.Copy(System.Text.Encoding.ASCII.GetBytes("RIFF"), 0, stream, 0, 4);
        Array.Copy(BitConverter.GetBytes(stream.Length - 8), 0, stream, 4, 4);
        Array.Copy(System.Text.Encoding.ASCII.GetBytes("WAVE"), 0, stream, 8, 4);

        // Subchunk 1 "fmt "
        Array.Copy(System.Text.Encoding.ASCII.GetBytes("fmt "), 0, stream, 12, 4);
        Array.Copy(BitConverter.GetBytes(16), 0, stream, 16, 4);
        Array.Copy(BitConverter.GetBytes((short)1), 0, stream, 20, 2);
        Array.Copy(BitConverter.GetBytes((short)channels), 0, stream, 22, 2);
        Array.Copy(BitConverter.GetBytes(frequency), 0, stream, 24, 4);
        Array.Copy(BitConverter.GetBytes(byteRate), 0, stream, 28, 4);
        Array.Copy(BitConverter.GetBytes((short)(channels * 2)), 0, stream, 32, 2);
        Array.Copy(BitConverter.GetBytes((short)16), 0, stream, 34, 2);

        // Subchunk 2 "data"
        Array.Copy(System.Text.Encoding.ASCII.GetBytes("data"), 0, stream, 36, 4);
        Array.Copy(BitConverter.GetBytes(sampleCount * 2), 0, stream, 40, 4);
    }
}
