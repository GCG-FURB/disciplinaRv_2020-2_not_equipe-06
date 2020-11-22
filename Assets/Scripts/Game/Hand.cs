using CodeMonkey;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public Transform HandMesh;

    private void Update()
    {
        HandMesh.position = Vector3.Lerp(HandMesh.position, transform.position, Time.deltaTime * 15.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cano"))
            CMDebug.TextPopupMouse("encostou caralho ae porra");
    }
}
