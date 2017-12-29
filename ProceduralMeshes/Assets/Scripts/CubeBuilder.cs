using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CubeBuilder : MonoBehaviour
{

    MeshFilter filter;
    MeshBuilder mb;
    public Vector3 size = Vector3.one;

    // Use this for initialization
    void Start()
    {
        filter = GetComponent<MeshFilter>();
    }

    // Update is called once per frame
    void Update()
    {
        mb = new MeshBuilder(6);

        Vector3 cubeSize = size * .5f;

        // top coordinates
        Vector3 t0 = new Vector3(cubeSize.x, cubeSize.y, -cubeSize.z);
        Vector3 t1 = new Vector3(-cubeSize.x, cubeSize.y, -cubeSize.z);
        Vector3 t2 = new Vector3(-cubeSize.x, cubeSize.y, cubeSize.z);
        Vector3 t3 = new Vector3(cubeSize.x, cubeSize.y, cubeSize.z);

        // bottom coordinates
        Vector3 b0 = new Vector3(cubeSize.x, -cubeSize.y, -cubeSize.z);
        Vector3 b1 = new Vector3(-cubeSize.x, -cubeSize.y, -cubeSize.z);
        Vector3 b2 = new Vector3(-cubeSize.x, -cubeSize.y, cubeSize.z);
        Vector3 b3 = new Vector3(cubeSize.x, -cubeSize.y, cubeSize.z);

        // top square
        mb.BuildTriangle(t0, t1, t2, 0);
        mb.BuildTriangle(t0, t2, t3, 0);

        // bottom square
        mb.BuildTriangle(b2, b1, b0, 1);
        mb.BuildTriangle(b3, b2, b0, 1);

        // back
        mb.BuildTriangle(b0, t1, t0, 2);
        mb.BuildTriangle(b0, b1, t1, 2);

        mb.BuildTriangle(b1, t2, t1, 3);
        mb.BuildTriangle(b1, b2, t2, 3);

        mb.BuildTriangle(b2, t3, t2, 4);
        mb.BuildTriangle(b2, b3, t3, 4);

        mb.BuildTriangle(b3, t0, t3, 5);
        mb.BuildTriangle(b3, b0, t0, 5);

        filter.mesh = mb.CreateMesh();
    }
}
