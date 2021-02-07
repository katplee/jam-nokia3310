using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private GameObject playerGO = null;
    private Animator objectAnimator = null;

    private bool isAnimating = false;
    private float animationTime = 7f;
    private float runningTime;
    private float animationEndTime;

    private void Start()
    {
        PlayerMovement.ReadyToAnimate += Animate;
        objectAnimator = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        PlayerMovement.ReadyToAnimate -= Animate;
    }

    private void Update()
    {
        if (!isAnimating) { return; }

        runningTime += Time.deltaTime;

        if (runningTime >= animationEndTime)
        {
            EndAnimate();
        }
    }

    private void Animate(string destinationName)
    {
        if (gameObject.name != destinationName) { return; }

        //Hide the player's game object.
        playerGO.SetActive(false);

        //Do the animation.
        objectAnimator.SetBool("Animate", true);

        SetTimers();

    }

    private void EndAnimate()
    {
        //Remove the animation.
        objectAnimator.SetBool("Animate", false);

        if (!objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Default")) { return; }

        //Show the player's game object.
        playerGO.SetActive(true);

        ClearTimers();
    }

    private void SetTimers()
    {
        isAnimating = true;
        runningTime = Time.time;
        animationEndTime = runningTime + animationTime;
    }

    private void ClearTimers()
    {
        isAnimating = false;
        runningTime = 0f;
        animationEndTime = 0f;
    }
}
