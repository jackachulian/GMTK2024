using UnityEngine;

public class CautionZone : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.name + " entered caution zone");
        Player.Instance.inCautionZone = true;
    }   

    private void OnTriggerExit(Collider other) {
        Debug.Log(other.name + " exited caution zone");
        Player.Instance.inCautionZone = false;
    }
}