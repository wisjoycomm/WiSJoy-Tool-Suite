using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace WiSdom
{
    [ExecuteInEditMode]
    public class TweenManager : MonoBehaviour
    {
        [SerializeField] private Transform target;

        [ContextMenu("Move")]
        public void Move()
        {
            Vector3 targetPosition = new Vector3(5, 0, 0);
            float duration = 1f;

            // Check if in Edit Mode
            if (!Application.isPlaying)
            {
                // Create a new Tween for Edit Mode
                DOTween.To(
                    () => target.position,           // Getter for the current value
                    x => target.position = x,        // Setter for updating the value
                    targetPosition,                  // Target value
                    duration                         // Duration
                );
            }
            else
            {
                // Run standard DOTween animation in Play Mode
                target.DOMove(targetPosition, duration);
            }
        }


    }
}