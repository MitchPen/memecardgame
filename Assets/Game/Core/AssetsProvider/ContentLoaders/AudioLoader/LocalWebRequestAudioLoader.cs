using System;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Game.Core.AssetsProvider.ContentLoaders.AudioLoader
{
    public class LocalWebRequestAudioLoader : BaseLocalContentLoader<AudioLoaderResult>
    {
        private const float MINIMAL_AUDIO_LENGTH = 0.1f;
        private const string WAV_EXTENSION = ".wav";
        private const string MP3_EXTENSION = ".mp3";
        private const string MP2_EXTENSION = ".mp2";
        private string[] _allowedType = new[] { WAV_EXTENSION, MP3_EXTENSION, MP2_EXTENSION };

        public override async UniTask<AudioLoaderResult> LoadContent(string link)
        {
            if (!ValidatePath(ConvertToFileRequest(link),_allowedType, out string url))
                return ReturnFailure();

            using UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, GetFileType(url));

            try
            {
                await request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    var audioClip = DownloadHandlerAudioClip.GetContent(request);
                    audioClip.name = Path.GetFileName(url);
                    if (audioClip.length < MINIMAL_AUDIO_LENGTH)
                        return ReturnFailure(": Wrong audio lenght");

                    return new AudioLoaderResult()
                    {
                        Result = true,
                        AudioClip = audioClip,
                    };
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"{nameof(LocalWebRequestAudioLoader)} request failed: {e.Message} _:" + request.error);
            }
            
            return ReturnFailure();

            AudioLoaderResult ReturnFailure(string additionalInfo = "")
            {
                Debug.LogError($"{nameof(LocalWebRequestAudioLoader)}: Load audio failed"+additionalInfo);
                return new AudioLoaderResult()
                {
                    Result = false,
                    AudioClip = null,
                };
            }
        }

        private AudioType GetFileType(string filePath)
        {
            string ext = Path.GetExtension(filePath).ToLower();
            switch (ext)
            {
                case WAV_EXTENSION:
                {
                    return AudioType.WAV;
                }
                case MP3_EXTENSION:
                case MP2_EXTENSION:
                {
                    return AudioType.MPEG;
                }
                default: return AudioType.UNKNOWN;
            }
        }
    }
}