# Örnek kod — TextMatching

Bu klasör, asıl (gizli) projeden alınmış küçük ve **işletmeye özgü hiçbir bilgi içermeyen** bir parçadır: müşteri sorusunu benzer geçmiş sorularla eşleştiren metin normalizasyonu ve karakter n-gram benzerliği.
API anahtarı, mağaza adresi, müşteri verisi ya da ticari iş mantığı içermez; tek başına derlenir ve test edilir.

| Dosya | Ne yapar |
|---|---|
| [`TextNormalizer.cs`](src/TextMatching/TextNormalizer.cs) | Türkçe kurallarıyla küçük harfe çevirir (`I→ı`, `İ→i`), `ş/ğ/ü/ö/ç/ı` ve şapkalı harfleri sadeleştirir, emoji/noktalama/çoklu boşluğu tek ayırıcıya indirir. |
| [`NGramVectorizer.cs`](src/TextMatching/NGramVectorizer.cs) | Kelime içi 3–5 karakterlik parçalardan vektör üretir, birim vektöre çevirir ve kosinüs benzerliği hesaplar. Yazım hatalarına dayanıklıdır (“verilcek” ≈ “verilecek”). |

Gerçek projede bu iki parça, TF-IDF ağırlıklandırma ve şablon gruplama ile birlikte gelen soruya en uygun hazır cevabı önermek için kullanılır. Testler Türkçe karakter, emoji ve yazım hatası durumlarını kapsar.

## Çalıştırma

Gerekli: [.NET 9 SDK](https://dotnet.microsoft.com/download)

```bash
cd ornek-kod
dotnet test
```
