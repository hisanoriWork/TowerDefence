using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace MyLibrary
{
    public class Utility
    {
        public static void SetLayerRecursively(GameObject self, string layerName)
        {
            int layer = LayerMask.NameToLayer(layerName);
            SetLayerRecursively(self, layer);
        }

        public static void SetLayerRecursively(GameObject self, int layer)
        {
            self.layer = layer;

            foreach (Transform n in self.transform)
            {
                SetLayerRecursively(n.gameObject, layer);
            }
        }
        public static IEnumerator WaitForSecond(float time, UnityEvent voidEvent)
        {
            yield return new WaitForSeconds(time);
            voidEvent.Invoke();
        }
    }

    public class Constant
    {
        public static string PngnLayer1 = "Player1";
        public static string ShipLayer1 = "PlayerShip1";
        public static string WeaponLayer1 = "PlayerWeapon1";
        public static string PngnLayer2 = "Player2";
        public static string ShipLayer2 = "PlayerShip2";
        public static string WeaponLayer2 = "PlayerWeapon2";
    }
}
