using System;
using UnityEngine;

public class HexMapCamera : MonoBehaviour {

    Transform swivel, stick;
    float zoom = 1f;
    public float stickMinZoom, stickMaxZoom;
    public float swivelMinZoom, swivelMaxZoom;
    public float moveSpeed;

    private void Awake()
    {
        swivel = transform.GetChild(0);
        stick = swivel.GetChild(0);
    }

    private void Update()
    {
        float zoomDelta = Input.GetAxis("Mouse ScrollWheel");

        if (zoomDelta != 0f)
        {
            AdjustZoom(zoomDelta);
        }

        float xDelta = Input.GetAxis("Horizontal");
        float zDeleta = Input.GetAxis("Vertical");

        if (xDelta != 0f || zDeleta != 0f)
        {
            AdjustPosition(xDelta, zDeleta);
        }
    }

    private void AdjustPosition(float xDelta, float zDeleta)
    {
        float distance = moveSpeed * Time.deltaTime;

        Vector3 position = transform.localPosition;
        position += new Vector3(xDelta, 0f, zDeleta) * distance;
        transform.localPosition = position;
    }

    private void AdjustZoom(float delta)
    {
        zoom = Mathf.Clamp01(zoom + delta);

        float distance = Mathf.Lerp(stickMinZoom, stickMaxZoom, zoom);
        stick.localPosition = new Vector3(0f, 0f, distance);

        float angle = Mathf.Lerp(swivelMinZoom, swivelMaxZoom, zoom);
        swivel.localRotation = Quaternion.Euler(angle, 0f, 0f);
    }
}
