using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Car : MonoBehaviour //, IPointerClickHandler
{
    private float inverseMoveTime;
    private bool isLeft = true;
    private bool shouldSwitchLane = false;
    private bool shouldRotate = false;
    private bool isRotating = false;
    private bool isSwitchingLane = false;
    private Vector3 lane;
    private Quaternion rotation;
    private Vector3 initialPosition;

    void Start()
    {
        inverseMoveTime = 1f / GameManager.instance.speed;

        lane = transform.position;

        initialPosition = transform.position;
    }

    void Update()
    {

        // if (!isMoving)
        // {
        //     StartCoroutine(SmoothMovementY());
        // }
        if (GameManager.instance.gameState == GameState.RESUME && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            var mp = Input.mousePosition;
            Vector3 p = Camera.main.ScreenToWorldPoint(mp);
            if ((p.x < 0 && this.transform.position.x < 0) || (p.x > 0 && this.transform.position.x > 0))
            {
                this.Move();
            }
        }

        if (shouldSwitchLane && !isSwitchingLane)
        {
            StartCoroutine(SmoothMovementX());
        }

        if (shouldRotate && !isRotating)
        {
            StartCoroutine(SmoothRotate());
        }
    }

    // public void OnPointerClick(PointerEventData eventData)
    // {
    //     if (GameManager.instance.gameState == GameState.RESUME)
    //     {
    //         var mp = Input.mousePosition;
    //         Vector3 p = Camera.main.ScreenToWorldPoint(mp);
    //         if ((p.x < 0 && this.transform.position.x < 0) || (p.x > 0 && this.transform.position.x > 0))
    //         {
    //             this.Move();
    //         }
    //     }
    // }

    // protected IEnumerator SmoothMovementY()
    // {
    //     var y = new Vector3(0, transform.position.y, 0);
    //     var end = y + Vector3.up;
    //     isMoving = true;
    //     while (Vector3.Distance(end, y) > float.Epsilon)
    //     {
    //         Vector3 newPosition = Vector3.MoveTowards(y, end, inverseMoveTime * Time.deltaTime);
    //         transform.position = new Vector3(transform.position.x, newPosition.y, transform.position.z);
    //         y = newPosition;
    //         yield return null;
    //     }

    //     isMoving = false;
    // }

    protected IEnumerator SmoothMovementX()
    {
        var x = new Vector3(transform.position.x, 0, 0);
        shouldSwitchLane = false;
        isSwitchingLane = true;
        while (Vector3.Distance(lane, x) > float.Epsilon)
        {
            if (!(GameManager.instance.gameState == GameState.RESUME || GameManager.instance.gameState == GameState.PAUSE))
            {
                break;
            }

            Vector3 newPosition = Vector3.MoveTowards(x, lane, inverseMoveTime * Time.deltaTime);
            transform.position = new Vector3(newPosition.x, transform.position.y, transform.position.z);
            x = newPosition;

            yield return null;
        }

        isSwitchingLane = false;
    }

    protected IEnumerator SmoothRotate()
    {
        shouldRotate = false;
        isRotating = true;
        while (Quaternion.Angle(rotation, transform.rotation) > float.Epsilon)
        {
            if (!(GameManager.instance.gameState == GameState.RESUME || GameManager.instance.gameState == GameState.PAUSE))
            {
                break;
            }

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, inverseMoveTime * 25 * Time.deltaTime);
            if (!(Quaternion.Angle(rotation, transform.rotation) > float.Epsilon))
            {
                isRotating = false;
                rotation = Quaternion.identity;
            }

            yield return null;
        }

        isRotating = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Block")
        {
            GameManager.instance.GameOver();
        }
        else if (collider.tag == "Collect")
        {
            collider.gameObject.SetActive(false);
            GameManager.instance.UpdateScore(1);
        }
    }

    public void Move()
    {
        if (isLeft)
        {
            this.lane += new Vector3(2.7f, 0, 0);
            rotation = isRotating ? Quaternion.identity : Quaternion.Euler(0, 0, -45);
            isLeft = false;
        }
        else
        {
            this.lane += new Vector3(-2.7f, 0, 0);
            rotation = isRotating ? Quaternion.identity : Quaternion.Euler(0, 0, 45);
            isLeft = true;
        }

        if (!isSwitchingLane)
        {
            shouldSwitchLane = true;
        }

        if (!isRotating)
        {
            shouldRotate = true;
        }
    }

    public void Reset()
    {
        isLeft = true;
        lane = initialPosition;
        transform.rotation = Quaternion.identity;
        transform.position = initialPosition;
    }
}
