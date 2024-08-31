using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public void PlayAudio()
    {
        AudioManager.Instance.PlaySound("Button");
    }
}
