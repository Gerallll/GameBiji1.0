using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDepan : MonoBehaviour
{
    public GameObject intText;
    public bool interactable, isOpened, toggle;
    public Animator doorAnim;
    public AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound;

    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("MainCamera") && !isOpened) // Tambahkan kondisi isOpened
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
        if(interactable == true && !isOpened) // Hanya interaksi jika pintu belum dibuka
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                toggle = !toggle;
                if(toggle == true)
                {
                    doorAnim.ResetTrigger("close");
                    doorAnim.SetTrigger("open");
                    audioSource.PlayOneShot(openSound);
                    isOpened = true; // Tandai bahwa pintu telah dibuka
                }
                intText.SetActive(false);
                interactable = false;
            }
        }
    }
}
