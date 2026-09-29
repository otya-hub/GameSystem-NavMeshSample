using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyView : MonoBehaviour
{
    [SerializeField] private Transform _target;                  //  プレイヤー
    [SerializeField] private List<Transform> _rootLists = new(); //  ルートリスト
    private int _rootIndex = 0;                                  //  現在のルートインデックス
    private NavMeshAgent _agent;                                 //  NavMeshAgentコンポーネント

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //  NavMeshAgentコンポーネントを取得
        _agent = GetComponent<NavMeshAgent>();
        //  ルートリストが存在する場合、最初のルートに向かって移動を開始
        if (_rootLists.Count > 0 && _agent != null)
        {
            _rootIndex++;
            //  NavMeshAgentの目的地を設定
            _agent.SetDestination(_rootLists[_rootIndex].position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_target == null) return;
        if (!IsTargetVisible())
        {
            if (_rootLists.Count > 0 && _agent != null && IsStopped())
            {

                  _rootIndex++;
                  _rootIndex %= _rootLists.Count;
                  _agent.SetDestination(_rootLists[_rootIndex].position);
                /*if (!IsStopped())
                {
                    _rootIndex++;
                    _rootIndex %= _rootLists.Count;
                }
                else if (IsStopped())
                {
                    _agent.SetDestination(_rootLists[_rootIndex].position);
                }*/
            }
            return;
        }
        _agent.SetDestination(_target.position);
    }

    private bool IsStopped()
    {
        if (_agent == null) return true;
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            return (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f);
        }
        return false;
    }

    private bool IsTargetVisible()
    {
        if (_target == null) return false;
        Vector3 directionToTarget = _target.position - transform.position;
        float distanceToTarget = directionToTarget.magnitude;
        // Check if the target is within the agent's stopping distance
        if (distanceToTarget > _agent.stoppingDistance)
        {
            // Perform a raycast to check if there are any obstacles between the agent and the target
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToTarget.normalized, out hit, distanceToTarget))
            {
                // If the raycast hits something other than the target, the target is not visible
                if (hit.transform != _target)
                {
                    return false;
                }
            }
        }
        return true;
       
    }
}
