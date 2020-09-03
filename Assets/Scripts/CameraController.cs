using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool isMoving = false;
    private float inverseMoveTime;

    void Start()
    {
        inverseMoveTime = 1f / GameManager.instance.speed;
    }

    void Update()
    {
        if (!isMoving && GameManager.instance.gameState == GameState.RESUME)
        {
            StartCoroutine(SmoothMovement());
        }
    }

    protected IEnumerator SmoothMovement()
    {
        var end = transform.position + Vector3.up;
        isMoving = true;
        while (Vector3.Distance(end, transform.position) > float.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, end, inverseMoveTime * Time.deltaTime);

            yield return null;
        }

        isMoving = false;
    }
}
