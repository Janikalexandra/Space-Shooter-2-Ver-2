using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    [SerializeField]
    [Header("Speed")]
    [Tooltip("Backgrounds scrolling speed")]
    private float _scrollingSpeed = 1.3f;

    private Vector3 _startPosition;

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _scrollingSpeed * Time.deltaTime);
        if(transform.position.y < -39.61f)
        {
            transform.position = _startPosition;
        }
    }
}