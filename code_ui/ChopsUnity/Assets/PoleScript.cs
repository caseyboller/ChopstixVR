using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleScript : MonoBehaviour {
    public SerialController serialController;
    private ArduinoListener arduino;

    private void Start() {
        arduino = serialController.GetComponent<ArduinoListener>();
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "left") {
            arduino.buzzLeft();
            arduino.leftContact = true;
        }
        if (collision.gameObject.tag == "right") {
            arduino.buzzRight();
            arduino.rightContact = true;
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.tag == "left") {
            arduino.leftContact = false;
        }
        if (collision.gameObject.tag == "right") {
            arduino.rightContact = false;
        }
    }
}
