using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class testagent : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent Agent;

    private bool MoveAcrossNavMeshesStarted;

    private NavMeshPath path;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            path = new NavMeshPath();
            bool valid = NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
            Debug.Log($"Is Valid: {valid}");
            Debug.Log(path.status);
            Agent.SetPath(path);
        }

        if (Agent.isOnOffMeshLink && !MoveAcrossNavMeshesStarted)
        {
            StartCoroutine(MoveAcrossNavMeshLink());
            MoveAcrossNavMeshesStarted = true;
        }
    }

    IEnumerator MoveAcrossNavMeshLink()
    {
            OffMeshLinkData data = Agent.currentOffMeshLinkData;
            Agent.updateRotation = false;

            Vector3 startPos = Agent.transform.position;
            Vector3 endPos = data.endPos + Vector3.up * Agent.baseOffset;
            float duration = (endPos - startPos).magnitude / Agent.speed;
            float t = 0.0f;
            float tStep = 1.0f / duration;
            while (t < 1.0f)
            {
                transform.position = Vector3.Lerp(startPos, endPos, t);
                t += tStep * Time.deltaTime;
                yield return null;
            }

            transform.position = endPos;
            Agent.updateRotation = true;
            Agent.CompleteOffMeshLink();
            MoveAcrossNavMeshesStarted = false;   
    }
}