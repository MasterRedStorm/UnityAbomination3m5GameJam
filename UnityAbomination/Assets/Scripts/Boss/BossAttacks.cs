using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    private void Start()
    {
    }

    public GameObject Flow;
    
    public List<FlowScript> BottomFlows = new List<FlowScript>();
    public List<FlowScript> TopFlows = new List<FlowScript>();
    
    public void FlowTop()
    {
        EdgeFlow(false);
    }
    
    public void FlowBottom()
    {
        EdgeFlow(true);
    }

    private void EdgeFlow(bool bottom)
    {
        var screenRect = GameManager.GetScreenRect();

        var currentPosition = screenRect.xMin;

        while (currentPosition < screenRect.xMax)
        {
            var flow = GameManager.Instantiate(Flow, Vector3.zero, Quaternion.identity);
            var flowScript = flow.GetComponent<FlowScript>();
            var flowCollider = flow.GetComponent<CircleCollider2D>();
            var flowTransform = flow.GetComponent<Transform>();

            var radius = flowCollider.radius;

            flowScript.IterationCounter = 1;
            flowScript.Strength = Random.Range(1.0f, 1.2f);
            flowScript.Direction = bottom ? 0.0f : 180f;
            flowScript.Scale = Random.Range(3.25f, 4.0f);

            var list = bottom ? BottomFlows : TopFlows;
            list.Add(flowScript);
            flowScript.FlowDestroyedEvent += () =>
            {
                list.Remove(flowScript);
                if (list.Count == 0 && AttackFinishedEvent != null)
                {
                    AttackFinishedEvent(
                        bottom
                            ? AttackType.FlowBottom
                            : AttackType.FlowTop
                    );
                } 
            };

        
            flowTransform.position = new Vector3(
                currentPosition + radius,
                bottom
                    ? screenRect.yMin + radius * 3.75f
                    : screenRect.yMax - radius * 3.75f,
                flowTransform.position.z
            );

            currentPosition += radius * 1.8f;
        }
    }

    public void SuperFlow()
    {
        var screenRect = GameManager.GetScreenRect();

        var a = screenRect.center + new Vector2(
            Random.Range(-0.2f * screenRect.width, 0.2f * screenRect.width),
            Random.Range(-0.2f * screenRect.height, 0.2f * screenRect.height)
        );

        var flow = GameManager.Instantiate(Flow, a, Quaternion.identity);
        var flowScript = flow.GetComponent<FlowScript>();
        flowScript.Scale = 8.0f;
        flowScript.Strength = 1.0f;
        flowScript.IterationCounter = 2;
        flowScript.FlowDestroyedEvent += () =>
        {
            if (AttackFinishedEvent != null)
            {
                AttackFinishedEvent(AttackType.SuperFlow);
            }
        };
    }
    
    public delegate void AttackFinishedDelegate(AttackType attackType);

    public event AttackFinishedDelegate AttackFinishedEvent;

    public enum AttackType
    {
        FlowTop,
        FlowBottom,
        SuperFlow
    }
}
