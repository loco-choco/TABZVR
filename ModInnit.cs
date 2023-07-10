using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VR;
using BepInEx;
using HarmonyLib;

namespace TABZVR
{
    [BepInPlugin("Locochoco.plugins.TABZVR", "TABZ VR", "1.0.0.0")]
    [BepInProcess("GAME.exe")] //TABZ executable
    public class ModInnit : BaseUnityPlugin
    {
        static public Transform leftHand;
        static public Transform rightHand;

        public void Awake()
        {
            try
            {
                VRDevice.SetTrackingSpaceType(TrackingSpaceType.Stationary);
                Harmony harmony = new Harmony("Locochoco.plugins.TABZVR");
                WeaponHandlerPatchs.ApplyPatchs(harmony);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }

        public void Start()
        {
            CreateVRHands();
            //StartCoroutine("RecenterHead");
        }

        private IEnumerable RecenterHead()
        {
            while (true)
            {
                yield return new WaitForSeconds(2f);
                InputTracking.Recenter();
            }
        }

        public void Update()
        {
            if (VRDevice.isPresent && leftHand == null && rightHand == null)
            {
                CreateVRHands();
                InputTracking.Recenter();
            }

            if (Input.GetKeyUp(KeyCode.T))
                InputTracking.Recenter();
        }

        private void CreateVRHands()
        {
            Camera mainCamera = Camera.main;
            GameObject leftHandGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            leftHand = leftHandGo.transform;
            leftHandGo.GetComponent<Collider>().enabled = false;
            leftHand.parent = mainCamera.transform.parent;
            leftHandGo.AddComponent<FollowVRNode>().SetVRNode(VRNode.LeftHand);
            leftHand.localScale = Vector3.one / 5f;


            GameObject rightHandGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            rightHand = rightHandGo.transform;
            rightHandGo.GetComponent<Collider>().enabled = false;
            rightHandGo.transform.parent = mainCamera.transform.parent;
            rightHandGo.AddComponent<FollowVRNode>().SetVRNode(VRNode.RightHand);
            rightHand.localScale = Vector3.one / 5f;
        }
    }
}