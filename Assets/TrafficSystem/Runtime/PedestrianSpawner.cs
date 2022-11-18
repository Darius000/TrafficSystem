using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianSpawner : MonoBehaviour
{
    public List<GameObject> m_PedestrianPrefabs;

    public int m_NumberToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        int count = 0;
        while(count < m_NumberToSpawn)
        {
            GameObject prefab = m_PedestrianPrefabs[Random.Range(0, m_PedestrianPrefabs.Count - 1)];
            GameObject obj = Instantiate(prefab);
            Transform child = transform.GetChild(Random.Range(0, transform.childCount - 1));
            obj.GetComponent<WayPointNavigator>().m_CurrentWaypoint = child.GetComponent<WayPoint>();
            obj.transform.position= child.position;

            yield return new WaitForEndOfFrame();

            count++;
        }
    }
}
