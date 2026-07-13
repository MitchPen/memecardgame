using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Core.AssetsProvider.ContentLoaders
{
    public abstract class BaseLocalContentLoader<T> : IContentLoader<T> where T : ContentLoaderResult
    {
        public async virtual UniTask<T> LoadContent(string link)
        {
            throw new System.NotImplementedException();
        }

        protected bool ValidatePath(string filePath, string[] allowedTypes, out string resultPath)
        {
            resultPath = filePath;
            if (resultPath.Contains(".."))
            {
                Debug.LogWarning("Relative path not supported: ..");
                return false;
            }

            string ext = Path.GetExtension(resultPath).ToLower();

            if (!allowedTypes.Contains(ext))
            {
                Debug.LogWarning("Format not supported: " + ext);
                return false;
            }

            return true;
        }

        protected string ConvertToFileRequest(string path)
        {
            return "file://" + path.Replace("\\", "/");
        }
    }
}