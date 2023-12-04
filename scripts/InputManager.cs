using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class InputManager : MonoBehaviour
{
    public TMP_Text orientationText;
    public TMP_Text accelerationText;
    public TMP_Text latitudeText;
    public TMP_Text longitudeText;
    public TMP_Text altitudeText;

    public TMP_Text angularVelocityText;

    public float lowLimitLattiude = 0;
    public float highLimitLattiude = 0;

    public float lowLimitLongitude = 0;
    public float highLimitLongitude = 0;

    public Transform rotator;

    public float smoothing = 0.1f;
    
    void Awake()
    {
        Input.location.Start();
        Input.compass.enabled = true;
        Input.gyro.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        var trueHeading = Input.compass.trueHeading;
        var acceleration = Input.acceleration;
        var latitude = Input.location.status == LocationServiceStatus.Running ? Input.location.lastData.latitude : 0;
        var longitude = Input.location.status == LocationServiceStatus.Running ? Input.location.lastData.longitude : 0;
        var altitude = Input.location.status == LocationServiceStatus.Running ? Input.location.lastData.altitude : 0;
        var angularVelocity = Input.gyro.attitude;
        Quaternion attitude = angularVelocity;

        rotator.rotation = attitude;
        rotator.Rotate(0f, 0f, 180f, Space.Self);
        rotator.Rotate(90f, 180f, 0f, Space.World);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotator.rotation, smoothing);

        var isInLatitudeRange = latitude > lowLimitLattiude && latitude < highLimitLattiude;
        var isInLongitudeRange = longitude > lowLimitLongitude && longitude < highLimitLongitude;

        if (isInLatitudeRange && isInLongitudeRange) {
            transform.Translate(0,0, acceleration.x * Time.deltaTime * 10, Space.World);
        }

        orientationText.text = "Orientation: " + trueHeading;
        accelerationText.text = "Acceleration: " + acceleration;
        latitudeText.text = "Latitude: " + latitude;
        longitudeText.text = "Longitude: " + longitude;
        altitudeText.text = "Altitude: " + altitude;
        angularVelocityText.text = "Angular Velocity: " + angularVelocity;
    }
}