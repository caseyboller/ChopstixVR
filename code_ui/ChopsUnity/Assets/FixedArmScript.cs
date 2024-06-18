using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedArmScript : MonoBehaviour
{

    public SerialController serialController;
    private ArduinoListener arduino;

    private void Start() {
        arduino = serialController.GetComponent<ArduinoListener>();
    }

    private void OnCollisionEnter(Collision collision) {
        arduino.buzzRight();
        arduino.rightContact = true;
    }

    private void OnCollisionExit(Collision collision) {
        arduino.rightContact = false;
    }
}
