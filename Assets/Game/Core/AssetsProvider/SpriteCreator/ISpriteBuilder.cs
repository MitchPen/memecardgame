using UnityEngine;

namespace Game.Core.AssetsProvider.SpriteCreator
{
    public interface ISpriteBuilder
    {
        public Sprite BuildSprite(Texture2D texture2D);
    }
}
