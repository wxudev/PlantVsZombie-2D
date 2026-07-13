using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ZombieSpawner : MonoBehaviour
{
    [Header("僵尸预制体")]
    public GameObject zombiePrefab;
    [Header("生成位置")]
    [Tooltip("僵尸生成的X坐标（屏幕右侧外）")]
    public float spawnX = 10f;
    [Header("路的Y坐标（从上到下）")]
    [Tooltip("每条路的Y坐标，有几条路就填几个值")]
    public float[] laneYPositions = new float[] { 2.5f, 1.25f, 0f, -1.25f, -2.5f };
    [Header("生成间隔（秒）")]
    public float minSpawnInterval = 3f;
    public float maxSpawnInterval = 6f;
    [Header("初始延迟")]
    public float startDelay = 2f;

    // 新增：背景音乐AudioSource，拖自身AudioSource组件
    public AudioSource bgm4;

    private float timer;
    private float currentInterval;
    // Start is called before the first frame update
    void Start()
    {
        // 初始延迟后开始生成
        timer = -startDelay;
        SetRandomInterval();
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentInterval)
        {
            SpawnZombie();
            timer = 0f;
            SetRandomInterval();
        }
    }
    // 随机设置下一次生成间隔
    void SetRandomInterval()
    {
        currentInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
    }
    // 生成一只僵尸
    void SpawnZombie()
    {
        if (zombiePrefab == null || laneYPositions.Length == 0)
        {
            Debug.LogWarning("僵尸预制体或路的坐标未设置！");
            return;
        }
        // 随机选一条路
        int randomLane = Random.Range(0, laneYPositions.Length);
        float spawnY = laneYPositions[randomLane];
        // 生成位置
        Vector3 spawnPos = new Vector3(spawnX, spawnY, 0);
        // 生成僵尸
        Instantiate(zombiePrefab, spawnPos, Quaternion.identity);
    }
}