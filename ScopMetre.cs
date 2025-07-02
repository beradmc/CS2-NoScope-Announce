using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Utils;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ScopMetre;

public class ScopMetre : BasePlugin
{
    public override string ModuleName => "ScopMetre";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "beratfps";
    public override string ModuleDescription => "No-scope bildirimi yapan plugin";

    private string _prefix = "[NoScope]";

    // Renk kodları (CS2 chat için)
    private const string PrefixColor = "";   // Beyaz (renksiz)
    private const string NameColor = "\x0E";     // Pembe
    private const string WeaponColor = "\x06";   // Yeşil
    private const string DistanceColor = "\x10"; // Sarı

    public override void Load(bool hotReload)
    {
        // Config klasörü ve dosyasını otomatik oluştur
        var configDir = Path.Combine(ModuleDirectory, "config");
        if (!Directory.Exists(configDir))
            Directory.CreateDirectory(configDir);
        var configPath = Path.Combine(configDir, "ScopMetreConfig.json");
        if (!File.Exists(configPath))
        {
            var defaultConfig = new Config { Prefix = "[NoScope]" };
            File.WriteAllText(configPath, JsonSerializer.Serialize(defaultConfig, new JsonSerializerOptions { WriteIndented = true }));
        }
        // Config dosyasını oku
        try
        {
            var configText = File.ReadAllText(configPath);
            var config = JsonSerializer.Deserialize<Config>(configText);
            if (config != null && !string.IsNullOrEmpty(config.Prefix))
                _prefix = config.Prefix;
        }
        catch { }

        RegisterEventHandler<EventPlayerDeath>(OnPlayerDeath);
    }

    private HookResult OnPlayerDeath(EventPlayerDeath @event, GameEventInfo info)
    {
        var attacker = @event.Attacker;
        var victim = @event.Userid;
        if (attacker == null || victim == null || !attacker.IsValid || !victim.IsValid) return HookResult.Continue;
        if (attacker == victim) return HookResult.Continue; // Self-kill

        var weapon = @event.Weapon;
        if (!IsNoScopeWeapon(weapon)) return HookResult.Continue;

        // Dürbün açık mı kontrolü (no-scope için)
        if (IsPlayerScoped(attacker)) return HookResult.Continue;

        // Mesafe hesapla
        var attackerPos = attacker.Pawn.Value?.AbsOrigin;
        var victimPos = victim.Pawn.Value?.AbsOrigin;
        if (attackerPos == null || victimPos == null) return HookResult.Continue;
        var distance = GetDistance(attackerPos, victimPos);

        // Silah ismini al
        string weaponName = weapon?.ToUpper() switch {
            var w when w != null && w.Contains("AWP") => "AWP",
            var w when w != null && w.Contains("SSG08") => "SSG08",
            _ => weapon ?? "?"
        };

        // Mesajı oluştur
        string message = GetColoredText("{gold}" + _prefix + " {pink}" + attacker.PlayerName + " {green}" + weaponName + " ile dürbünsüz vurdu {yellow}[" + distance.ToString("F1") + "mt]");
        Server.PrintToChatAll(message);
        return HookResult.Continue;
    }

    private bool IsNoScopeWeapon(string? weapon)
    {
        if (string.IsNullOrEmpty(weapon)) return false;
        weapon = weapon.ToLower();
        return weapon.Contains("awp") || weapon.Contains("ssg08");
    }

    private bool IsPlayerScoped(CCSPlayerController player)
    {
        var pawn = player.PlayerPawn.Value as CounterStrikeSharp.API.Core.CCSPlayerPawn;
        if (pawn == null) return false;
        return pawn.IsScoped;
    }

    private float GetDistance(Vector attacker, Vector victim)
    {
        // CS2 birimleri metreye çevrilir (1 birim ≈ 0.0254 metre)
        float dx = attacker.X - victim.X;
        float dy = attacker.Y - victim.Y;
        float dz = attacker.Z - victim.Z;
        float units = MathF.Sqrt(dx * dx + dy * dy + dz * dz);
        return units * 0.0254f;
    }

    private static string GetColoredText(string message)
    {
        Dictionary<string, int> colorMap = new()
        {
            { "{default}", 1 },
            { "{white}", 1 },
            { "{darkred}", 2 },
            { "{purple}", 3},
            { "{green}", 4 },
            { "{lightgreen}", 5 },
            { "{slimegreen}", 6 },
            { "{red}", 7 },
            { "{grey}", 8 },
            { "{yellow}", 9 },
            { "{invisible}", 10 },
            { "{lightblue}", 11 },
            { "{blue}", 12 },
            { "{lightpurple}", 13 },
            { "{pink}", 14 },
            { "{fadedred}", 15 },
            { "{gold}", 16 },
        };

        string pattern = "{(\\w+)}";
        string replaced = Regex.Replace(message, pattern, match =>
        {
            string colorCode = match.Groups[1].Value;
            if (colorMap.TryGetValue("{" + colorCode + "}", out int replacement))
            {
                return Convert.ToChar(replacement).ToString();
            }
            return match.Value;
        });
        return $"\u200B{replaced}";
    }

    private class Config
    {
        public string Prefix { get; set; } = "[NoScope]";
    }
}
