using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class LandscapeBuilder : MonoBehaviour
{

    public float cellsize = 1f;
    public int width = 24;
    public int height = 24;

    public float bumpyness = 3f;
    public float bumpheight = 3f;

    private void Update()
    {
        MeshFilter filter = GetComponent<MeshFilter>();
        MeshBuilder mb = new MeshBuilder(6);

        Vector3[,] points = new Vector3[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                points[x, y] = new Vector3(
                    x * cellsize,
                    Mathf.PerlinNoise(
                        (x + Time.time) * bumpyness * .1f,
                        (y + Time.time) * bumpyness * .1f)
                        * bumpheight,
                    y * cellsize);
            }
        }

        int submesh = 0;

        for (int x = 0; x < width - 1; x++)
        {
            for (int y = 0; y < height - 1; y++)
            {
                Vector3 br = points[x, y];
                Vector3 bl = points[x + 1, y];
                Vector3 tr = points[x, y + 1];
                Vector3 tl = points[x + 1, y + 1];

                mb.BuildTriangle(bl, tr, tl, submesh % 6);
                mb.BuildTriangle(bl, br, tr, submesh % 6);
            }
        }

        filter.mesh = mb.CreateMesh();
    }

}
