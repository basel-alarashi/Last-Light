using System.Collections;
using UnityEngine;
using LastLight.Systems;

namespace LastLight.Player
{
    /// <summary>
    /// Handles player interaction with gatherable resource nodes.
    /// </summary>
    public class PlayerGatherer : MonoBehaviour
    {
        [Header("Gather Settings")]
        [SerializeField] private float gatherRadius = 2f;
        [SerializeField] private LayerMask resourceLayerMask;

        private bool _isGathering = false;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E) && !_isGathering)
            {
                TryGather();
            }
        }

        private void TryGather()
        {
            IGatherable node = FindNearestNode();

            if (node == null || node.IsDepleted) return;

            StartCoroutine(GatherRoutine(node));
        }

        private IGatherable FindNearestNode()
        {
            Collider[] hits = Physics.OverlapSphere(
                transform.position,
                gatherRadius,
                resourceLayerMask
            );

            float closestDistance = float.MaxValue;
            IGatherable closest = null;

            foreach (Collider hit in hits)
            {
                IGatherable gatherable = hit.GetComponent<IGatherable>();
                if (gatherable == null || gatherable.IsDepleted) continue;

                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = gatherable;
                }
            }

            return closest;
        }

        private IEnumerator GatherRoutine(IGatherable node)
        {
            _isGathering = true;

            Debug.Log($"[Gathering] Gathering {node.Data.resourceName}...");
            yield return new WaitForSeconds(node.Data.gatherTime);

            int amount = node.Gather();
            Debug.Log($"[Gathering] Gathered {amount}x {node.Data.resourceName}");

            _isGathering = false;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, gatherRadius);
        }
#endif
    }
}