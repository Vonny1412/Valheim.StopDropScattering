using HarmonyLib;
using System.Collections;
using UnityEngine;

namespace StopDropScattering.Patches
{
    [HarmonyPatch(typeof(ItemDrop), "Awake")]
    static class ItemDrop_Awake_Patch
    {
        static void Postfix(ItemDrop __instance)
        {
            if (!__instance || !__instance.transform || __instance.transform.position.y < 4500)
            {
                return;
            }
            __instance.StartCoroutine(CheckIfFalling(__instance));
        }

        static IEnumerator CheckIfFalling(ItemDrop itemDrop)
        {

            Rigidbody rb = itemDrop.GetComponent<Rigidbody>();
            if (!rb) yield break;
            
            Transform t = itemDrop.transform;
            var lastY = t.position.y;

            for (var i=10; i>0; i--) // 10 * 0.5 = 5 sec
            {
                yield return new WaitForSeconds(0.5f);
                if (!itemDrop) yield break;

                var distY = lastY - t.position.y;
                if (distY <= 0.01f) yield break;

                if (distY > 10)
                {
                    var isServer = ZNet.instance.IsServer();
                    var itemName = itemDrop.gameObject.name.Replace("(Clone)", "");
                    var player = Player.GetClosestPlayer(t.position, 100f);
                    if (player)
                    {
                        if (isServer)
                        {
                            Plugin.Log.LogInfo($"{player.GetPlayerName()} catches {itemName}");
                        }
                        else if (Player.m_localPlayer == player)
                        {
                            Plugin.Log.LogInfo($"Catching {itemName}");
                        }
                    }
                    else
                    {
                        if (isServer)
                        {
                            Plugin.Log.LogInfo($"No player found to catch {itemName}");
                        }
                        yield break;
                    }

                    ZNetView nview = itemDrop.GetComponent<ZNetView>();
                    if (!nview || !nview.IsValid() || !nview.IsOwner())
                    {
                        yield break;
                    }

                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.position = player.transform.position + Vector3.up * 1f;

                    yield break;
                }

                lastY = t.position.y;
            }
        }

    }
}
