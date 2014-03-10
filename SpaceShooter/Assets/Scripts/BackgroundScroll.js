var scrollSpeed : float;

function Update () {
    renderer.material.SetTextureOffset("_MainTex", Vector2(0, Time.time * scrollSpeed));
}