using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    //[SerializeField]
    //private Camera camera;

    //[SerializeField]
    //private Transform target;

    //[SerializeField]
    //private Vector3 offset;

    //private void Update()
    //{
    //    slider.value = 0;
    //}

    private void Start()
    {
        slider.value = 1;
    }

    private void Update()
    {
        //transform.rotation = camera.transform.rotation;
        //transform.position = target.position + offset;  //Can work without these
    }

    public void updateHealth(float health, float maxHealth)  // youtube tut
    {
        slider.value = health/maxHealth;
    }
}
