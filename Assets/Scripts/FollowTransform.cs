using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    [SerializeField] Transform toFollow;
    // Start is called before the first frame update
    void Start()
    {
        if (!toFollow) toFollow = FindFirstObjectByType<Player>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = toFollow.position;
    }
}
