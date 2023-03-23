using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wacki
{
    [RequireComponent(typeof(OVRControllerHelper))]
    public class OculusUILaserPointer : IUILaserPointer
    {
        private OVRControllerHelper _trackedObject;
        private bool _connected = false;

        protected override void Initialize()
        {
            base.Initialize();

            _trackedObject = GetComponent<OVRControllerHelper>();

        }

        protected new void Update()
        {
            base.Update();

            _connected = OVRInput.IsControllerConnected(_trackedObject.m_controller);
            
        }

        public override bool ButtonDown()
        {
            if (!_connected)
                return false;

            return OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger) || OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger);
        }

        public override bool ButtonUp()
        {
            if (!_connected)
                return false;

            return OVRInput.GetUp(OVRInput.RawButton.LIndexTrigger) || OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger);
        }

        public override void OnEnterControl(GameObject control)
        {
            if (!_connected)
                return;
        }

        public override void OnExitControl(GameObject control)
        {
            if (!_connected)
                return;
        }
    }
}
