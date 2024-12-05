using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class particle_unit
{
    public string id;
    public GameObject prefab;
    public int pool_size;
}

public class ParticlePoolManager : MonoBehaviour
{
    public static ParticlePoolManager instance { get; private set; }

    public List<particle_unit> particle_list;

    public Dictionary<string, List<GameObject>> particle_pool;

    public List<GameObject> activated_particles;

    MapManager map_manager;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    void Start()
    {
        map_manager = FindObjectOfType<MapManager>();

        particle_pool = new Dictionary<string, List<GameObject>>();
        activated_particles = new List<GameObject>();

        RegisterParticles();
    }

    void Update()
    {
        for (int i = 0; i < activated_particles.Count; i++)
        {
            GameObject particle = activated_particles[i].gameObject;

            if (!particle.GetComponent<ParticleSystem>().isPlaying)
            {
                activated_particles[i].SetActive(false);
                activated_particles.RemoveAt(i);
            }
            else
            {
                float x = particle.transform.position.x;

                Debug.Log("speed:" + map_manager.getMapSpeed());

                //x += map_manager.getMapSpeed();
                x += 5 * Time.deltaTime;

                particle.transform.position = new Vector3(
                    x,
                    particle.transform.position.y,
                    particle.transform.position.z
                );
            }
        }
    }

    public void SpawnEffect(string effectName, Vector3 position)
    {
        if (particle_pool.ContainsKey(effectName))
        {
            var pool = particle_pool[effectName];

            foreach (var particle in pool)
            {
                if (!particle.activeInHierarchy)
                {
                    particle.SetActive(true);
                    particle.transform.position = position;

                    activated_particles.Add(particle);

                    return;
                }
            }

            Debug.LogWarning("Particle Manager: No available particles: " + effectName);
        }
        else
            Debug.LogWarning("Particle Manager: No Effect: " + effectName);
    }

    // 활성화된 파티클을 비활성화하고 풀에 다시 넣기
    public IEnumerator ReturnParticleAfterDelay(GameObject particle)
    {
        ParticleSystem ps = particle.GetComponent<ParticleSystem>();

        while (ps.isPlaying)
            yield return null;

        particle.SetActive(false);
    }

    void RegisterParticles()
    {
        foreach (particle_unit item in particle_list)
        {
            List<GameObject> pool = new List<GameObject>();

            for (int i = 0; i < item.pool_size; i++)
            {
                GameObject particle = Instantiate(item.prefab);
                particle.SetActive(false);
                pool.Add(particle);
            }

            particle_pool[item.id] = pool;
        }
    }
}
