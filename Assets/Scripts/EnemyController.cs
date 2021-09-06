using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    green,
    blue,
    red
}

public class EnemyController : MonoBehaviour
{
    public EnemyType myType;

    int myMaxHitPoints = 1;
    int myPointsValue = 1;

    SpriteRenderer mySpriteRenderer;
    Animator myAnimator;
    Collider2D myCollider;

    readonly float myFeedBackDuration = 0.3f;

    Vector3 originalPosition;

    AudioSource myAudioSource;

    public void Start()
    {
        Init();
    }
    /// <summary>
    /// sets values and gets components
    /// </summary>
    void Init()
    {
        switch (myType)
        {
            case EnemyType.green:
                myMaxHitPoints = 1;
                myPointsValue = 1;
                break;
            case EnemyType.blue:
                myMaxHitPoints = 2;
                myPointsValue = 2;
                break;
            case EnemyType.red:
                myMaxHitPoints = 3;
                myPointsValue = 3;
                break;
            default:
                break;
        }

        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();
        myAudioSource = GetComponent<AudioSource>();
    }
    /// <summary>
    /// On hit from bullet, checks if it should reduce health or go to death function
    /// </summary>
    public void Damage(int value)
    {
        myMaxHitPoints -= value;

        if (myMaxHitPoints <= 0)
        {
            StartCoroutine(Die());
        }
        else
        {
            StopCoroutine(DamageFeedback());

            StartCoroutine(DamageFeedback());
        }
    }
    /// <summary>
    /// Removes object from parent, starts explode animation, and updates score points
    /// </summary>

    public IEnumerator Die()
    {
        Vector3 pos = this.transform.position;

        StopCoroutine(DamageFeedback());
        myCollider.enabled = false;
        yield return new WaitForEndOfFrame();
        this.transform.parent = null;
        mySpriteRenderer.color = Color.white;
        myAudioSource.PlayOneShot(GameManager.instance.EnemyHandler.myDeathClip);
        myAnimator.SetTrigger("Explode");

        GameManager.instance.UpdateScore(myPointsValue);
    }
    /// <summary>
    /// Feedback on hit by bullet
    /// </summary>
    IEnumerator DamageFeedback()
    {
        myAudioSource.PlayOneShot(GameManager.instance.EnemyHandler.myDamageClip);
        originalPosition =
                  this.transform.localPosition;
        float timer = 0;
        float currentValue = 0;
        while (timer < myFeedBackDuration && this.transform.parent != null)
        {
            timer += Time.deltaTime;

            if (timer < myFeedBackDuration * 0.25f)
            {
                currentValue = timer / (myFeedBackDuration * 0.25f);
                this.transform.localPosition = Vector3.Lerp(originalPosition, originalPosition + new Vector3(0, 0.25f, 0), currentValue);
                mySpriteRenderer.color = Color.Lerp(Color.white, Color.red, currentValue);
            }
            else
            {
                currentValue = ((4f / 3f) * (timer / myFeedBackDuration)) - (1f / 3f);
                this.transform.localPosition = Vector3.Lerp(originalPosition + new Vector3(0, 0.25f, 0), originalPosition, currentValue);
                mySpriteRenderer.color = Color.Lerp(Color.red, Color.white, currentValue);
            }

            yield return null;
        }
    }

    public void SelfDestruct()
    {
        Destroy(this.gameObject);
    }

    public AudioSource MyAudioSource { get { return myAudioSource; } }
}