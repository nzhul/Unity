using System.Collections.Generic;
using UnityEngine;

public class MeshBuilder
{
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();

    private List<Vector3> normals = new List<Vector3>();
    private List<Vector2> uvs = new List<Vector2>();

    private List<int>[] submeshTriangles = new List<int>[] { };

    public MeshBuilder(int submeshCount)
    {
        submeshTriangles = new List<int>[submeshCount];
        for (int i = 0; i < submeshCount; i++)
        {
            submeshTriangles[i] = new List<int>();
        }
    }

    public void BuildTriangle(Vector3 p0, Vector3 p1, Vector3 p2, int submesh)
    {
        Vector3 normal = Vector3.Cross(p1 - p0, p2 - p0).normalized;
        this.BuildTriangle(p0, p1, p2, normal, submesh);
    }

    public void BuildTriangle(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 normal, int submesh)
    {
        int p0Index = vertices.Count;
        int p1Index = vertices.Count + 1;
        int p2Index = vertices.Count + 2;

        triangles.Add(p0Index);
        triangles.Add(p1Index);
        triangles.Add(p2Index);

        submeshTriangles[submesh].Add(p0Index);
        submeshTriangles[submesh].Add(p1Index);
        submeshTriangles[submesh].Add(p2Index);

        this.vertices.Add(p0);
        this.vertices.Add(p1);
        this.vertices.Add(p2);

        this.normals.Add(normal);
        this.normals.Add(normal);
        this.normals.Add(normal);

        this.uvs.Add(new Vector2(0, 0));
        this.uvs.Add(new Vector2(0, 1));
        this.uvs.Add(new Vector2(1, 1));
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();

        mesh.vertices = this.vertices.ToArray();
        mesh.triangles = this.triangles.ToArray();

        mesh.normals = this.normals.ToArray();
        mesh.uv = this.uvs.ToArray();

        mesh.subMeshCount = submeshTriangles.Length;

        for (int i = 0; i < submeshTriangles.Length; i++)
        {
            if (submeshTriangles[i].Count < 3)
            {
                mesh.SetTriangles(new int[3] { 0, 0, 0 }, i);
            }
            else
            {
                mesh.SetTriangles(submeshTriangles[i], i);
            }
        }

        return mesh;
    }




}