using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Instances hold wave settings for a run
 * Contains basic enemy info
 */
[CreateAssetMenu(fileName = "EnemySettings", menuName = "Enemy Setting")]
public class Enemies : ScriptableObject
{
    public List<GameObject> wave1;
    public List<GameObject> wave2;
    public List<GameObject> wave3;
    public List<GameObject> wave4;
    public List<EnemyCost> enemyCosts;
    public EnemyCost zero;

    public List<Item> allItems;
    
    [Serializable]
    public struct EnemyCost
    {
        public GameObject enemy;
        public EnemyTier tier;
        public SoundtrackManager.Events wave;
        public int cost;
        public int selectionWeight;
    }

    public enum EnemyTier
    {
        Peasant,
        Commoner,
        Lord,
        Saint,
        Deity
    }

    public int tierToCost(EnemyTier tier)
    {
        foreach(EnemyCost e in enemyCosts)
        {
            if (e.tier == tier)
                return e.cost;
        }
        return 0;
    }

    public EnemyCost FindEnemyOfTier(EnemyTier tier, SoundtrackManager.Events wave)
    {
        List<EnemyCost> choices = new List<EnemyCost>();
        foreach (EnemyCost e in enemyCosts)
        {
            if (e.tier == tier && e.wave == wave)
            {
                for(int i = 0; i < e.selectionWeight; i++)
                {
                    choices.Add(e);
                }
            }
        }
        if (choices.Count == 0)
            return zero;
        else
            return choices[UnityEngine.Random.Range(0, choices.Count)];
    }
    
    public List<GameObject> wavePrefabs(SoundtrackManager.Events wave)
    {
        switch (wave)
        {
            case SoundtrackManager.Events.WAVE_1: return wave1;
            case SoundtrackManager.Events.WAVE_2: return wave2;
            case SoundtrackManager.Events.WAVE_3: return wave3;
            case SoundtrackManager.Events.WAVE_4: return wave4;
            default: return null;
        }
    }

    public int minCost(SoundtrackManager.Events wave)
    {
        int outp = 10000;
        foreach(EnemyCost c in enemyCosts)
        {
            if(c.wave == wave && c.cost < outp)
            {
                outp = c.cost;
            }
        }
        return outp;
    }

}
