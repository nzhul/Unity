using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ChessboardBuilder : MonoBehaviour
{

    public float cellsize = 1f;

    [Range(2, 100)]
    public int width = 8;
    [Range(2, 100)]
    public int height = 8;

    private void Update()
    {
        MeshFilter filter = this.GetComponent<MeshFilter>();
        MeshBuilder mb = new MeshBuilder(6);

        Vector3[,] points = new Vector3[width, height];

        float halfCellSize = cellsize / 2;
        int halfWidth = width / 2;
        int halfHeight = height / 2;

        for (int x = -halfWidth; x < halfWidth; x++)
        {
            for (int y = -halfHeight; y < halfHeight; y++)
            {
                points[x + halfWidth, y + halfHeight] = new Vector3(
                    (cellsize * x) + halfCellSize,
                    0,
                    (cellsize * y) + halfCellSize);
            }
        }

        int submesh = 0;

        for (int x = 0; x < width - 1; x++)
        {
            for (int y = 0; y < height - 1; y++)
            {
                submesh++;

                Vector3 br = points[x, y];
                Vector3 bl = points[x + 1, y];
                Vector3 tr = points[x, y + 1];
                Vector3 tl = points[x + 1, y + 1];

                mb.BuildTriangle(bl, tr, tl, submesh % 2);
                mb.BuildTriangle(bl, br, tr, submesh % 2);
            }
        }

        filter.mesh = mb.CreateMesh();

    }


}
