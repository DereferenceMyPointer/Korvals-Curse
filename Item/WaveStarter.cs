using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveStarter : Item
{
    public SoundtrackManager.Events wave;

    private void Awake()
    {
        owner.OnKillSelf += OnDeath;
    }

    public override void Equip(Character target)
    {
        Destroy(this);
    }

    public void OnDeath(object sender, EventArgs e)
    {
        ProgressionManager.Instance.StartWave(wave);
    }

}
