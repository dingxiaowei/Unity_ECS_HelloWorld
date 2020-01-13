using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing1 : MonoBehaviour
{
    public int count = 1000;
    List<Transform> entities = new List<Transform>();
    void Start()
    {
        for (int i = 0; i < count; i++)
            entities.Add(GameObject.CreatePrimitive(PrimitiveType.Cube).transform);
    }

    void Update()
    {
        for (int i = 0; i < count; i++)
        {
            entities[i].Translate(Vector3.up * (UnityEngine.Random.Range(1f, 2f)) * Time.deltaTime);
            //if (entities[i].localPosition.y > 5f)
            //{
            //    moveSpeedComponent.moveSpeed = -math.abs(moveSpeedComponent.moveSpeed);
            //}
            //if (entities[i].localPosition.y < -5f)
            //{
            //    moveSpeedComponent.moveSpeed += math.abs(moveSpeedComponent.moveSpeed);
            //}
        }
    }
}
