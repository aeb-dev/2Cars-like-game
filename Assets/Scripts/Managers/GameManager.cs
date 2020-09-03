using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    START,
    GAME_OVER,
    RESUME,
    PAUSE
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Road road;
    public Car leftCar;
    public Car rightCar;
    public Camera cameraPrefab;
    public float speed;

    [HideInInspector] public GameState gameState = GameState.START;

    private float spriteHeight;
    private Queue<Road> roads;
    private Transform gameHolder;

    private int score = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);

        gameHolder = new GameObject("game").transform;

        roads = new Queue<Road>();
    }

    void Start()
    {
        InitGame();
    }

    void Update()
    {
        if (roads.Any())
        {
            Road road = roads.Peek();
            var sprite = road.GetComponent<SpriteRenderer>();
            var roadScript = road.GetComponent<Road>();
            bool isInView = Camera.main.IsObjectVisible(sprite);
            if (!isInView)
            {
                roads.Dequeue();
                road.transform.position = new Vector3(0, road.transform.position.y + 2 * spriteHeight - 0.1f, -0.25f);
                roadScript.CreateNewObstacles();
                roads.Enqueue(road);

                road = roads.Peek();
                road.transform.position = new Vector3(0, road.transform.position.y, 0);
            }
        }
    }

    void InitGame()
    {
        InitRoad();
        InitCamera();
        InitPlayer();
    }

    void InitPlayer()
    {
        var leftCar = Instantiate(this.leftCar, new Vector3(-4.05f, 0.5f, -0.5f), Quaternion.identity, gameHolder);
        leftCar.transform.SetParent(Camera.main.transform);

        var rightCar = Instantiate(this.rightCar, new Vector3(1.35f, 0.5f, -0.5f), Quaternion.identity, gameHolder);
        rightCar.transform.SetParent(Camera.main.transform);
    }

    void InitCamera()
    {
        Instantiate(cameraPrefab, new Vector3(0, 4, -10), Quaternion.identity, gameHolder);
    }

    void InitRoad()
    {
        var firstRoad = Instantiate(road, new Vector2(0, 0), Quaternion.identity, gameHolder);
        var spriteInfo = firstRoad.GetComponent<SpriteRenderer>();
        spriteHeight = spriteInfo.bounds.size.y;

        var secondRoad = Instantiate(road, new Vector2(0, spriteHeight - 0.05f), Quaternion.identity, gameHolder);
        var roadScript = secondRoad.GetComponent<Road>();
        roadScript.CreateNewObstacles();

        roads.Enqueue(firstRoad);
        roads.Enqueue(secondRoad);
    }

    void ResetCamera()
    {
        Camera.main.transform.position = new Vector3(0, 4, -10);
        var cars = Camera.main.GetComponentsInChildren<Car>();

        foreach (var car in cars)
        {
            car.Reset();
        }
    }

    void ResetRoads()
    {
        var firstRoad = roads.Dequeue();
        firstRoad.transform.position = new Vector3(0, 0);
        firstRoad.ClearObstacles();

        var secondRoad = roads.Dequeue();
        secondRoad.transform.position = new Vector2(0, spriteHeight - 0.05f);
        secondRoad.CreateNewObstacles();

        roads.Enqueue(firstRoad);
        roads.Enqueue(secondRoad);
    }

    public int UpdateScore(int value)
    {
        score += value;
        return score;
    }

    public void Play()
    {
        gameState = GameState.RESUME;
    }

    public void GameOver()
    {
        gameState = GameState.GAME_OVER;

        UIManager.instance.GameOver(this.score);
    }

    public void Restart()
    {
        this.score = 0;

        ResetCamera();
        ResetRoads();
        gameState = GameState.RESUME;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        gameState = GameState.PAUSE;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        gameState = GameState.RESUME;
    }
}
