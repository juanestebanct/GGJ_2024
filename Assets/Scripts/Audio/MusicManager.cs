using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.SetRTPCValue("Music_Parameter", 0f);
        SpawnEnemy.Instance.StarFight.AddListener(SetCombatMode);
        SpawnEnemy.Instance.EndFight.AddListener(SetChillMode);
    }

    public void SetCombatMode()
    {
        AkSoundEngine.SetRTPCValue("Music_Parameter", 1f);
    }

    public void SetChillMode()
    {
        AkSoundEngine.SetRTPCValue("Music_Parameter", 0f);
    }
}
