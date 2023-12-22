using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace Lib
{
    /// <summary>
    /// A spell charge animation that is displayed on the ball.
    /// </summary>
    public class BallIndicatorSpellCharge : MonoBehaviour, ISpellChargeable
    {
        private GameObject ballIndicatorInstance;
        private bool isPlaying = false;
        private float duration = 0f;
        private float spinTimer = 0f;

        private void Start()
        {
            spinTimer = gameObject.GetComponent<WaitDurationSpellCharge>().duration;
        }

        public void ChargeSpell()
        {
            isPlaying = true;
            ballIndicatorInstance = Instantiate(FindObjectOfType<GameCanvas>().BallIndicatorObject, FindObjectOfType<GameCanvas>().transform);
            ballIndicatorInstance.layer = LayerMask.NameToLayer("UI");

            Update();
        }

        private void Update()
        {
            var ball = GameObject.FindGameObjectWithTag("GameBall");
            if (ballIndicatorInstance != null) {
                ballIndicatorInstance.transform.position =
                    Camera.main.WorldToScreenPoint(ball.transform.position);
                
                ballIndicatorInstance.transform.eulerAngles =
                    new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, (duration / Mathf.Pow(spinTimer, 2)) * 15);
            }

            if (Spellcasting.SpellIsDoneCharging(gameObject)) {
                Destroy(ballIndicatorInstance);
            }

        }

        public bool IsDone()
        {
            return true;
        }
    }
}