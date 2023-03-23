using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct NerveTextures
{
    public Texture cutTextureAlbedo;
    public Texture cutTextureEmission;
}

public class SwapTextures : MonoBehaviour {

    public NerveTextures[] nerveTextures;
    private Material mat;

	void Start ()
    {
        mat = GetComponent<Renderer>().material;
        setHealthyTexture();
    }

    public void ChangeToCutTexture(int textureIndex)
    {
        if (mat == null)
        {
            mat = GetComponent<Renderer>().material;
        }

        mat.SetTexture("_MainTex", nerveTextures[textureIndex].cutTextureAlbedo);
        mat.SetTexture("_EmissionMap", nerveTextures[textureIndex].cutTextureEmission);
    }

    public void setHealthyTexture()
    {
        if (mat == null)
        {
            mat = GetComponent<Renderer>().material;
        }

        mat.SetTexture("_MainTex", nerveTextures[0].cutTextureAlbedo);
        mat.SetTexture("_EmissionMap", nerveTextures[0].cutTextureEmission);
    }
}
