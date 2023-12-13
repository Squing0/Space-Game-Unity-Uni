using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField]
    private Camera camera;  // CHANGE NAME

    private void Update()
    {
        transform.rotation = camera.transform.rotation;
    }
}
