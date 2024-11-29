using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [SerializeField]
    private string sound_id = "car_horn";

    public string getTriggerSoundID() { return sound_id; }
}
