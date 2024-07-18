using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WiSdom
{
    public class CatManager : MonoBehaviour
    {
        [SerializeField] private Animator m_Animator;
        private int isMoving = Animator.StringToHash("isMoving");
        private int lickPaw = Animator.StringToHash("lickPaw");
        private int loaf = Animator.StringToHash("loaf");
        private int sit = Animator.StringToHash("sit");
        private int stretch = Animator.StringToHash("stretch");
        private bool isMovingBool = false;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if(isMovingBool)
                {
                    m_Animator.SetBool(isMoving, false);
                    isMovingBool = false;
                }
                else
                {
                    m_Animator.SetBool(isMoving, true);
                    isMovingBool = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                m_Animator.SetTrigger(lickPaw);
                StopMove();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                m_Animator.SetTrigger(loaf);
                StopMove();
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                m_Animator.SetTrigger(sit);
                StopMove();
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                m_Animator.SetTrigger(stretch);
                StopMove();
            }
        }

        private void StopMove()
        {
            if (isMovingBool)
            {
                m_Animator.SetBool(isMoving, false);
                isMovingBool = false;
            }
        }
    }
}
