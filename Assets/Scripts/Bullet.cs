using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;
    private float gravity;
    private Vector3 startPosition;
    private Vector3 startForward;

    private bool isInitialized = false;
    private float startTime = -1;

    public void Initialize(Transform _startPosition, float _speed, float _gravity )
    {
        startPosition = _startPosition.position;
        startForward = _startPosition.forward.normalized;
        speed = _speed;
        gravity = _gravity;

        isInitialized = true;
    }

    private Vector3 FindParabolicPoint(float time)
    {
        Vector3 forwardVector = startPosition + ( startForward * speed * time );
        Vector3 gravityVec = Vector3.down * gravity * time * time;
        return forwardVector + gravityVec;
    }

    private void FixedUpdate()
    {
        if (!isInitialized) { return; }
        if (startTime < 0) startTime = Time.time;

        float currentTime = Time.time - startTime;
        float nextTime = currentTime + Time.fixedDeltaTime;

        Vector3 currentPosition = FindParabolicPoint(currentTime);
        Vector3 nextPosition = FindParabolicPoint(nextTime);
            
        RaycastHit hit;
        if(CastRayBetweenPoints(currentPosition, nextPosition, out hit))
        {
            Debug.Log(hit.transform.name);
        }    

    }

    private bool CastRayBetweenPoints(Vector3 startPosition, Vector3 endPosition, out RaycastHit raycastHit)
    {
        return Physics.Raycast(startPosition, endPosition - startPosition, out raycastHit, (endPosition - startPosition).magnitude);
    }

    private void Update() 
    {
        if(!isInitialized || startTime < 0) { return; }
        float currentTime = Time.time - startTime;
        transform.position = FindParabolicPoint(currentTime); 
    }

}
