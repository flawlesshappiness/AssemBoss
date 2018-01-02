using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MakeSprite : AssetPostprocessor {

	void OnPreprocessTexture()
	{
		if(assetPath.Contains("Sprites"))
		{
			Debug.Log("MakeSprite: Importing and making new sprite...");
			TextureImporter imp = (TextureImporter)assetImporter;
			imp.textureType = TextureImporterType.Sprite;
			imp.spritePixelsPerUnit = 64;
			imp.filterMode = FilterMode.Point;
			imp.maxTextureSize = 2048;
		}

		if(assetPath.Contains("Textures"))
		{
			Debug.Log("MakeSprite: Importing and making new texture...");
			TextureImporter imp = (TextureImporter)assetImporter;
			imp.textureType = TextureImporterType.Default;
			imp.textureShape = TextureImporterShape.Texture2D;
			imp.wrapMode = TextureWrapMode.Repeat;
			imp.filterMode = FilterMode.Point;
			imp.maxTextureSize = 2048;
		}
	}
}
