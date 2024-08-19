using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BackRoomTrigger : LightEvent
{
    public PostProcessVolume postProcessVolume;
    public Light[] lights;
    public GameObject fluorescentLight;
    public GameObject Player;
    public GameObject TrapDoor;

    [Header("EnemyChase")]
    public Transform hands;
    public GameObject[] enemys;
    public Transform[] paths;
    public float handsInterval = 0.5f;
    public float enemySpeed = 1f;
    [SerializeField] AudioClip chaseSound;

    SoundManager soundManager;

    void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    public override void TriggerLightEvent()
    {
        // turn off light noise
        soundManager.StopBackgroundSound();

        StartCoroutine(StartBackground());
        StartCoroutine(StartBackRoom());
        StartCoroutine(StartHandsOn());
        for(int i = 0; i < enemys.Length; i++)
        {
            enemys[i].SetActive(true);
            StartCoroutine(StartEnemyMove(enemys[i], paths[i]));
        }
        TrapDoor.SetActive(false);

        Player.GetComponent<Status>().staminaDecreaseRate = 0f;
    }

    IEnumerator StartBackground()
    {
        yield return new WaitForSeconds(3f);
        soundManager.PlayBackgroundSound(chaseSound);
    }

    IEnumerator StartBackRoom()
    {
        foreach (Light light in lights)
        {
            light.enabled = false;
        }
        postProcessVolume.profile.GetSetting<Bloom>().enabled.value = true;
        postProcessVolume.profile.GetSetting<AutoExposure>().enabled.value = true;
        Instantiate(fluorescentLight, new Vector3(Player.transform.position.x, Player.transform.position.y+5, Player.transform.position.z), Quaternion.identity);
        yield return new WaitForSeconds(1f);
    }

    IEnumerator StartHandsOn()
    {
        for(int i=0; i < hands.childCount; i++)
        {
            foreach (Transform hand in hands.GetChild(i))
            {
                hand.gameObject.SetActive(true);
                yield return new WaitForSeconds(handsInterval);
            }
        }
    }

    IEnumerator StartEnemyMove(GameObject enemy, Transform path)
    {
        Vector3 goal = path.GetChild(0).transform.position;
        int count = 1;
        while(Vector3.Distance(enemy.transform.position, path.GetChild(path.childCount-1).transform.position) > 1f)
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, goal, Time.deltaTime * enemySpeed);
            if(Vector3.Distance(enemy.transform.position, goal) < 1f)
            {
                if(count < path.childCount)
                {
                    goal = path.GetChild(count++).transform.position;
                }
            }
            yield return new WaitForEndOfFrame();
        }

    }
}
