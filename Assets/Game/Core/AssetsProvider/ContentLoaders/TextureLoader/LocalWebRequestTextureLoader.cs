using System;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Game.Core.AssetsProvider.ContentLoaders.TextureLoader
{
    public class LocalWebRequestTextureLoader : BaseLocalContentProvider<TextureLoaderResult>
    {
        private const string PNG_EXTENSION = ".png";
        private const string JPG_EXTENSION = ".jpg";
        private const string JPEG_EXTENSION = ".jpeg";
        private string[] _allowedType = { PNG_EXTENSION, JPG_EXTENSION, JPEG_EXTENSION };

        public override async UniTask<TextureLoaderResult> LoadContent(string link)
        {
            if (!ValidatePath(link, _allowedType, out string url))
                return ReturnFailure();

            using UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            
            try
            {
                await request.SendWebRequest();
                
                if (request.result == UnityWebRequest.Result.Success)
                {
                    var texture = DownloadHandlerTexture.GetContent(request);
                    texture.name = Path.GetFileName(url);
                    if (texture.width <= 0 || texture.height <= 0)
                        return ReturnFailure();

                    return new TextureLoaderResult()
                    {
                        Result = true,
                        Texture = texture,
                    };
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Load image request failed: {e.Message} _:" + request.error);
            }
            
            return ReturnFailure();
            
            TextureLoaderResult ReturnFailure()
            {
                Debug.LogError("Load image failed");
                return new TextureLoaderResult()
                {
                    Result = false,
                    Texture = null,
                };
            }
        }
    }
}