using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics;

public static class Mp3DividerAsset
{
    public static float splitTime = 0.35f;

    [MenuItem("自作メニュー/Create/Divide Audio File")]
    public static void DivideAudioClip()
    {
        // ファイルを選択
        string assetPath = EditorUtility.OpenFilePanel("MP3またはWAVファイルを選択", "Assets", "mp3,wav");
        if (string.IsNullOrEmpty(assetPath))
        {
            EditorUtility.DisplayDialog("キャンセル", "ファイル選択がキャンセルされました。", "OK");
            return;
        }

        if (!assetPath.EndsWith(".mp3") && !assetPath.EndsWith(".wav"))
        {
            EditorUtility.DisplayDialog("エラー", "MP3またはWAVファイルを選択してください。", "OK");
            return;
        }

        // 相対パスに変換（Assetsフォルダ以下であることが必要）
        string relativePath = "Assets" + assetPath.Substring(Application.dataPath.Length);

        AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(relativePath);
        if (clip == null)
        {
            EditorUtility.DisplayDialog("エラー", "AudioClipを読み込めませんでした。", "OK");
            return;
        }

        string folder = Path.GetDirectoryName(assetPath);
        string baseName = Path.GetFileNameWithoutExtension(assetPath);

        // 分割してWAVファイルを生成
        AudioClip part1 = CreateSubClip(clip, 0f, splitTime);
        AudioClip part2 = CreateSubClip(clip, splitTime, clip.length);

        string path1 = Path.Combine(folder, baseName + "_part1.wav");
        string path2 = Path.Combine(folder, baseName + "_part2.wav");

        File.WriteAllBytes(path1, WavUtility.FromAudioClip(part1, out _, true));
        File.WriteAllBytes(path2, WavUtility.FromAudioClip(part2, out _, true));
        AssetDatabase.Refresh();

        // MP3変換（FFmpegが必要）
        ConvertWavToMp3(path1);
        ConvertWavToMp3(path2);

        EditorUtility.DisplayDialog("完了", "MP3ファイルを生成しました。", "OK");
    }

    static AudioClip CreateSubClip(AudioClip original, float startTime, float endTime)
    {
        int frequency = original.frequency;
        int channels = original.channels;

        int startSample = Mathf.FloorToInt(startTime * frequency);
        int endSample = Mathf.FloorToInt(endTime * frequency);
        int lengthSamples = endSample - startSample;

        float[] data = new float[lengthSamples * channels];
        original.GetData(data, startSample);

        AudioClip newClip = AudioClip.Create(original.name + "_cut", lengthSamples, channels, frequency, false);
        newClip.SetData(data, 0);

        return newClip;
    }

    static void ConvertWavToMp3(string wavPath)
    {
        string mp3Path = Path.ChangeExtension(wavPath, ".mp3");

        string ffmpeg = "ffmpeg";
        string args = $"-y -i \"{wavPath}\" \"{mp3Path}\"";

        var process = new Process();
        process.StartInfo.FileName = ffmpeg;
        process.StartInfo.Arguments = args;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.UseShellExecute = false;
        process.Start();
        process.WaitForExit();

        File.Delete(wavPath);
    }
}