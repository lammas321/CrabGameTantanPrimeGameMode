using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using System.Globalization;
using System.IO;
using System.Reflection;
using static TantanPrimeGameMode.CustomGameModeTantanPrime;

namespace TantanPrimeGameMode
{
    [BepInPlugin($"lammas123.{MyPluginInfo.PLUGIN_NAME}", MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInDependency("lammas123.CustomGameModes")]
    public class TantanPrimeGameMode : BasePlugin
    {
        internal static TantanPrimeGameMode Instance;

        public override void Load()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            Instance = this;

            BindTantanConfigValues(ref snowballDamageMultiplier, ref snowballDamageMultiplierSupreme, "SnowballDamageMultiplier", "Multiply the damage snowballs deal by this number.");
            BindTantanConfigValues(ref maxDifficulty, ref maxDifficultySupreme, "MaxDifficulty", "Difficulty increases as Tantan's health depletes, determining Tantan's turning speed and time between attacks.");

            BindTantanConfigValues(ref meteorAttackMeteors, ref meteorAttackMeteorsSupreme, "MeteorAttackMeteors", "How many meteors should spawn.");
            BindTantanConfigValues(ref meteorAttackExponent, ref meteorAttackExponentSupreme, "MeteorAttackExponent", "Meteors = MeteorAttackMeteors * (LivingPlayers^this)");
            BindTantanConfigValues(ref meteorAttackDelay, ref meteorAttackDelaySupreme, "MeteorAttackDelay", "The time in seconds between each meteor spawning.");

            BindTantanConfigValues(ref spikeAttackSpikes, ref spikeAttackSpikesSupreme, "SpikeAttackSpikes", "How many spikes should spawn.");
            BindTantanConfigValues(ref spikeAttackExponentSupreme, ref spikeAttackExponentSupreme, "SpikeAttackExponent", "Spikes = SpikeAttackSpikes * (LivingPlayers^this)");
            BindTantanConfigValues(ref spikeAttackDelay, ref spikeAttackDelaySupreme, "SpikeAttackDelay", "The time in seconds between each spike spawning.");

            BindTantanConfigValues(ref jumpMeteorAttackMeteors, ref jumpMeteorAttackMeteorsSupreme, "JumpMeteorAttackMeteors", "How many meteors should spawn after landing from a jump.");
            BindTantanConfigValues(ref jumpMeteorAttackExponent, ref jumpMeteorAttackExponentSupreme, "JumpMeteorAttackExponent", "Meteors = JumpMeteorAttackMeteors * (LivingPlayers^this)");
            BindTantanConfigValues(ref jumpMeteorAttackDelay, ref jumpMeteorAttackDelaySupreme, "JumpMeteorAttackDelay", "The time in seconds between each meteor spawning.");

            BindTantanConfigValues(ref slamSpikeAttackSpikes, ref slamSpikeAttackSpikesSupreme, "SlamSpikeAttackSpikes", "How many spikes should spawn after a slam.");
            BindTantanConfigValues(ref slamSpikeAttackExponent, ref slamSpikeAttackExponentSupreme, "SlamSpikeAttackExponent", "Spikes = SlamSpikeAttackSpikes * (LivingPlayers^this)");
            BindTantanConfigValues(ref slamSpikeAttackDelay, ref slamSpikeAttackDelaySupreme, "SlamSpikeAttackDelay", "The time in seconds between each spike spawning.");

            CustomGameModes.Api.RegisterCustomGameMode(new CustomGameModeTantanPrime());

            Log.LogInfo($"Loaded [{MyPluginInfo.PLUGIN_NAME} {MyPluginInfo.PLUGIN_VERSION}]");
        }

        internal void BindTantanConfigValues<T>(ref T value, ref T valueSupreme, string name, string description)
        {
            value = Config.Bind("Tantan Prime", name, value, description).Value;
            valueSupreme = Config.Bind("Tantan SUPREME", name, valueSupreme, description).Value;
        }
    }
}