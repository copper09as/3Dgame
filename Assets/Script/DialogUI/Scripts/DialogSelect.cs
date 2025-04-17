using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class DialogSelect : MonoBehaviour
{
    public SequenceEventExecutor executor;
    public PlayerController playerController;

    private void Start()
    {
        executor.Init(OnFinishedEvent);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            playerController.SetInputEnabled(false);
            executor.Execute();
        }
    }

    void OnFinishedEvent(bool success)
    {
        playerController.SetInputEnabled(true);
    }
}
