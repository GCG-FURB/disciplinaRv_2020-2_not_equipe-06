using System.Collections;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public Sprite BubbleSprite;
    public Sprite PopSprite;

    [HideInInspector]
    public BubbleManager _bubbleManager = null;

    private Vector3 _movementDirection = Vector3.zero;
    private SpriteRenderer _spriteRenderer = null;
    private Coroutine _currentChanger = null;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _currentChanger = StartCoroutine(DirectionChanger());
    }

    private void OnBecameInvisible()
    {
        transform.position = _bubbleManager.GetPlanePosition();
    }

    private void Update()
    {
        transform.position += _movementDirection * Time.deltaTime * 0.35f;
        transform.Rotate(Vector3.forward * Time.deltaTime * _movementDirection.x * 20, Space.Self);
    }

    public IEnumerator Pop()
    {
        _spriteRenderer.sprite = PopSprite;

        StopCoroutine(_currentChanger);
        _movementDirection = Vector3.zero;

        yield return new WaitForSeconds(0.5f);

        transform.position = _bubbleManager.GetPlanePosition();

        _spriteRenderer.sprite = BubbleSprite;
        _currentChanger = StartCoroutine(DirectionChanger());
    }

    private IEnumerator DirectionChanger()
    {
        while (gameObject.activeSelf)
        {
            _movementDirection = new Vector2(Random.Range(-100, 100) * 0.01f, Random.Range(0, 100) * 0.01f);

            yield return new WaitForSeconds(5.0f);
        }
    }
}
