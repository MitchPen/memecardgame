using UnityEngine;

namespace Game.Core.AssetsProvider.SpriteCreator
{
    public class SpriteBuilder: ISpriteBuilder
    {
        public Sprite BuildSprite(Texture2D texture2D)
        {
            Sprite sprite = Sprite.Create(
                texture2D,
                new Rect(0, 0, texture2D.width, texture2D.height),
                new Vector2(0.5f, 0.5f),
                100f,
                0,
                SpriteMeshType.FullRect,
                Vector4.zero,
                false
            );
            sprite.name = texture2D.name;
            return sprite;
        }
    }
}
