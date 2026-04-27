using System.Text.RegularExpressions;
using Minecraft.Server.FourKit.Plugin;

namespace Respublica.Utils;

public class Version : IComparable<Version>
{
    private readonly string _version;
    private static readonly Regex _separator = new(@"\.");
    private static readonly Regex _version_pattern = new(@"[0-9]+(" + _separator + "[0-9]+)*");
    private readonly string[] _components;

    private Version(string version)
    {
        _version = version;
        _components = version.Split(_separator.ToString());
    }

    public static Version FromString(string version)
    {
        if (string.IsNullOrEmpty(version)) throw new ArgumentException("Version can not be null");
        var matcher = _version_pattern.Match(version);
        if (!matcher.Success) throw new ArgumentException($"Invalid version format: {version}");

        return new Version(matcher.Value);
    }

    public static Version FromPlugin(ServerPlugin plugin)
    {
        var version = plugin.version;
        var matcher = _version_pattern.Match(version);
        if (!matcher.Success) throw new ArgumentException($"Invalid version format: {version}");

        return new Version(matcher.Value);
    }

    public int CompareTo(Version? that)
    {
        if (that == null) return 1;
        int length = Math.Max(_components.Length, that._components.Length);

        for (int i=0; i < length; i++)
        {
            int thisPart = i < _components.Length ? int.Parse(_components[i]) : 0;
            int thatPart = i < that._components.Length ? int.Parse(that._components[i]) : 0;

            if (thisPart < thatPart) return -1;

            if (thisPart > thatPart) return 1;
        }
        return 0;
    }

    public override bool Equals(object? obj)
    {
        if (this == obj) return true;
        if (obj is not Version) return false;
        
        var version1 = (Version)obj;
        return Equals(_version, version1._version);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_version);
    }

    public override string ToString()
    {
        return _version;
    }

    public string[] GetComponents()
    {
        return _components;
    }

    public bool IsPreRelease()
    {
        try {
            return int.Parse(_components[_components.Length-1]) != 0;
        } catch {return false;}
    }

    public bool IsNewerThanOrEquals(Version other) => CompareTo(other) >= 0;
    public bool IsNewerThan(Version other) => CompareTo(other) > 0;
    public bool IsOlderThanOrEquals(Version other) => CompareTo(other) <= 0;
    public bool IsOlderThan(Version other) => CompareTo(other) < 0;
}