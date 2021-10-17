using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    public GameObject myShip;

    public float movementSpeed;

    public float fireRate;
    float timer;

    public GameObject bullet;

    public List<Transform> myInstanceTransforms;

    int myCurretInstance;

    public AudioClip myAudioClip;
    AudioSource myAudioSource;

    private void Update()
    {
        if (GameManager.instance.MyGameState != GameState.gamePlay)
        {
            timer += Time.deltaTime;
            myShip.transform.position = Vector3.Lerp(new Vector3(0, -(CameraComponent.instance.CameraHeight / 2f) + 1f, 0), new Vector3(0, -(CameraComponent.instance.CameraHeight / 2f) - 1f, 0), timer);
           
        }
        else
        {
            timer = 0;
            GameManager.instance.myShipPositionYValue = myShip.transform.position.y+0.5f;
        }
    }
    /// <summary>
    ///  start values for the avatar
    /// </summary>
    public void Init()
    {
        myAudioSource = GetComponent<AudioSource>();
        myShip.transform.position = new Vector3(0, -(CameraComponent.instance.CameraHeight / 2f) + 1f, 0);
    }
    /// <summary>
    /// Moves the ship 
    /// </summary>
    public void Move(float value)
    {
        float x = value * movementSpeed * Time.deltaTime;
        if ((CameraComponent.instance.CameraWidth / 2f) - 0.5f > Mathf.Abs(myShip.transform.position.x))
            myShip.transform.position += new Vector3(x, 0, 0);
        else
            myShip.transform.position = new Vector3((myShip.transform.position.x > 0 ? 1 : -1) * ((CameraComponent.instance.CameraWidth / 2f) - 0.6f), myShip.transform.position.y, 0);

        myShip.transform.eulerAngles = new Vector3(0, 45f * value, 0);
    }

    public void AutomaticFire(float value)
    {
        if (value > 0)
        {
            timer += Time.deltaTime;

            if (timer >= fireRate)
            {
                //foo
                CreatBullet();
                Debug.Log("pew");
                timer = 0;
            }
        }
    }
    /// <summary>
    /// Every time it gets called it creates a bullet
    /// </summary>
    public void Fire()
    {
        myAudioSource.PlayOneShot(myAudioClip);

        CreatBullet();
    }

    /// <summary>
    /// being called it alterantes between positions to intance the bullet
    /// </summary>
    public void CreatBullet()
    {
        myCurretInstance++;

        if (myInstanceTransforms.Count > 0)
        {
            if (myCurretInstance >= myInstanceTransforms.Count)
                myCurretInstance = 0;
            Instantiate(bullet, myInstanceTransforms[myCurretInstance].position, bullet.transform.rotation);
        }
        else
        {
            Instantiate(bullet, myShip.transform.position, bullet.transform.rotation);
        }
    }
}