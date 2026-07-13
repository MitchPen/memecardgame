using Cysharp.Threading.Tasks;
using Game.Core.AssetsProvider.ContentLoaders;
using Game.Core.AssetsProvider.ContentLoaders.AudioLoader;
using Game.Core.AssetsProvider.ContentLoaders.TextureLoader;
using Game.Core.AssetsProvider.SpriteCreator;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.AssetsProvider
{
    public class AssetDebug : MonoBehaviour
    {
        [SerializeField] private RawImage _rawImage;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private AudioSource _source;
        [SerializeField] private string _imagePath;
        [SerializeField] private string _audioPath;

        [EasyButtons.Button]
        public void ApplyImage()
        {
            _ = LoadImage();
        }
        
        [EasyButtons.Button]
        public void ApplySprite()
        {
            _ = LoadSprite();
        }

        [EasyButtons.Button]
        public void PlayAudio()
        {
            _ = LoadAudio();
        }

        private async UniTaskVoid LoadImage()
        {
            IContentLoader<TextureLoaderResult> reader = new ByteTextureLoader();
            var readerResult = await reader.LoadContent(_imagePath);
            if (readerResult.Result)
            {
                _rawImage.texture = readerResult.Texture;
                _rawImage.SetNativeSize();
            }
        }
        
        private async UniTaskVoid LoadSprite()
        {
            IContentLoader<TextureLoaderResult> reader = new LocalWebRequestTextureLoader();
            var readerResult = await reader.LoadContent(_imagePath);
            if (readerResult.Result)
            {
                ISpriteBuilder spriteBuilder = new SpriteBuilder();
                var sprite = spriteBuilder.BuildSprite(readerResult.Texture);
                if (sprite != null)
                {
                    _spriteRenderer.sprite = sprite;
                }
            }
        }

        private async UniTaskVoid LoadAudio()
        {
            IContentLoader<AudioLoaderResult> reader = new LocalWebRequestAudioLoader();
            var readerResult = await reader.LoadContent(_audioPath);
            if (readerResult.Result)
            {
                _source.clip = readerResult.AudioClip;
                _source.Play();
            }
        }
    }
}