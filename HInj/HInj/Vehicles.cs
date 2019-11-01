using SDG.Unturned;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


namespace HInj
{
    class Vehicles : MonoBehaviour
    {
        public FieldInfo Engine, hasLockMouse, airsteerMin, airsteerMax, airturnResponsiveness, Lift;

        //Startup
        public void Awake()
        {
            Engine = typeof(VehicleAsset).GetField("_engine", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            hasLockMouse = typeof(VehicleAsset).GetField("_hasLockMouse", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            airsteerMin = typeof(VehicleAsset).GetField("<airSteerMin>k__BackingField", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            airsteerMax = typeof(VehicleAsset).GetField("<airSteerMax>k__BackingField", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            airturnResponsiveness = typeof(VehicleAsset).GetField("<airTurnResponsiveness>k__BackingField", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            Lift = typeof(VehicleAsset).GetField("_lift", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        static InteractableVehicle LastVehicHandle;
        static List<Collider> Colliders = new List<Collider>();
        static EEngine LastVehicEngine;

        //Called every tick
        public void Update()
        {
            if (!Global.VehicleEnabled || Global.AllOff)
                return;

            if (!Provider.isConnected)
                return;

            if (Provider.clients.Count < 1)
                return;

            if (Player.player == null)
                return;

            //Player is in vehicle
            if (Player.player.movement?.getVehicle() != null)
            {
                InteractableVehicle x = Player.player.movement.getVehicle();

                if (Global.VehicleSettings.Fly || Global.VehicleSettings.Ping)
                    if (LastVehicHandle == null)
                    {
                        LastVehicHandle = x;
                        LastVehicEngine = x.asset.engine;
                    }

                if (Global.VehicleSettings.Fly)
                    SetFly(x);

                if (Global.VehicleSettings.Ping)
                {
                    Vector3 dir = Vector3.zero;
                    if (Input.GetKey(KeyCode.UpArrow))
                        dir = MainCamera.instance.transform.forward * Global.VehicleSettings.PingForce;
                    if (Input.GetKey(KeyCode.DownArrow))
                        dir = (MainCamera.instance.transform.forward * -1) * Global.VehicleSettings.PingForce;
                    if (Input.GetKey(KeyCode.KeypadMinus))
                        dir.y += Global.VehicleSettings.PingForce;
                    if (Input.GetKey(KeyCode.KeypadPlus))
                        dir.y -= Global.VehicleSettings.PingForce;

                    if (!Global.VehicleSettings.BypassOne)
                        x.tellRecov(dir + x.transform.position, 1);
                    else
                    {
                        x.GetComponent<Rigidbody>().useGravity = false;
                        x.GetComponent<Rigidbody>().velocity = dir;
                        x.GetComponent<Rigidbody>().rotation = Quaternion.identity;
                    }

                    foreach (Collider p in x.gameObject.GetComponentsInChildren<Collider>())
                        if (p.enabled)
                        {
                            Colliders.Add(p);
                            p.enabled = false;
                        }
                }
            }
            else if (LastVehicHandle != null)
            {
                Engine.SetValue(LastVehicHandle.asset, LastVehicEngine);
                Lift.SetValue(LastVehicHandle.asset, 0f);
                hasLockMouse.SetValue(LastVehicHandle.asset, false);
                foreach (Collider p in Colliders)
                    if (p != null)
                        p.enabled = true;
                Colliders.Clear();
                LastVehicHandle = null;
            }
        }

        public void SetFly(InteractableVehicle pass)
        {
            //This could use work
            Engine.SetValue(pass.asset, EEngine.PLANE);
            hasLockMouse.SetValue(pass.asset, true);
            airsteerMin.SetValue(pass.asset, 1);
            airsteerMax.SetValue(pass.asset, 1);
            airturnResponsiveness.SetValue(pass.asset, 100);
            Lift.SetValue(pass.asset, Global.VehicleSettings.VehicLift);
        }
    }
}