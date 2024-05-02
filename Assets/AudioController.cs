using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    [SerializeField]
    AudioMixerSnapshot Exploring;
    [SerializeField]
    AudioMixerSnapshot Fighting;
    [SerializeField]
    AudioMixerSnapshot Final;
    private float transitionTime = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "BattleZone")
        {
            Fighting.TransitionTo(transitionTime);
        }

        if (collision.tag == "ExploringZone")
        {
            Exploring.TransitionTo(transitionTime);
        }

        if (collision.tag == "FinalZone")
        {
            Final.TransitionTo(transitionTime);
        }
    }
}
