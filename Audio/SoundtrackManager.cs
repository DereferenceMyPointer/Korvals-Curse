using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Basic event-based soundtrack management tool
 * Plays soundtracks based on wave number
 * Fades in new layers of track when directed
 */
public class SoundtrackManager : MonoBehaviour
{
    public static SoundtrackManager Instance;
    public List<KeyPair> soundtracks;
    public float fadeOutTime;
    public float downTime;
    private Soundtrack currentTrack;
    private int currentLayer = 0;

    public enum Events
    {
        START,
        WAVE_1,
        WAVE_2,
        WAVE_3,
        WAVE_4
    }

    [Serializable]
    public struct KeyPair
    {
        public Soundtrack track;
        public Events stEvent;
    }

    // Initiate singleton
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(this);
        }
    }

    // Start a track based on an event
    // Takes time equal to fadeOutTime + downTime
    // Start any NextLayers on the next frame or later
    public void StartTrack(Events stEvent)
    {
        foreach (KeyPair s in soundtracks)
            if (s.stEvent == stEvent)
                StartCoroutine(Begin(s.track));
    }

    // Fades in a new layer
    public void NextLayer()
    {
        if (currentTrack && currentLayer < currentTrack.layers.Count - 1)
        {
            currentLayer++;
            StartCoroutine(AddLayer());
        }
    }

    public void End()
    {
        StartCoroutine(Stop());
    }

    public IEnumerator AddLayer()
    {
        SoundtrackLayer layer = currentTrack.layers[currentLayer];
        float t = layer.fadeTime;
        while(t > 0)
        {
            t -= Time.deltaTime;
            layer.clip.volume += Time.deltaTime * layer.maxVolume / layer.fadeTime;
            yield return new WaitForEndOfFrame();
        }
        layer.clip.volume = layer.maxVolume;
    }

    public IEnumerator Begin(Soundtrack nextTrack)
    {
        if (currentTrack)
        {
            float t = fadeOutTime;
            while (t > 0)
            {
                yield return new WaitForEndOfFrame();
                foreach (SoundtrackLayer l in currentTrack.layers)
                    l.clip.volume -= Time.deltaTime * l.maxVolume / fadeOutTime;
                t -= Time.deltaTime;
            }
            foreach (SoundtrackLayer l in currentTrack.layers)
                l.clip.Stop();
            yield return new WaitForSeconds(downTime);
        }
        currentLayer = 0;
        currentTrack = nextTrack;
        foreach(SoundtrackLayer s in currentTrack.layers)
        {
            s.clip.volume = 0;
            s.clip.Play();
        }
        currentTrack.layers[0].clip.volume = currentTrack.layers[0].maxVolume;
    }

    private IEnumerator Stop()
    {
        if (currentTrack)
        {
            float t = fadeOutTime;
            while (t > 0)
            {
                yield return new WaitForEndOfFrame();
                foreach (SoundtrackLayer l in currentTrack.layers)
                    l.clip.volume -= Time.deltaTime * l.maxVolume / fadeOutTime;
                t -= Time.deltaTime;
            }
            foreach (SoundtrackLayer l in currentTrack.layers)
                l.clip.Stop();
            yield return new WaitForSeconds(downTime);
        }
        currentLayer = 0;
    }

}
