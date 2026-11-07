using TextMatching;

namespace TextMatching.Tests;

public sealed class TextNormalizerTests
{
    [Theory]
    [InlineData("Siparişim NE ZAMAN kargoya verilecek??", "siparisim ne zaman kargoya verilecek")]
    [InlineData("IŞIK İÇİN", "isik icin")]
    [InlineData("14:00'dan önce 🎉🎉", "14 00 dan once")]
    [InlineData("  çok   teşekkürler\nkolay gelsin ", "cok tesekkurler kolay gelsin")]
    [InlineData("Hâlâ gelmedi", "hala gelmedi")]
    [InlineData("🎉🎉🎉", "")]
    [InlineData("", "")]
    [InlineData(null, "")]
    public void Normalize_Cases(string? input, string expected) =>
        Assert.Equal(expected, TextNormalizer.Normalize(input));
}

public sealed class NGramVectorizerTests
{
    private static Dictionary<string, double> Vec(string text) =>
        NGramVectorizer.Unit(NGramVectorizer.Grams(TextNormalizer.Normalize(text))
            .Select(p => new KeyValuePair<string, double>(p.Key, p.Value)));

    [Fact]
    public void Grams_AddsWordBoundaries()
    {
        var g = NGramVectorizer.Grams("kargo");
        Assert.Contains(" ka", g.Keys);
        Assert.Contains("go ", g.Keys);
        Assert.Contains(" karg", g.Keys);
    }

    [Fact]
    public void Cosine_OfSameText_IsOne() =>
        Assert.Equal(1.0, NGramVectorizer.Cosine(Vec("Kargom nerede?"), Vec("kargom nerede")), 6);

    [Fact]
    public void Cosine_IsTolerantToTypos()
    {
        var soru = Vec("siparişim ne zaman kargoya verilecek");
        var yazimHatali = Vec("siparisim ne zman kargoya verilcek");
        var alakasiz = Vec("iade kodunu nasıl alırım");

        Assert.True(NGramVectorizer.Cosine(soru, yazimHatali) > 0.6);
        Assert.True(NGramVectorizer.Cosine(soru, yazimHatali) > NGramVectorizer.Cosine(soru, alakasiz));
    }

    [Fact]
    public void Unit_OfEmpty_IsEmpty() =>
        Assert.Empty(NGramVectorizer.Unit([]));
}
