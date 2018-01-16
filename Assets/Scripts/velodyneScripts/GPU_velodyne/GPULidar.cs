using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GPULidar : MonoBehaviour
{
    public Shader Shade;
    Material material;
    public float depthLevel = 1;

    // Use this for initialization
    void Start()
    {
        material = new Material(Shade);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (Shade != null)
        {
            material.SetFloat("_DepthLevel", depthLevel);
            Graphics.Blit(src, dest, material);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }

}
