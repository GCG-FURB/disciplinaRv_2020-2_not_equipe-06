using UnityEngine;

public class MaterialPanner : MonoBehaviour
{
    private MeshRenderer _meshRenderer = null;
    private Vector2 _offset = Vector2.zero;

    public void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Update()
    {
        _offset.x += 0.075f * Time.deltaTime;
        _meshRenderer.material.SetTextureOffset("_MainTex", _offset);
    }
}
