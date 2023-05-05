using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    private RectTransform left;
    private RectTransform right;
    private Vector2 leftOriginalPos;
    private Vector2 rightOriginalPos;
    private bool open;
    public bool close;

    // Start is called before the first frame update
    void Start()
    {
        left = transform.Find("Left").GetComponent<RectTransform>();
        right = transform.Find("Right").GetComponent<RectTransform>();
        leftOriginalPos = left.anchoredPosition;
        rightOriginalPos = right.anchoredPosition;
        open = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (open) {
            left.anchoredPosition = Vector2.MoveTowards(
                left.anchoredPosition,
                new Vector2(-100.0f, left.anchoredPosition.y),
                1200.0f * Time.deltaTime);
            right.anchoredPosition = Vector2.MoveTowards(
                right.anchoredPosition,
                new Vector2(100.0f, right.anchoredPosition.y),
                1200.0f * Time.deltaTime);
        }

        if (!(left.anchoredPosition.x > -100.0f && right.anchoredPosition.x < 100.0f))
            open = false;

        if (close) {
            left.anchoredPosition = Vector2.MoveTowards(
                left.anchoredPosition,
                leftOriginalPos,
                1200.0f * Time.deltaTime);
            right.anchoredPosition = Vector2.MoveTowards(
                right.anchoredPosition,
                rightOriginalPos,
                1200.0f * Time.deltaTime);
        }
    }
}
