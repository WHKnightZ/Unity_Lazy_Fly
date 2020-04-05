using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentCollisionSetup : MonoBehaviour
{

    void Start()
    {
        ParticleSystem ps = transform.parent.gameObject.GetComponent<ParticleSystem>();
        float speedMax = ps.main.startSpeed.constantMax;
        float lifeTime = ps.main.startLifetimeMultiplier;
        float height = speedMax * lifeTime;
        Transform ventTransform = transform.parent.parent;
        float x = ventTransform.position.x;
        float y = ventTransform.position.y;
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.size = new Vector2(0.7f, height);
        collider.transform.position = new Vector3(x, y + height / 2, 0f);
        //Debug.DrawLine(new Vector2(x, y), new Vector2(x, y + height), Color.green, 20f, false);
    }

}
