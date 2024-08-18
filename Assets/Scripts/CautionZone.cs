using UnityEngine;

public class CautionZone : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.name + " entered caution zone");
    }   

    private void OnTriggerExit(Collider other) {
        Debug.Log(other.name + " exited caution zone");
    }
}