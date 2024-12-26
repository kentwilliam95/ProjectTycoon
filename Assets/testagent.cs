using System;
using System.Collections;
using System.Collections.Generic;
using ProjectSims.Simulation.CoreSystem.Scripts.Interface;
using UnityEngine;
using UnityEngine.AI;

public class testagent : MonoBehaviour, IAgent
{
    public Transform target;
    public NavMeshAgent Agent;

    private bool MoveAcrossNavMeshesStarted;

    private NavMeshPath path;

    private IEnumerator MoveAcrossNavMeshLink()
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

    public void MoveTo(Vector3 target)
    {
        path = new NavMeshPath();
        bool valid = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
        Agent.SetPath(path);
    }

    public void DisableAgent()
    {
        Agent.enabled = false;
    }

    public void EnableAgent()
    {
        Agent.enabled = true;
    }
}