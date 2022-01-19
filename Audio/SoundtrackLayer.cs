using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Single layer of a soundtrack piece
 */
[RequireComponent(typeof(AudioSource))]
public class SoundtrackLayer : MonoBehaviour
{
    public float maxVolume;
    public float fadeTime;
    public AudioSource clip;
}
