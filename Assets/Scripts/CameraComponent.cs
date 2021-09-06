using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraComponent : MonoBehaviour
{
    public static CameraComponent instance;

    Camera myCamera;

     float height;
    float width;

    Vector3 originalPos;
    public SpriteRenderer targetBackground;
    float originalSize;

    private void Awake()
    {
        instance = this;

        Init();
    }
    
    // Update is called once per frame
    void Update()
    {
        GetBorder();
        ScaleViewport();
    }

    void Init()
    {
        ScaleViewport();
        myCamera = Camera.main;
        GetBorder();
        originalPos = this.transform.position;
    }
    /// <summary>
    /// It gets the borders in the screen in world unitys 
    /// </summary>
    public void GetBorder()
    {
        width = 1 / (myCamera.WorldToViewportPoint(new Vector3(1, 1, 0)).x-0.5f);
        height = 1 / (myCamera.WorldToViewportPoint(new Vector3(1, 1, 0)).y-0.5f);
    }


    /// <summary>
    /// adjust camera orthographic size depening of a target (this case the sky render)
    /// it has a call on editor so it can be tested
    /// </summary>
    private void ScaleViewport()
    {
        var bounds = targetBackground.bounds.extents;
        var height = bounds.x / Camera.main.aspect;
        if (height > bounds.y)
            height = bounds.y;
        Camera.main.orthographicSize = height;
        originalSize = Camera.main.orthographicSize;

    }

    public float CameraHeight { get { return height; } }
    public float CameraWidth { get { return width; } }
}
