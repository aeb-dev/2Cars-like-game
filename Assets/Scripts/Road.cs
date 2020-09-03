using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    // public
    public GameObject obstacle;
    public Sprite[] redSprites;
    public Sprite[] blueSprites;

    private List<GameObject> obstacleList;

    void Awake()
    {
        obstacleList = new List<GameObject>();
    }

    void Update()
    {
    }

    public void ClearObstacles()
    {
        foreach (var oldObstacle in obstacleList)
        {
            Destroy(oldObstacle);
        }
        obstacleList.Clear();
    }

    public void CreateNewObstacles()
    {
        this.ClearObstacles();

        for (int index = 0; index < 3; ++index)
        {
            for (int carIndex = 0; carIndex < 2; ++carIndex)
            {
                int roadSide = Random.Range(0, 2);
                int spriteIndex = Random.Range(0, 2);

                float xPosition;
                if (roadSide == 0)
                {
                    xPosition = carIndex == 0 ? -4.05f : 4.05f;
                }
                else
                {
                    xPosition = carIndex == 0 ? -1.35f : 1.35f;
                }

                GameObject clone = Instantiate(obstacle, new Vector3(0, 0, 0), Quaternion.identity);
                clone.transform.SetParent(this.transform);
                clone.transform.localPosition = new Vector3(xPosition, index * 6 - 5f, 0);

                if (carIndex == 0)
                {
                    var cloneSprite = clone.GetComponent<SpriteRenderer>();
                    cloneSprite.sprite = redSprites[spriteIndex];
                }
                else
                {
                    var cloneSprite = clone.GetComponent<SpriteRenderer>();
                    cloneSprite.sprite = blueSprites[spriteIndex];
                }

                clone.gameObject.tag = spriteIndex == 0 ? "Collect" : "Block";

                obstacleList.Add(clone);
            }
        }
    }
}
