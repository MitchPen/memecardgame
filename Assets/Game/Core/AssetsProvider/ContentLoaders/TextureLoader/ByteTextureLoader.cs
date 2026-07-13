using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Core.AssetsProvider.ContentLoaders.TextureLoader
{
    public class ByteTextureLoader: BaseLocalContentLoader<TextureLoaderResult>
    {
        private const string PNG_EXTENSION = ".png";
        private const string JPG_EXTENSION = ".jpg";
        private const string JPEG_EXTENSION = ".jpeg";
        private string[] _allowedType = { PNG_EXTENSION, JPG_EXTENSION, JPEG_EXTENSION };
        
        public override async UniTask<TextureLoaderResult> LoadContent(string link)
        {
            if (!ValidatePath(link, _allowedType, out _ ))
                return ReturnFailure();

            if (File.Exists(link))
            {
                byte[] fileBytes =await File.ReadAllBytesAsync(link);
                Texture2D newTexture = new Texture2D(2, 2);
                bool success = newTexture.LoadImage(fileBytes, false);
                if (success)
                {
                    if (newTexture.width <= 0 || newTexture.height <= 0)
                        return ReturnFailure(": Wrong image resolution");
                    
                    return new TextureLoaderResult()
                    {
                        Result = true,
                        Texture = newTexture,
                    };
                }
            }
            else
                Debug.LogError($"{nameof(ByteTextureLoader)}: File not exist ");
            
            Debug.LogError($"{nameof(ByteTextureLoader)}: Failed to read file");
            
            return ReturnFailure();
            
            TextureLoaderResult ReturnFailure(string additionalInfo = "")
            {
                Debug.LogError($"{nameof(ByteTextureLoader)}: Load image failed"+ additionalInfo);
                return new TextureLoaderResult()
                {
                    Result = false,
                    Texture = null,
                };
            }
        }
    }
}
