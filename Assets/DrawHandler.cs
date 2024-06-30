using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawHandler : MonoBehaviour
{
    public Camera cam;

    GameOfLife gof;

    public bool white;
    // Update is called once per frame
    private void Start()
    {
        gof = GetComponent<GameOfLife>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0) ||Input.GetMouseButton(1))
        {
            bool w = white;
            if (Input.GetMouseButton(1)) { w = !w; }

            RaycastHit hit;
            if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
                return;

            Renderer rend = hit.transform.GetComponent<Renderer>();
            MeshCollider meshCollider = hit.collider as MeshCollider;

            if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
                return;

            Texture2D tex = rend.material.mainTexture as Texture2D;
            Vector2 pixelUV = hit.textureCoord;
            pixelUV.x *= tex.width;
            pixelUV.y *= tex.height;

            if (w)
            {
                tex.SetPixel((int)pixelUV.x, (int)pixelUV.y, Color.white);
                gof.DrawupdateGrid((int)pixelUV.x, (int)pixelUV.y, true);
            }
            else
            {
                tex.SetPixel((int)pixelUV.x, (int)pixelUV.y, Color.black);
                gof.DrawupdateGrid((int)pixelUV.x, (int)pixelUV.y, false);
            }


            tex.Apply();
        }
    }
}
