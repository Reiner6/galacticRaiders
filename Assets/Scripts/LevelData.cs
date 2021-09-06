using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelData", menuName = "Data/LevelData", order = 1)]
/// <summary>
/// Level data, Enemies configuration, movement speed and level music
/// </summary>
public class LevelData : ScriptableObject
{
    public string levelName;
    [TextArea]
    public string SpawnValue;

    public float speed;

    public AudioClip introClip;
    public AudioClip loopClip;
}
