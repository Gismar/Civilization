using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CameraControl : MonoBehaviour, ITime {

    private MusicControl _musicController;

    private void Start()
    {
        _musicController = GetComponent<MusicControl>();
    }

    public void PerTick() { }
    public void PerDay()
    {
        _musicController.ChangeToNightMusic();
    }
    public void PerNight()
    {
        _musicController.ChangeToDayMusic();
    }
    public void PerCycle() { }
}
