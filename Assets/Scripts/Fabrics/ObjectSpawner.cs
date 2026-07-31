using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpawnData
{
    public GameObject prefab;
    public int count;
}

public class ObjectSpawner : MonoBehaviour
{
    [Header("Настройки спавна при запуске")]
    [Tooltip("Автоматически генерировать объекты при старте игры (Play mode)")]
    public bool spawnOnStart = true;

    [Header("Настройки объектов")]
    [Tooltip("Массив префабов, которые будут случайно создаваться")]
    [SerializeField] private SpawnData[] prefabsToSpawn;
    
    [SerializeField] private float sizeX = 50f;
    [SerializeField] private float sizeZ = 50f;

    [Header("Высота и Поверхность")]
    [Tooltip("Высота, откуда пускаем луч вниз для поиска земли")]
    public float raycastRayHeight = 100f;

    [Tooltip("Слой земли (Terrain/Ground)")]
    public LayerMask groundLayer;

    [Header("Случайный поворот")]
    [Tooltip("Поворачивать объекты случайно по оси Y (0-360 deg)")]
    public bool randomRotation = true;

    private void Start()
    {
        // Если галочка стоит — спавним объекты автоматически при запуске игры
        if (spawnOnStart)
        {
            SpawnObjects();
        }
    }

    [ContextMenu("Сгенерировать объекты")]
    public void SpawnObjects()
    {
        ClearSpawnedObjects();

        if (prefabsToSpawn == null || prefabsToSpawn.Length == 0)
        {
            Debug.LogWarning("ObjectSpawner: Не добавлены префабы для спавна!");
            return;
        }

        Vector3 center = transform.position;

        for (int i = 0; i < prefabsToSpawn.Length; i++)
        {

            for (int j = 0; j < prefabsToSpawn[i].count; j++)
            {
                // 1. Выбираем случайную точку в границах поля
                float randomX = Random.Range(center.x - sizeX / 2f, center.x + sizeX / 2f);
                float randomZ = Random.Range(center.z - sizeZ / 2f, center.z + sizeZ / 2f);

                Vector3 rayOrigin = new Vector3(randomX, center.y + raycastRayHeight, randomZ);

                // 2. Пускаем луч вниз, чтобы найти поверхность Terrain
                if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, raycastRayHeight * 2f, groundLayer))
                {
                    GameObject prefab = prefabsToSpawn[i].prefab;

                    // Настройка поворота
                    Quaternion rotation = prefab.transform.rotation;
                    if (randomRotation)
                    {
                        rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                    }

                    // Спавним объект с его РОДНЫМ масштабом (без изменений scale)
                    GameObject obj = Instantiate(prefab, hit.point, rotation, transform);
                    obj.transform.localScale =
                        new Vector3(obj.transform.localScale.x, 0.2f, obj.transform.localScale.z);
                }
            }
        }
    }



    [ContextMenu("Очистить сгенерированное")]
    public void ClearSpawnedObjects()
    {
        // Очистка ранее созданных объектов
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            if (Application.isPlaying)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            else
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
    }
}