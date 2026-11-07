namespace TextMatching;

/// <summary>Kelime içi karakter parçaları (3–5): yazım hatalarına dayanıklı benzerlik.</summary>
public static class NGramVectorizer
{
    public static Dictionary<string, int> Grams(string normalized)
    {
        var sayac = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (var kelime in normalized.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            var d = $" {kelime} ";
            for (var n = 3; n <= 5; n++)
            {
                for (var i = 0; i + n <= d.Length; i++)
                {
                    var g = d.Substring(i, n);
                    sayac[g] = sayac.GetValueOrDefault(g) + 1;
                }
            }
        }
        return sayac;
    }

    public static Dictionary<string, double> Unit(IEnumerable<KeyValuePair<string, double>> weights)
    {
        var v = weights.Where(p => p.Value > 0).ToDictionary(p => p.Key, p => p.Value, StringComparer.Ordinal);
        var boy = Math.Sqrt(v.Values.Sum(x => x * x));
        if (boy == 0) return [];
        foreach (var k in v.Keys.ToList()) v[k] /= boy;
        return v;
    }

    /// <summary>Birim vektörler için kosinüs = iç çarpım.</summary>
    public static double Cosine(IReadOnlyDictionary<string, double> a, IReadOnlyDictionary<string, double> b)
    {
        var (kucuk, buyuk) = a.Count <= b.Count ? (a, b) : (b, a);
        var t = 0.0;
        foreach (var (k, x) in kucuk)
            if (buyuk.TryGetValue(k, out var y)) t += x * y;
        return t;
    }
}
