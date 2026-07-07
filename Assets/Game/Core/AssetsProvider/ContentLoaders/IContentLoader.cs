using Cysharp.Threading.Tasks;

namespace Game.Core.AssetsProvider.ContentLoaders
{
    public interface IContentLoader<T> where T : ContentLoaderResult
    {
        public UniTask<T> LoadContent(string link);
    }
}