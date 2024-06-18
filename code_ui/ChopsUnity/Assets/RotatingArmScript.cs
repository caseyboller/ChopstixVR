using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingArmScript : MonoBehaviour
{
    public SerialController serialController;
    private ArduinoListener arduino;

    private void Start() {
        arduino = serialController.GetComponent<ArduinoListener>();
    }

    private void OnCollisionEnter(Collision collision) {
        arduino.buzzLeft();
        arduino.leftContact = true;
    }

    private void OnCollisionExit(Collision collision) {
        arduino.leftContact = false;
    }
}
