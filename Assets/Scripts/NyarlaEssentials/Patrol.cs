using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NyarlaEssentials
{
public class Patrol : Transformer
{
    [SerializeField] private Vector3[] path;
    [SerializeField] private float speed;
    [SerializeField] private float nextPointDelay;
    public enum LoopType
    {
        Return,
        Lap
    }
    [SerializeField] private LoopType loopType;
    private Vector3 origin;

    private void Awake()
    {
        origin = transform.position;
    }

    private void Start()
    {
        StartCoroutine(Launch());
    }

    private IEnumerator Launch()
    {
        while (true)
        {
            for (int i = 0; i < path.Length; i++){
                yield return StartCoroutine(GoToNextPoint(path[i]));
            }
            if (loopType.Equals(LoopType.Return))
            {
                for (int i = path.Length - 2; i >= 0; i--){
                    yield return StartCoroutine(GoToNextPoint(path[i]));
                }
            }
            yield return StartCoroutine(GoToNextPoint(Vector3.zero));
        }
    }

    private IEnumerator GoToNextPoint(Vector3 point)
    {
        point += origin;
        while (!transform.position.Equals(point)){
            transform.position = Vector3.MoveTowards(transform.position, point, speed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(nextPointDelay);
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < path.Length; i++)
        {
            // Draw cubes
            float cubeSize = 4;
            Gizmos.color = Color.green;
            Gizmos.DrawCube(path[i] + gameObject.transform.position, new Vector3(cubeSize, cubeSize, cubeSize));
            // Draw lines
            Vector3 origin;
            if (i == 0)
                origin = Vector3.zero;
            else
                origin = path[i - 1];
            
            Gizmos.color = Color.green;
            Gizmos.DrawLine(path[i] + gameObject.transform.position, origin + gameObject.transform.position);
        }
    }
}

}