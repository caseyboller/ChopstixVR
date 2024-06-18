using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArduinoListener : MonoBehaviour
{
    public SerialController serialController;

    public GameObject fixedArm;
    public GameObject rotatingArm;

    public bool overrideAngle = false;
    public float angleOverride = 0;

    public bool rightContact;
    public bool leftContact;

    public bool useGyro = false;

    bool magOverrideOn = false;
    bool magOn = false;
    bool leftOn = false;
    bool rightOn = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void buzzLeft() {
        StartCoroutine(buzz(0));
    }

    public void buzzRight() {
        StartCoroutine(buzz(1));
    }

    // 0 left, 1 right
    IEnumerator buzz(int motor) {
        if (motor == 0) {
            if (!leftOn) {
                Debug.Log("Sending L:1");
                serialController.SendSerialMessage("L:1");
                leftOn = true;
            }
        } else {
            if (!rightOn) {
                Debug.Log("Sending R:1");
                serialController.SendSerialMessage("R:1");
                rightOn = true;
            }
        }

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(0.5f);

        if (motor == 0) {
            Debug.Log("Sending L:0");
            serialController.SendSerialMessage("L:0");
            leftOn = false;
        } else {
            Debug.Log("Sending R:0");
            serialController.SendSerialMessage("R:0");
            rightOn = false;
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (leftContact && rightContact) {
            if (!magOn) {
                Debug.Log("Sending M:1");
                serialController.SendSerialMessage("M:1");
                magOn = true;
            }
        }

        if (!(leftContact && rightContact)) {
            if (magOn && !(magOverrideOn)) {
                Debug.Log("Sending M:0");
                serialController.SendSerialMessage("M:0");
                magOn = false;
            }
        }

        // Magnet
        if (Input.GetKeyDown(KeyCode.M)) {
            if (!magOn) {
                Debug.Log("Sending M:1");
                serialController.SendSerialMessage("M:1");
                magOn = true;
                magOverrideOn = true;
            } else {
                Debug.Log("Sending M:0");
                serialController.SendSerialMessage("M:0");
                magOn = false;
                magOverrideOn = false;
            }

        }

        // Left Motor
        if (Input.GetKeyDown(KeyCode.L)) {
            buzzLeft();

        }

        // Right Motor
        if (Input.GetKeyDown(KeyCode.R)) {
            buzzRight();
        }

        if (overrideAngle) {
            float angle = angleOverride;
            rotatingArm.transform.localRotation = Quaternion.Slerp(
                rotatingArm.transform.localRotation,
                Quaternion.Euler(0f, angle, 0f), 100f * Time.deltaTime);
        }

    }

    private void OnCollisionEnter(Collision collision) {
        //if (collision.gameObject.tag != "Player") {
            Debug.Log("Collision!");
        //}
        
    }

    void OnMessageArrived(string msg) {
        // Debug.Log("Arrived: " + msg);
        try {
            // Potentiometer reading
            if (msg.StartsWith("P")) {
                if (!overrideAngle) {
                    float angle = float.Parse(msg.Substring(msg.IndexOf(':') + 1)) / 1028 * 360;

                    Debug.Log("Angle: " + angle);
                    //rotatingArm.transform.Rotate(0, angle, 0);
                    rotatingArm.transform.localRotation = Quaternion.Slerp(
                        rotatingArm.transform.localRotation,
                        Quaternion.Euler(0f, angle, 0f), 100f * Time.deltaTime);
                }
            }
            // Quaternion
            else if (useGyro && msg.StartsWith("Q")) {
                var values = msg.Substring(msg.IndexOf(':') + 1).Split(';');

                float w = float.Parse(values[0]);
                float x = float.Parse(values[1]);
                float y = float.Parse(values[2]);
                float z = float.Parse(values[3]);
                this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, new Quaternion(w, y, x, z), Time.deltaTime * 100.0f);
            }
        } catch (Exception e) { Debug.Log(e); }
    }

    void OnConnectionEvent(bool success) {
        //Debug.Log(success ? "Device connected" : "Device disconnected");
    }
}
