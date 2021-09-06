using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    public GameObject greenEnemy;
    public GameObject blueEnemy;
    public GameObject redEnemy;

    public int currentLevel;

    List<EnemyController> myShipsList = new List<EnemyController>();

    string mySpawnValue;

    [SerializeField]
    float speed;

    float timer;
    float currentPosX;
    float currentPosY;
    float oldPosY;
    float newPosY;

    float borderX;

    bool left = false;

    public AudioClip myDamageClip;

    public AudioClip myDeathClip;

    public void Start()
    {

        GameManager.instance.EnemyHandler = this;

    }


    private void Update()
    {
        switch (GameManager.instance.MyGameState)
        {
            case GameState.mainMenu:
                break;
            case GameState.gamePlay:
                GameplayFunction();
                break;
            case GameState.gameOver:
                break;
            default:
                break;
        }
    }
    /// <summary>
    ///  Restarts the values for the enemies formation 
    /// </summary>

    public void Init()
    {
        Spawn();

        borderX = (0.5f - CameraComponent.instance.CameraWidth / 2);
        currentPosX = borderX;
        currentPosY = (-0.5f + CameraComponent.instance.CameraHeight / 2);
        Vector3 startPosition = new Vector3(borderX, currentPosY, 0);

        this.transform.position = startPosition;
        currentPosY = startPosition.y;
        newPosY = currentPosY;
        oldPosY = newPosY;
        left = false;
    }

    /// <summary>
    ///  upong game over this method destroys every enemy in the screen
    /// </summary>
    public void ClearEnemies()
    {
        StartCoroutine(DestroyShips());
    }

    IEnumerator DestroyShips()
    {
        for (int i = 0; i < myShipsList.Count; i++)
        {
            if (myShipsList[i] != null && myShipsList[i].transform.parent != null)
            {
                myShipsList[i].MyAudioSource.volume = 0.1f;

                StartCoroutine(myShipsList[i].Die());
                yield return null;
            }
        }
    }

    /// <summary>
    ///  This method controls the movement of the enemies group
    /// </summary>
    void GameplayFunction()
    {
        if (this.transform.position.y == newPosY)
        {

            currentPosX += Time.deltaTime * speed * (left ? -1 : 1);
            if (left && (borderX - GetLeftMostShip()) > currentPosX)
            {


                left = false;
                oldPosY = newPosY;
                newPosY -= 0.5f;
                timer = 0;
            }
            else if (!left && ((-borderX - GetRightMostShip())) < currentPosX)
            {

                left = true;

                oldPosY = newPosY;
                newPosY -= 0.5f;
                timer = 0;
            }
        }
        else
        {
            timer += (speed * Time.deltaTime);
            currentPosY = Mathf.Lerp(oldPosY, newPosY, timer);
        }

        this.transform.position = new Vector3(currentPosX, currentPosY, 0);
    }


    /// <summary>
    ///  Returns the x value of the vector 3 position of the right most ship in the group
    /// </summary>
    float GetRightMostShip()
    {
        float x = 0;

        foreach (var item in myShipsList)
        {
            if (item != null && item.transform.parent != null)
                if (item.transform.localPosition.x > x)
                    x = item.transform.localPosition.x;
        }

        return x;
    }

    /// <summary>
    ///  Returns the x value of the vector 3 position of the left most ship in the group
    /// </summary>

    float GetLeftMostShip()
    {
        float x = 10;

        foreach (var item in myShipsList)
        {
            if (item != null && item.transform.parent != null)
                if (item.transform.localPosition.x < x)
                    x = item.transform.localPosition.x;
        }
        return x;
    }


    /// <summary>
    ///  Returns the y value of the vector 3 position of the lowest enemy in the screen
    /// </summary>
    float GetLowestShip()
    {
        float y = 0;

        foreach (var item in myShipsList)
        {
            if (item != null && item.transform.parent != null)
                if (item.transform.localPosition.y < y)
                    y = item.transform.position.y;
        }
        return y;
    }

    /// <summary>
    ///  Returns false if there are still enemies in screen
    /// </summary>
    bool ShipsLeft()
    {
        foreach (var item in myShipsList)
        {
            if (item != null)
                return false;
        }
        return true;
    }

    /// <summary>
    ///  Spawns the enemies using the values from LevelData
    /// </summary>
    void Spawn()
    {
        string[] perLine = mySpawnValue.Split('\n');
        GameObject go = null;
        float y = 0;
        float x = 0;

        for (int i = 0; i < myShipsList.Count; i++)
        {
            if (myShipsList[i] != null && myShipsList[i].transform.parent != null)
                Destroy(myShipsList[i].gameObject);
        }

        myShipsList.Clear();
        for (int i = 0; i < perLine.Length; i++)
        {
            string[] temp = perLine[i].Split(',');
            for (int j = 0; j < temp.Length; j++)
            {
                if (temp[j] != "")
                {
                    switch (int.Parse(temp[j]))
                    {
                        case 1:
                            go = Instantiate(greenEnemy, this.transform);

                            go.transform.localPosition = new Vector3(x, y, 0);

                            x++;
                            break;
                        case 2:

                            go = Instantiate(blueEnemy, this.transform);
                            go.transform.localPosition = new Vector3(x, y, 0);
                            x++;
                            break;
                        case 3:
                            go = Instantiate(redEnemy, this.transform);
                            go.transform.localPosition = new Vector3(x, y, 0);
                            x++;
                            break;
                        default:
                            break;
                    }

                    myShipsList.Add(go.GetComponent<EnemyController>());

                }
            }
            y--;
            x = 0;
        }
    }

    public float LowestShipPosition { get { return GetLowestShip(); } }
    public bool AreThereShipsLeft { get { return ShipsLeft(); } }
    public string MySpawnValues { set { mySpawnValue = value; } }
    public float Speed { set { speed = value; } }

}