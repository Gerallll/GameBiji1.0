using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggerawal : MonoBehaviour
{
    public Animator doorAnim;  // Animasi pintu
    public string closeTriggerName = "close";  // Nama trigger untuk menutup pintu
    public AudioSource audioSource;  // Sumber audio untuk suara pintu
    public AudioClip closeSound;  // Suara saat pintu ditutup

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Saat player memasuki trigger
        {
            doorAnim.SetTrigger(closeTriggerName);  // Aktifkan animasi menutup pintu
            audioSource.PlayOneShot(closeSound);  // Putar suara menutup pintu
            Destroy(gameObject);  // Hancurkan trigger setelah digunakan sekali
        }
    }
}
