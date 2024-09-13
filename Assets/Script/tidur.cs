using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tidur : MonoBehaviour
{
    public Dialogpintu dialogPintu;
    public GameObject intText;
    public bool interactable;
    public Collider laci, laci2, meja, key, lemari1, lemari2, laci4;

    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("MainCamera"))
        {
            intText.SetActive(true);
            interactable = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("MainCamera"))
        {
            intText.SetActive(false);
            interactable = false;
        }
    }

    void Update()
    {
        if(interactable == true)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("udah Turu");
                dialogPintu.hasTuru = true;
                laci.enabled = true;
                laci2.enabled = true;
                meja.enabled = true;
                key.enabled = true;
                lemari1.enabled = true;
                lemari2.enabled = true;
                laci4.enabled = true;
            }
        }
    }


}
