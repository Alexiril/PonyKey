using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Engine.BaseSystems;

public static class PlayerSettings
{
    public static void ForceUpdate() => _values = GetValues();

    public static void SetValue(string valueName, string value)
    {
        if (!_valuesActual)
        {
            _values = GetValues();
            _valuesActual = true;
        }
        _values[valueName] = value;
        ResetValues(_values);
    }

    public static void SetValues(Dictionary<string, string> values)
    {
        if (!_valuesActual)
        {
            _values = GetValues();
            _valuesActual = true;
        }
        foreach (var value in values)
            _values[value.Key] = value.Value;
        ResetValues(_values);
    }

    public static string GetValue(string valueName)
    {
        if (_valuesActual) return _values.GetValueOrDefault(valueName);
        _values = GetValues();
        _valuesActual = true;
        return _values.GetValueOrDefault(valueName);
    }

    private static Dictionary<string, string> GetValues()
    {
        var buffer = File.Exists("settings") ? File.ReadAllText("settings") : "";
        return buffer != "" ?
            buffer.Split(";").ToDictionary(s => s.Split(":")[0], s => s.Split(":")[1]) :
            new Dictionary<string, string>();
    }

    private static void ResetValues(Dictionary<string, string> values)
    {
        var file = new FileStream("settings", FileMode.Create);
        file.Write(Encoding.UTF8.GetBytes(
            string.Join(";", values.Select(x => $"{x.Key}:{x.Value}"))));
        file.Dispose();
    }

    private static Dictionary<string, string> _values;

    private static bool _valuesActual;
}
