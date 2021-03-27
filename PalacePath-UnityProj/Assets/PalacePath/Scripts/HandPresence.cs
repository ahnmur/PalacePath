using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR; //so we can access the player's controllers

//Note to self: Ctrl K C - comment out 
//              Ctrl K U - uncomment

public class HandPresence : MonoBehaviour
{
    public bool showController = false; //variable that controls whether the hand models or the controller models are shown
    public InputDeviceCharacteristics controllerCharacteristics;
    public List<GameObject> controllerPrefabs; //populate this field with models of controllers in the inspector
    public GameObject handModelPrefab;
    private InputDevice targetDevice;
    private GameObject spawnedController;
    private GameObject spawnedHandModel;
    private Animator handAnimator;

    // Start is called before the first frame update
    void Start()
    {
        TryInitialize();
    }

    //checks if target device is there, we can keep checking in case controllers are not there at the start
    void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>(); //List all the detect devices (essentially, the L and R controllers)

        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        foreach (var item in devices) //we can now access each device paramter with "item.xxxx"
        {
            Debug.Log(item.name + item.characteristics); //this will write the name + characteristics of each detected device to the console
        }

        if (devices.Count > 0) //check if player has controllers, if so... run this script:
        {
            targetDevice = devices[0]; //listen for controllers, we've specified the targetDevice as the device we want to listen for
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
            if (prefab)
            {
                spawnedController = Instantiate(prefab, transform); //instantiates the controller prefab based on whatever controller is detected
            }
            else
            {
                Debug.LogError("Did not find corresponding controller model"); //In case the player has a controller we haven't anticipated
                spawnedController = Instantiate(controllerPrefabs[0], transform);
            }

            spawnedHandModel = Instantiate(handModelPrefab, transform); //spawn hand model as child of player's controllers
            handAnimator = spawnedHandModel.GetComponent<Animator>();
        }
    }

    void UpdateHandAnimation()
    {
        //the TryGetFeatureValue function takes 2 parameters: (button we want to listen to, the output value)
        //the output value can be one of three types:
        //boolean (pressed or not), float (values between 0 and1), and vector 2 (with the joystick)
        if(targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
            Debug.Log("Trigger pressed: " + triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
            Debug.Log("Grip pressed: " + gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!targetDevice.isValid) // the ! in C# manes "not" ie. if isValid is not true, search for controllers again, and if we've found them, spawn the hand models.
        {
            TryInitialize();
        }
        else
        {
            if (showController) //script to run if we choose to show the controller models instead of the hand models
            {
                spawnedHandModel.SetActive(false);
                spawnedController.SetActive(true);
            }
            else
            {
                spawnedHandModel.SetActive(true);
                spawnedController.SetActive(false);
                UpdateHandAnimation();
            }
        }
    }
}
