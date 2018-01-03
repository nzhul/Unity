using UnityEngine;

[System.Serializable]
public class PathShape
{
    public Vector3[] shape = new Vector3[] { -Vector3.up, Vector3.up, -Vector3.up };
}

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class PathBuilder : MonoBehaviour
{

    

}
