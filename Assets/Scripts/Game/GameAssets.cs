using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    #region Assets
    public Sprite PipeHeadSprite;
    public Transform PipeHead;
    public Transform PipeBody;
    #endregion Assets

    public SoundAudioClip[] SoundAudioClips;

    private static GameAssets _instance;

    public static GameAssets GetInstance() => _instance;
    
    private void Awake()
    {
        _instance = this;
    }
}
