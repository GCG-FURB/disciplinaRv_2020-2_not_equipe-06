using CodeMonkey;
using UnityEngine;

public class BodyJoint : MonoBehaviour
{
    public Transform BodyMesh;

    private void Update()
    {
        BodyMesh.position = Vector3.Lerp(BodyMesh.position, transform.position, Time.deltaTime * 15.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pipe"))
        {
            CMDebug.TextPopupMouse("Perdeu!");
        }
    }
}
