using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GardenManager : MonoBehaviour
{
    public static GardenManager Instance;

    private List<Garden> _gardens;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        // Start is called before the first frame update
    }

    private void Start()
    {
        _gardens = GameObject.FindObjectsOfType<Garden>().ToList();
    }


    public Garden FindNearestGardenFromPosition(Transform sourcePosition)
    {
        var aliveGardens = _gardens.Where(g => g.isAlive()).ToList();
        if (aliveGardens.Count == 0)
        {
            return null;
        }

        return aliveGardens.OrderBy(g => Vector3.Distance(g.transform.position, sourcePosition.position)).First();
    }
}