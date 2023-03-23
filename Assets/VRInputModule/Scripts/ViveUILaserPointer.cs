using UnityEngine;
using System.Collections;
//using Valve.VR;

namespace Wacki
{
    //[RequireComponent(typeof(SteamVR_Behaviour_Pose))]
    public class ViveUILaserPointer : MonoBehaviour//IUILaserPointer
    {
        /*
        public SteamVR_Action_Boolean trigger;
        public SteamVR_Action_Vibration hapticAction;

        private SteamVR_Behaviour_Pose _trackedObject;
        private SteamVR_Input_Sources inputSource;
        private bool _connected = false;

        protected override void Initialize()
        {
            base.Initialize();

            _trackedObject = GetComponent<SteamVR_Behaviour_Pose>();

            if(_trackedObject != null) {
                _connected = true;
            }

            inputSource = _trackedObject.inputSource;
        }

        public override bool ButtonDown()
        {
            if(!_connected)
                return false;

            return trigger.GetStateDown(inputSource);
        }

        public override bool ButtonUp()
        {
            if(!_connected)
                return false;

            return trigger.GetStateUp(inputSource);
        }
        
        public override void OnEnterControl(GameObject control)
        {
            if (!_connected)
                return;
            
            //hapticAction.Execute(0, 0.4f, 10.0f, 0.1f, inputSource);
        }

        public override void OnExitControl(GameObject control)
        {
            if (!_connected)
                return;
        }
        */
    }
}