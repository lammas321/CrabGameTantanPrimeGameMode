using BepInEx.Configuration;
using HarmonyLib;
using System;
using TMPro;
using UnityEngine;

namespace TantanPrimeGameMode
{
    public sealed class CustomGameModeTantanPrime() : CustomGameModes.CustomGameMode
    (
        name: "Tantan Prime",
        description: "• Watch out for falling crabs\n\n• Good luck, you'll need it ;)",
        gameModeType: GameModeType.CrabFight,
        vanillaGameModeType: GameModeType.CrabFight,
        musicType: SongType.Funky,
        waitForRoundOverToDeclareSoloWinner: true,

        shortModeTime: 150,
        mediumModeTime: 150,
        longModeTime: 150,

        compatibleMapNames: [
            "Crabfields",
            "Crabheat",
            "Crabland"
        ]
    )
    {
        internal static float snowballDamageMultiplier = 0.5f;
        internal static float snowballDamageMultiplierSupreme = 0.3f;

        internal static int maxDifficulty = 10; // turning speed and time between attacks, difficulty increases as health depletes
        internal static int maxDifficultySupreme = 20;


        internal static int meteorAttackMeteors = 25;
        internal static int meteorAttackMeteorsSupreme = 35;

        internal static float meteorAttackExponent = 0.4f; // meteors = meteorAttackMeteors * (players^this)
        internal static float meteorAttackExponentSupreme = 0.6f;

        internal static float meteorAttackDelay = 0.4f; // time between each meteor
        internal static float meteorAttackDelaySupreme = 0.3f;


        internal static int spikeAttackSpikes = 15;
        internal static int spikeAttackSpikesSupreme = 20;

        internal static float spikeAttackExponent = 0.3f; // spikes = spikeAttackSpikes * (players^this)
        internal static float spikeAttackExponentSupreme = 0.4f;

        internal static float spikeAttackDelay = 0.7f; // time between each spike
        internal static float spikeAttackDelaySupreme = 0.6f;


        internal static int jumpMeteorAttackMeteors = 12;
        internal static int jumpMeteorAttackMeteorsSupreme = 16;

        internal static float jumpMeteorAttackExponent = 0.3f; // meteors = jumpMeteorAttackMeteors * (players^this)
        internal static float jumpMeteorAttackExponentSupreme = 0.4f;

        internal static float jumpMeteorAttackDelay = 0.3f; // time between each meteor
        internal static float jumpMeteorAttackDelaySupreme = 0.2f;


        internal static int slamSpikeAttackSpikes = 8;
        internal static int slamSpikeAttackSpikesSupreme = 12;

        internal static float slamSpikeAttackExponent = 0.2f; // spikes = slamSpikeAttackSpikes * (players^this)
        internal static float slamSpikeAttackExponentSupreme = 0.3f;

        internal static float slamSpikeAttackDelay = 0.5f; // time between each spike
        internal static float slamSpikeAttackDelaySupreme = 0.4f;


        internal static bool isSupreme = false;

        internal static float SnowballDamageMultiplier
            => isSupreme ? snowballDamageMultiplierSupreme : snowballDamageMultiplier;

        internal static int MaxDifficulty
            => isSupreme ? maxDifficultySupreme : maxDifficulty;


        internal static int MeteorAttackMeteors
            => isSupreme ? meteorAttackMeteorsSupreme : meteorAttackMeteors;

        internal static float MeteorAttackExponent
            => isSupreme ? meteorAttackExponentSupreme : meteorAttackExponent;
        
        internal static float MeteorAttackDelay
            => isSupreme ? meteorAttackDelaySupreme : meteorAttackDelay;


        internal static int SpikeAttackSpikes
            => isSupreme ? spikeAttackSpikesSupreme : spikeAttackSpikes;

        internal static float SpikeAttackExponent
            => isSupreme ? spikeAttackExponentSupreme : spikeAttackExponent;

        internal static float SpikeAttackDelay
            => isSupreme ? spikeAttackDelaySupreme : spikeAttackDelay;


        internal static int JumpMeteorAttackMeteors
            => isSupreme ? jumpMeteorAttackMeteorsSupreme : jumpMeteorAttackMeteors;

        internal static float JumpMeteorAttackExponent
            => isSupreme ? jumpMeteorAttackExponentSupreme : jumpMeteorAttackExponent;

        internal static float JumpMeteorAttackDelay
            => isSupreme ? jumpMeteorAttackDelaySupreme : jumpMeteorAttackDelay;


        internal static int SlamSpikeAttackSpikes
            => isSupreme ? slamSpikeAttackSpikesSupreme : slamSpikeAttackSpikes;

        internal static float SlamSpikeAttackExponent
            => isSupreme ? slamSpikeAttackExponentSupreme : slamSpikeAttackExponent;

        internal static float SlamSpikeAttackDelay
            => isSupreme ? slamSpikeAttackDelaySupreme : slamSpikeAttackDelay;


        internal Harmony patches;

        public override void PreInit()
        {
            isSupreme = false;

            TantanPrimeGameMode.Instance.Config.Reload();

            GetConfigEntry(ref snowballDamageMultiplier, ref snowballDamageMultiplierSupreme, "SnowballDamageMultiplier");
            GetConfigEntry(ref maxDifficulty, ref maxDifficultySupreme, "MaxDifficulty");

            GetConfigEntry(ref meteorAttackMeteors, ref meteorAttackMeteorsSupreme, "MeteorAttackMeteors");
            GetConfigEntry(ref meteorAttackExponent, ref meteorAttackExponentSupreme, "MeteorAttackExponent");
            GetConfigEntry(ref meteorAttackDelay, ref meteorAttackDelaySupreme, "MeteorAttackDelay");

            GetConfigEntry(ref spikeAttackSpikes, ref spikeAttackSpikesSupreme, "SpikeAttackSpikes");
            GetConfigEntry(ref spikeAttackExponent, ref spikeAttackExponentSupreme, "SpikeAttackExponent");
            GetConfigEntry(ref spikeAttackDelay, ref spikeAttackDelaySupreme, "SpikeAttackDelay");

            GetConfigEntry(ref jumpMeteorAttackMeteors, ref jumpMeteorAttackMeteorsSupreme, "JumpMeteorAttackMeteors");
            GetConfigEntry(ref jumpMeteorAttackExponent, ref jumpMeteorAttackExponentSupreme, "JumpMeteorAttackExponent");
            GetConfigEntry(ref jumpMeteorAttackDelay, ref jumpMeteorAttackDelaySupreme, "JumpMeteorAttackDelay");

            GetConfigEntry(ref slamSpikeAttackSpikes, ref slamSpikeAttackSpikesSupreme, "SlamSpikeAttackSpikes");
            GetConfigEntry(ref slamSpikeAttackExponent, ref slamSpikeAttackExponentSupreme, "SlamSpikeAttackExponent");
            GetConfigEntry(ref slamSpikeAttackDelay, ref slamSpikeAttackDelaySupreme, "SlamSpikeAttackDelay");

            patches = Harmony.CreateAndPatchAll(GetType());
        }
        internal void GetConfigEntry<T>(ref T value, ref T valueSupreme, string name)
        {
            if (TantanPrimeGameMode.Instance.Config.TryGetEntry("Tantan Prime", name, out ConfigEntry<T> entry))
                value = entry.Value;
            if (TantanPrimeGameMode.Instance.Config.TryGetEntry("Tantan SUPREME", name, out ConfigEntry<T> entrySupreme))
                valueSupreme = entrySupreme.Value;
        }
        public override void PostEnd()
        {
            isSupreme = false;
            patches?.UnpatchSelf();
        }

        internal static void MakeSupreme()
        {
            isSupreme = true;
            ServerSend.SendChatMessage(1, "Tantan SUPREME has been awakened!");
        }


        // Override Damage
        [HarmonyPatch(typeof(CrabFightCrabManager), nameof(CrabFightCrabManager.DamageCrab))]
        [HarmonyPrefix]
        internal static bool PostCrabFightCrabManagerDamageCrab(CrabFightCrabManager __instance)
        {
            if (__instance.field_Private_Single_0 <= 0f)
                return false;

            __instance.field_Private_Single_0 -= __instance.FindCrabDamage() * SnowballDamageMultiplier;
            if (__instance.field_Private_Single_0 <= 0f)
                ServerSend.CrabAnimation(5);

            int difficulty = Math.Max(1, Mathf.CeilToInt((100f - __instance.field_Private_Single_0) * maxDifficulty / 100f));
            if (__instance.field_Private_Int32_0 != difficulty)
                ServerSend.CrabDifficulty(difficulty);

            return false;
        }

        // Override Meteor Attack
        [HarmonyPatch(typeof(CrabFightCrabAnimator), nameof(CrabFightCrabAnimator.CrabRain))]
        [HarmonyPrefix]
        internal static bool PreCrabFightCrabAnimatorCrabRain(CrabFightCrabAnimator __instance)
        {
            CrabFightCrabBallAttackController ballController = UnityEngine.Object.Instantiate(__instance.crabRain).GetComponent<CrabFightCrabBallAttackController>();
            ballController.ballsToSpawn = (int)(MeteorAttackMeteors * Math.Pow(GameManager.Instance.GetPlayersAlive(), MeteorAttackExponent));
            ballController.delay = MeteorAttackDelay;
            return false;
        }

        // Override Spike Attack
        [HarmonyPatch(typeof(CrabFightCrabAnimator), nameof(CrabFightCrabAnimator.CrabSpikes))]
        [HarmonyPrefix]
        internal static bool PreCrabFightCrabAnimatorCrabSpikes(CrabFightCrabAnimator __instance)
        {
            CrabFightCrabSpikeAttackController spikesController = UnityEngine.Object.Instantiate(__instance.crabSpikes).GetComponent<CrabFightCrabSpikeAttackController>();
            spikesController.ballsToSpawn = (int)(SpikeAttackSpikes * Math.Pow(GameManager.Instance.GetPlayersAlive(), SpikeAttackExponent));
            spikesController.delay = SpikeAttackDelay;
            return false;
        }

        // Jump Attack
        [HarmonyPatch(typeof(CrabFightCrabAnimator), nameof(CrabFightCrabAnimator.ShockwaveRing))]
        [HarmonyPostfix]
        internal static void PostCrabFightCrabAnimatorShockwaveRing(CrabFightCrabAnimator __instance)
        {
            CrabFightCrabBallAttackController ballController = UnityEngine.Object.Instantiate(__instance.crabRain).GetComponent<CrabFightCrabBallAttackController>();
            ballController.ballsToSpawn = (int)(JumpMeteorAttackMeteors * Math.Pow(GameManager.Instance.GetPlayersAlive(), JumpMeteorAttackExponent));
            ballController.delay = JumpMeteorAttackDelay;
        }

        // Slam Attack
        [HarmonyPatch(typeof(CrabFightCrabAnimator), nameof(CrabFightCrabAnimator.CrabSlam))]
        [HarmonyPostfix]
        internal static void PostCrabFightCrabAnimatorCrabSlam(CrabFightCrabAnimator __instance)
        {
            CrabFightCrabSpikeAttackController spikesController = UnityEngine.Object.Instantiate(__instance.crabSpikes).GetComponent<CrabFightCrabSpikeAttackController>();
            spikesController.ballsToSpawn = (int)(SlamSpikeAttackSpikes * Math.Pow(GameManager.Instance.GetPlayersAlive(), SlamSpikeAttackExponent));
            spikesController.delay = SlamSpikeAttackDelay;
        }

        // Tantan Boss Bar Name
        [HarmonyPatch(typeof(CrabFightCrabBossBar), nameof(CrabFightCrabBossBar.ShowCrabUi))]
        [HarmonyPostfix]
        internal static void PostCrabFightCrabBossBarShowCrabUi(CrabFightCrabBossBar __instance)
            => __instance.parent.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = isSupreme ? "Tantan SUPREME" : "Tantan Prime";
    }
}