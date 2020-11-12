using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    public GameObject BubblePrefab;

    private readonly List<Bubble> _allBubbles = new List<Bubble>();
    private Vector2 _bottomLeft = Vector2.zero;
    private Vector2 _topRight = Vector2.zero;

    private void Awake()
    {
        _bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.farClipPlane));
        _topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight / 2, Camera.main.farClipPlane));
    }

    private void Start()
    {
        StartCoroutine(CreateBubbles());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.farClipPlane)), 0.5f);
        Gizmos.DrawSphere(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, Camera.main.farClipPlane)), 0.5f);
    }

    public Vector3 GetPlanePosition()
    {
        float targetX = Random.Range(_bottomLeft.x, _topRight.x);
        float targetY = Random.Range(_bottomLeft.y, _topRight.y);

        return new Vector3(targetX, targetY, 0);
    }

    private IEnumerator CreateBubbles()
    {
        while (_allBubbles.Count < 20)
        {
            GameObject newBubbleObject = Instantiate(BubblePrefab, GetPlanePosition(), Quaternion.identity, transform);
            Bubble newBubble = newBubbleObject.GetComponent<Bubble>();

            newBubble._bubbleManager = this;
            _allBubbles.Add(newBubble);

            yield return new WaitForSeconds(0.5f);
        }
    }
}
