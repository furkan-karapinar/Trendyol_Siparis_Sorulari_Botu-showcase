using System.Globalization;
using System.Text;

namespace TextMatching;

public static class TextNormalizer
{
    private static readonly CultureInfo Tr = CultureInfo.GetCultureInfo("tr-TR");

    public static string Normalize(string? text)
    {
        if (string.IsNullOrWhiteSpace(text)) return "";

        var kucuk = text.ToLower(Tr);
        var sb = new StringBuilder(kucuk.Length);
        var oncekiBosluk = true;

        foreach (var ch in kucuk)
        {
            var c = ch switch
            {
                'ı' => 'i', 'ş' => 's', 'ğ' => 'g', 'ü' => 'u', 'ö' => 'o', 'ç' => 'c',
                'â' => 'a', 'î' => 'i', 'û' => 'u',
                _ => ch
            };

            // Birleşik aksanlar ve harf olmayan simgeler (emoji vb.) ayırıcı sayılır.
            if (char.IsLetterOrDigit(c) && c < 0x0300)
            {
                sb.Append(c);
                oncekiBosluk = false;
            }
            else if (!oncekiBosluk)
            {
                sb.Append(' ');
                oncekiBosluk = true;
            }
        }

        return sb.ToString().TrimEnd();
    }
}
