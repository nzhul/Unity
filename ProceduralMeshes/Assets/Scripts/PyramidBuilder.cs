using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PyramidBuilder : MonoBehaviour {

    public float topPosition = 5f;
    public float size = 5f;

    public float r1 = 0f;
    public float r2 = 240f;
    public float r3 = 120f;

    private void Update()
    {
        MeshFilter filter = GetComponent<MeshFilter>();
        MeshBuilder mb = new MeshBuilder(4);

        Vector3 top = new Vector3(0, topPosition, 0);

        Vector3 b0 = Quaternion.AngleAxis(r1, Vector3.up) * Vector3.forward * size;
        Vector3 b1 = Quaternion.AngleAxis(r2, Vector3.up) * Vector3.forward * size;
        Vector3 b2 = Quaternion.AngleAxis(r3, Vector3.up) * Vector3.forward * size;

        mb.BuildTriangle(b0, b1, b2, 0);
        mb.BuildTriangle(b1, b0, top, 1);
        mb.BuildTriangle(b2, top, b0, 2);
        mb.BuildTriangle(top, b2, b1, 3);

        filter.mesh = mb.CreateMesh();
    }

}
