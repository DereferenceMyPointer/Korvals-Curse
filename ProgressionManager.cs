using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/*
 * Manages progression
 * Uses a wave-by-wave cycle that increases in scale over time
 * Enemies are spawned pseudo-randomly based on level
 */
public class ProgressionManager : MonoBehaviour
{
    public static ProgressionManager Instance;
    public int progressionLevel = 0;
    public float progressionTimeout = 1f;
    private int points = 0;
    public List<Transform> spawnPoints;
    private Dictionary<Enemies.EnemyTier, Transform> spawns;
    public Enemies waves;
    public SoundtrackManager.Events currentWave;
    public TextMeshProUGUI waveText;

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

    private void Start()
    {
        SoundtrackManager.Instance.StartTrack(SoundtrackManager.Events.START);
        spawns = new Dictionary<Enemies.EnemyTier, Transform>();
        StartCoroutine(Progress());
    }

    public IEnumerator Progress()
    {
        while (gameObject)
        {
            points += progressionLevel;
            yield return new WaitForSeconds(progressionTimeout);
        }
        
    }

    public void NextWave()
    {
        currentWave = GetNextWave();
        StartWave(currentWave);
    }

    private SoundtrackManager.Events GetNextWave()
    {
        if (System.Enum.IsDefined(typeof(SoundtrackManager.Events), currentWave + 1) && waves.wavePrefabs(currentWave + 1).Count != 0)
        {
            return currentWave + 1;
        }
        else
        {
            return SoundtrackManager.Events.WAVE_1;
        }
    }

    public void StartWave(SoundtrackManager.Events wave)
    {
        foreach(EnemyCharacter c in FindObjectsOfType<EnemyCharacter>())
        {
            Destroy(c.gameObject);
        }
        if (points < 2)
            points += progressionLevel;
        currentWave = wave;
        StartCoroutine(WaveAsync(wave));
    }

    private IEnumerator WaveAsync(SoundtrackManager.Events wave)
    {
        progressionLevel++;
        waveText.text = "Wave " + progressionLevel;
        SoundtrackManager.Instance.StartTrack(wave);
        yield return new WaitForSeconds(SoundtrackManager.Instance.downTime + SoundtrackManager.Instance.fadeOutTime);
        RollSubwaves(Random.Range(2, 4));
    }

    private void RollSubwaves(int subWaves)
    {
        SoundtrackManager.Instance.NextLayer();
        int maxSpawns = System.Math.Max(progressionLevel * 2, points / 4);
        float spawns = 0;
        for (int i = 0; i < maxSpawns; i++)
        {
            if (points == 0 || waves.minCost(currentWave) > points)
            {
                if (spawns == 0)
                {
                    NextWave();
                    return;
                }
                else
                    return;
            }
            Enemies.EnemyCost toSpawn = GetEnemyToSpawn(currentWave);
            spawns++;
            points -= toSpawn.cost;
            Instantiate(toSpawn.enemy, spawnPoints[Random.Range(0, spawnPoints.Count)].position, Quaternion.identity)
                .GetComponent<Character>().OnKillSelf += delegate (object sender, System.EventArgs e)
                {
                    spawns--;
                    if (spawns <= 0)
                    {
                        subWaves--;
                        if (subWaves >= 0)
                        {
                            RollSubwaves(subWaves);
                        }
                        else
                        {
                            NextWave();
                        }
                    }
                };
        }
    }

    private Enemies.EnemyCost GetEnemyToSpawn(SoundtrackManager.Events wave)
    {
        Enemies.EnemyTier tierToSpawn = (Enemies.EnemyTier)Random.Range(0, 5);
        Enemies.EnemyCost generated = waves.FindEnemyOfTier(tierToSpawn, wave);
        if (generated.cost == 0 || generated.cost > points)
            return GetEnemyToSpawn(wave);
        return generated;
    }

}
