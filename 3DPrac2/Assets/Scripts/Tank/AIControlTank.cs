using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIControlTank : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private TankMovement movement;
    [SerializeField] private TurretController turret;
    [SerializeField] private TankHeadlights headlights;
    [SerializeField] private TankMinePlacer minePlacer;
    [SerializeField] private NavMeshAgent navMeshAgent;

    enum State {
        Debug,
        Patrolling,
        Attacking
    }
    [Header ("State")]
    [SerializeField] private State currentState;

    [Header ("Debug")]
    [SerializeField] private Transform debugPoint;
    [SerializeField] private Vector3 destination;
    [SerializeField] private Vector3 prevDestination;

    [Header ("Patrolling")]
    [SerializeField] private float randomRangeMinX;
    [SerializeField] private float randomRangeMaxX;
    [SerializeField] private float randomRangeMinZ;
    [SerializeField] private float randomRangeMaxZ;
    [SerializeField] private bool goLeft;
    [SerializeField] private bool goRight;

    [Header ("Attacking")]
    [SerializeField] private float offsetRadius;
    [SerializeField] private float sightRadius;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private string targetTag;
    [SerializeField] private Transform currentTarget;
    [SerializeField] private float fireDistance;
    [SerializeField] private float fireCD;
    [SerializeField] private bool fireReady;

    [Header ("Anti-collision")]
    [SerializeField] private LayerMask obstacleLayers;
    [SerializeField] private Transform antiColBoxLF;
    [SerializeField] private Transform antiColBoxRF;
    [SerializeField] private Transform antiColBoxLB;
    [SerializeField] private Transform antiColBoxRB;
    [SerializeField] private bool drawBoxGizmos;

    Vector3 spawnPos;
    
    void Awake() {
        spawnPos = transform.position;
        destination = transform.position;
    }

    void Start() {
        navMeshAgent.enabled = false;
        fireReady = true;
    }

    void Update() {
        if (currentState == State.Debug && debugPoint) {
            if (ComputeNewDestination(debugPoint.position)) MoveTowardDestination();
            else StopMoving();
        }
        if (currentState == State.Patrolling) {
            if (CanSeeTarget()) {
                currentState = State.Attacking;
                print("Start attacking.");
            }
            else {
                if (!AvoidBeingBlocked() && ReachDestination()) {
                ComputeNewDestinationForPatrolling();
                }
                MoveTowardDestination();
            }
        }
        if (currentState == State.Attacking) {
            if (!TryDamageTarget()) {
                if (CanSeeTarget()) {
                    if (ComputeNewDestination(currentTarget.position)) {
                        MoveTowardDestination();
                    }
                    else {
                        print("Target is unreachable.");
                    }
                }
                else {
                    currentState = State.Patrolling;
                    print("Target ran out of sight.");
                }
            }
        }
    }

    bool CanSeeTarget() {
        if (FindTargetInSightShpere(out Collider collider)) {
            Vector3 targetDir = (collider.transform.position - transform.position).normalized;
            Vector3 startPos = transform.position + offsetRadius * targetDir;
            if (Physics.Linecast(startPos, collider.transform.position, out RaycastHit info, obstacleLayers)) {
                Debug.DrawLine(startPos, info.point, Color.yellow);
                return false;
            }
            else {
                Debug.DrawLine(startPos, collider.transform.position, Color.yellow);
                currentTarget = collider.transform;
                return true;
            }
        }
        else return false;
    }

    bool FindTargetInSightShpere(out Collider target) {
        Collider[] colliders = Physics.OverlapSphere(transform.position, sightRadius, targetLayer);
        foreach (Collider collider in colliders) {
            if (collider.tag == targetTag) {
                target = collider;
                return true;
            }
        }
        target = null;
        return false;
    }

    void OnDrawGizmos() {
        if (drawBoxGizmos) {
            Gizmos.color = Color.blue;
            Gizmos.matrix = antiColBoxLF.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
            Gizmos.matrix = antiColBoxRF.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
            Gizmos.matrix = antiColBoxLB.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
            Gizmos.matrix = antiColBoxRB.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
            Gizmos.matrix = Matrix4x4.identity;
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, offsetRadius); 
        Gizmos.DrawWireSphere(transform.position, sightRadius); 
    }

    bool TryDamageTarget() {
        StopMoving();
        turret.AimAt(currentTarget);
        Vector3 endPos = turret.firePosition + fireDistance * turret.fireDirection;
        Debug.DrawLine(turret.firePosition, endPos, Color.red);
        if (Physics.Linecast(turret.firePosition, endPos, out RaycastHit info, targetLayer)) {
            if (info.collider.tag == targetTag) {
                if (fireReady) {
                    turret.Fire(false);
                    fireReady = false;
                    Invoke(nameof(GetReadyToFire), fireCD);
                }
                return true;
            }
            else return false;
        }
        return false;
    }

    bool AvoidBeingBlocked() {  
        if (movement.vertiInput > 0) {
            //bool leftBlocked  = Physics.Raycast(antiColPointLF.position, transform.forward, 1.5f, obstacleLayers);
            //bool rightBlocked = Physics.Raycast(antiColPointRF.position, transform.forward, 1.5f, obstacleLayers);
            bool  leftBlocked = Physics.BoxCast(antiColBoxLF.position, antiColBoxLF.lossyScale / 2f, transform.forward, antiColBoxLF.rotation, 1f, obstacleLayers);
            bool rightBlocked = Physics.BoxCast(antiColBoxRF.position, antiColBoxRF.lossyScale / 2f, transform.forward, antiColBoxRF.rotation, 1f, obstacleLayers);
            if (leftBlocked || rightBlocked) {
                print("Blocking detected.");
                destination = prevDestination;
                if (leftBlocked && !rightBlocked) goRight = true;
                if (!leftBlocked && rightBlocked) goLeft = true;
                return true;
            }
        }
        if (movement.vertiInput < 0 && (
            Physics.BoxCast(antiColBoxLB.position, antiColBoxLB.lossyScale / 2f, -transform.forward, antiColBoxLB.rotation, 1f, obstacleLayers) ||
            Physics.BoxCast(antiColBoxRB.position, antiColBoxRB.lossyScale / 2f, -transform.forward, antiColBoxRB.rotation, 1f, obstacleLayers)
        )) {
            print("Blocking detected.");
            destination = prevDestination;
            return true;
        }
        return false;
    }

    bool ReachDestination() {
        return Vector3.Distance(transform.position, destination) <= navMeshAgent.stoppingDistance;
    }

    void ComputeNewDestinationForPatrolling() {
        for (int i = 0; i < 8; i++) {
            Vector3 targetPos = transform.position;
            if (goRight) {
                targetPos += transform.right * randomRangeMaxX;
                goRight = false;
            }
            else if (goLeft) {
                targetPos -= transform.right * randomRangeMaxX;
                goLeft = false;
            }
            else targetPos += transform.right * Random.Range(randomRangeMinX, randomRangeMaxX);
            targetPos += transform.forward * Random.Range(randomRangeMinZ, randomRangeMaxZ);
            if (ComputeNewDestination(targetPos)) return;
        }
        ComputeNewDestination(spawnPos);
        print("Failed to find next patrolling point. Back to spawn pos.");
    }

    void ComputeNewDestinationForChasing() {
    }

    bool ComputeNewDestination(Vector3 targetPos) {
        if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit, 64f, 1)) {
            targetPos = hit.position;
        }
        else return false;

        navMeshAgent.enabled = true;
        NavMeshPath path = new NavMeshPath();
        bool result = false;
        if (navMeshAgent.CalculatePath(targetPos, path) && path.corners.Length > 1) {
            prevDestination = destination;
            destination = path.corners[1];
            result = true;
        }
        navMeshAgent.enabled = false;
        return result;
    }

    void MoveTowardDestination() {
        Vector3 toDestination = (destination - transform.position).normalized;

        float dotForward = Vector3.Dot(toDestination, transform.forward);
        if (dotForward > 0.1) movement.vertiInput = 1f;
        else if (dotForward < -0.1) movement.vertiInput = -1f;
        else movement.vertiInput = 0;

        float dotRight = Vector3.Dot(toDestination, transform.right);
        if (dotRight > 0.1) movement.horizInput = 1f;
        else if (dotRight < -0.1) movement.horizInput = -1f;
        else movement.horizInput = 0;
    }

    void StopMoving() {
        if (movement.velocity.magnitude > 0.1) {
            if (Vector3.Dot(transform.forward, movement.velocity) > 0) {
                movement.vertiInput = -1f;
            }
            else {
                movement.vertiInput = 1f;
            }
        }
        movement.horizInput = 0;
    }

    void GetReadyToFire() {
        fireReady = true;
    }
}
