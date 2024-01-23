using UnityEngine;

[CreateAssetMenu(fileName = "RandomTextureData", menuName = "Random Texture/RandomTextureData", order = 4)]
public class RandomTextureData : ScriptableObject
{
    [SerializeField] private Texture[] _textureArray;

    public Texture[] TextureArray => _textureArray;

    public Texture GetRandomTexture()
    {
        int randomIndex = Random.Range(0, _textureArray.Length);
        return _textureArray[randomIndex];
    }
}
