using BepInEx;
using HarmonyLib;
using System;
using UnityEngine;
using Utilla;
using GorillaNetworking;
using Photon.Pun;
using PlayFab;

namespace GorillaUnlocker
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        bool inRoom;

        void Start()
        {
            /* A lot of Gorilla Tag systems will not be set up when start is called */
            /* Put code in OnGameInitialized to avoid null references */

            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnEnable()
        {
            /* Set up your mod here */
            /* Code here runs at the start and whenever your mod is enabled */

            ApplyHarmonyPatches();
        }

        void OnDisable()
        {
            /* Undo mod setup here */
            /* This provides support for toggling mods with ComputerInterface, please implement it :) */
            /* Code here runs whenever your mod is disabled (including if it disabled on startup) */

            RemoveHarmonyPatches();
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            /* Code here runs after the game initializes (i.e. GorillaLocomotion.Player.Instance != null) */
        }

        void Update()
        {
            /* Code here runs every frame when the mod is enabled */
        }

        /* This attribute tells Utilla to call this method when a modded room is joined */
        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            /* Activate your mod here */
            /* This code will run regardless of if the mod is enabled */

            inRoom = true;
        }

        /* This attribute tells Utilla to call this method when a modded room is left */
        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            /* Deactivate your mod here */
            /* This code will run regardless of if the mod is enabled */

            inRoom = false;
        }

        void ApplyHarmonyPatches()
        {
            var harmony = new Harmony("com.yourname.gorillaunlocker");
            harmony.PatchAll();
        }

        void RemoveHarmonyPatches()
        {
            var harmony = new Harmony("com.yourname.gorillaunlocker");
            harmony.UnpatchSelf();
        }
    }

    [HarmonyPatch(typeof(GorillaComputer))]
    [HarmonyPatch("GeneralFailureMessage")]
    internal class ComputerPatch
    {
        private static bool Prefix(string failMessage)
        {
            Debug.Log("GorillaUnlocker: Patch triggered.");
            if (failMessage.Contains("UNABLE"))
            {
                Debug.Log("GorillaUnlocker: Updating Photon and PlayFab settings.");
                PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = "67da7426-4438-4eb4-a9f0-97cd26994cd3";
                PhotonNetwork.PhotonServerSettings.AppSettings.AppIdVoice = "aad123d1-c6a0-4f01-af59-2346f0e40267";
                PlayFabSettings.TitleId = "C5E1D";
                PhotonNetwork.ConnectUsingSettings();
                return false;
            }
            return true;
        }
    }
}
