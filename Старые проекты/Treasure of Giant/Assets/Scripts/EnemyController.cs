using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _walkPointRange;
    [SerializeField] private float _sightRange;
    [SerializeField] private LayerMask _whatIsPlayer;
    [SerializeField] private Transform _raycastPoint;
    private Vector3 _walkPoint;

    private bool _walkPointSet = false;

    void Update()
    {
        if(isPlayerInSightRange())
        {
            ChasePlayer();
        }
        else if(!isPlayerInSightRange())
        {
            Patroling();
        }
    }

    private bool isPlayerInSightRange()
    {
        Vector3 offset = new Vector3(0, 0.5f, 0);
        Debug.DrawLine(transform.position + offset, _playerTransform.position + offset, Color.blue);
        bool playerInSightRange = Physics.CheckSphere(transform.position, _sightRange, _whatIsPlayer) && CanChasePlayer();
        return playerInSightRange;
    }

    private bool CanChasePlayer()
    {
        RaycastHit hit;
        Vector3 direction = _playerTransform.position - transform.position;
        if(Physics.Raycast(_raycastPoint.position, direction, out hit))
        {
            if(hit.collider.tag == "Obstacle")
                return false;
        }
        return true;
    }

    private void ChasePlayer()
    {
        _agent.SetDestination(_playerTransform.position);
    }

    private void Patroling()
    {
        if(!_walkPointSet) SearchWalkPoint();
        else if(_walkPointSet) _agent.SetDestination(_walkPoint);

        Vector3 distanceToWalkPoint = transform.position - _walkPoint;

        if(distanceToWalkPoint.magnitude < 1f)
            _walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-_walkPointRange, _walkPointRange);
        float randomX = Random.Range(-_walkPointRange, _walkPointRange);

        _walkPoint = new Vector3(transform.position.x + randomX, 0, transform.position.z + randomZ);

        Debug.DrawLine(_walkPoint, _walkPoint + Vector3.up, Color.red, 3f);
        Debug.DrawLine(_walkPoint, -transform.up, Color.yellow, 3f);
        if(Physics.Raycast(_walkPoint + Vector3.up, -transform.up))
        {
            _walkPointSet = true;  
            Debug.DrawLine(_walkPoint, _walkPoint + Vector3.up, Color.green, 3f);
        }
    }
        void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _sightRange);
    }
}
