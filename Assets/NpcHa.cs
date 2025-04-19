using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcHa : MonoBehaviour
{
    public TalkPanel talk;
    public bool CanDo = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            _ = GameApp.Instance.uiManager.GetTipUi("Presse E to trig talk");

            CanDo = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _ = GameApp.Instance.uiManager.GetTipUi("you re away from npc");
            CanDo = false;
        }
    }
    private void Update()
    {
        if(CanDo && talk.CanActive && Input.GetKeyDown(KeyCode.E))
        {
            talk.gameObject.SetActive(true);
        }
    }
}
