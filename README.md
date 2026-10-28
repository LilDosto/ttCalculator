# 📐 ttCalculator v2.1.0

[![GitHub release](https://img.shields.io/github/v/release/LilDosto/ttCalculator?color=brightgreen&label=S%C3%BCr%C3%BCm)](https://github.com/LilDosto/ttCalculator/releases)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8.0--windows-purple.svg)](https://dotnet.microsoft.com/)
[![Platform](https://img.shields.io/badge/Platform-Windows%20x64-lightgrey.svg)](https://microsoft.com)

**ttCalculator**, 128-bit yüksek hassasiyetli matematik motoru, gelişmiş nümerik yöntemler, matris dekompozisyonları, dinamik grafik çizimleri ve çok değişkenli analizler gerçekleştirebilen modern ve profesyonel bir masaüstü matematik yazılımıdır.

Tüm matematiksel çekirdek, 1.169 otomatik testten oluşan kapsamlı doğrulama paketinden **%100 başarıyla (0 hata)** geçirilmiş ve son kullanıcıya hazır hale getirilmiştir.

---

## 🚀 Hızlı Başlangıç (Kurulumsuz Doğrudan Çalıştırma)

Uygulamanın hazır derlenmiş Windows sürümünü (`ttCalculator.exe`) hemen çalıştırmak için:

1. **[Releases](https://github.com/LilDosto/ttCalculator/releases/tag/v2.1.0)** sayfasına gidin.
2. **`ttCalculator.exe`** (veya `ttCalculator-v2.1.0-win-x64.zip`) dosyasını indirin.
3. Çift tıklayarak doğrudan çalıştırın — ek bağımlılık gerektirmez!

---

## ✨ v2.1.0 Temel Özellikleri & Modüller

Uygulama modern koyu tema (Dark Theme) arayüzü ile 9 ana modülden oluşmaktadır:

### 1. 🔢 Basit & Bilimsel Hesap Makinesi
* **128-Bit Yüksek Hassasiyet:** Standart IEEE-754 çift duyarlılık kayıpları yerine tam hassasiyetli `decimal` aritmetik.
* **Açı Modu (RAD / DEG):** Tek tıkla Radyan / Derece birimi değişimi.
* **Kapsamlı Fonksiyon Desteği:** `sin`, `cos`, `tan`, `cot`, `asin`, `acos`, `atan`, `atan2`, `sinh`, `cosh`, `tanh`, `ln`, `log10`, `log(x, base)`, `exp`, `sqrt`, `cbrt`, `abs`, `max`, `min`.
* **Kullanıcı Kolaylıkları:** Otomatik parantez tamamlama, geçmiş (history) paneli ve çift tıklamayla geri yükleme, klavye kısayolları.

### 2. 🔍 Tek Değişkenli Kök Bulucu (Root Finder)
* **Newton-Raphson Yöntemi:** $f(x) / f'(x)$ teğet tabanlı hızlı yakınsama.
* **Bisection (İkiye Bölme):** Garantili yakınsama ve uç nokta kök tespiti ($f(a)=0$ veya $f(b)=0$).
* **Secant (Kiriş):** İki başlangıç noktası üzerinden türevsiz yaklaşım.
* **Regula Falsi (Illinois Hızlandırmalı):** Dışbükey fonksiyonlardaki durgunluk (stagnation) sorununu çözen Illinois algoritması.
* **Dinamik Değişken Desteği:** $x, t, y$ veya istenen herhangi bir değişken adıyla kök bulma.

### 3. 📉 Sayısal & Kısmi Türev Hesaplayıcı
* **Farklı Türev Formülleri:** Merkezi sonlu farklar ($O(h^4)$ ve $O(h^6)$ hassasiyet), İleri Binom ve Geri Sonlu Farklar.
* **1. ile 12. Derece Arası Türev:** Yüksek mertebeli türevler için optimize edilmiş adım büyüklüğü ($h$) ve koşullandırma.
* **Çok Değişkenli Kısmi Türev:** İstenen değişkene göre kısmi türev alma ($f(x, y, z)$).

### 4. 📜 Taylor Serisi Açılımı (Tek Değişkenli)
* İstenen merkez noktası ($a$) etrafında $n$. dereceden sembolik Taylor polinom açılımı.
* Noktadaki türevler ve terim katsayıları tablosu.

### 5. 🌐 Çok Değişkenli Taylor Serisi
* $f(x, y)$, $f(x, y, z)$ fonksiyonlarının $(x_0, y_0, \dots)$ noktası etrafında çok değişkenli polinom yaklaşımı.
* Sayısal türev kalıntılarını filtreleyen rasyonel katsayı yakalama algoritması.

### 6. ⚖️ Doğrusal & Doğrusal Olmayan Denklem Sistemleri
* **LU Ayrışımı (A = P·L·U):** Doğrusal sistemler için doğrudan en hızlı çözücü.
* **Ters Matris ($X = A^{-1} \cdot b$):** Analitik ters matris çarpımı.
* **QR Ayrışımı ($Q \cdot R \cdot X = b$):** Householder ortogonal dönüşümü.
* **Gauss-Seidel İterasyonu:** Köşegen baskınlık denetimli ardışık iteratif çözücü.
* **Çok Boyutlu Newton-Raphson:** Doğrusal olmayan (polinom, trigonometrik, üstel vb.) karmaşık sistem çözücü.
* **Doğrusallık Denetimi:** Lineer çözücüye non-lineer denklem verildiğinde otomatik uyarı.

### 7. 🧮 Gelişmiş Matris Hesaplayıcı & 5 Temel Ayrışım
* **5 Temel Dekompozisyon:**
  * **LU Ayrışımı:** $L \cdot U = P \cdot A$
  * **QR Ayrışımı:** $Q \cdot R = A$
  * **Cholesky Ayrışımı:** $L \cdot L^T = A$ (Pozitif tanımlı denetimi)
  * **Özdeğer & Özvektör:** $\det(A - \lambda I) = 0$
  * **Tekil Değer Ayrışımı (SVD):** Kare ($n \times n$) ve Dikdörtgen ($m \times n$, $m < n$ ve $m > n$) tam destek ($U \cdot S \cdot V^T = A$).
* **Matris Aritmetiği:** Toplama, çıkarma, matris çarpımı, eleman bazlı çarpma/bölme, skaler çarpım.
* **Matris Analizi:** Determinant, İz (Trace), Rank, 1-Norm, 2-Norm, Sonsuz-Norm, Frobenius Normu, Kondisyon Sayısı.
* **Matris Denklem Çözümü:** $A \cdot X = B$ ve $A^T \cdot X = B$.

### 8. 🎲 Kombinatorik, Polar Dönüşüm ve Özel Fonksiyonlar
* **Taşmasız Kombinatorik:** $C(n, k)$ ve $P(n, k)$ çarpımsal sadeleştirme ile $52!$ gibi $10^{67}$ değerlerine çıkmadan $C(52, 5) = 2.598.960$ hesaplar.
* **Pascal Üçgeni:** $n$. dereceye kadar binom katsayıları tablosu.
* **Karakter Kombinasyon Üreteci:** Belirtilen alfabeden istenen boyutta tüm dizilimleri türetme.
* **Kartezyen $\leftrightarrow$ Polar Dönüşüm:** Hipotenüs ve açı hesabı ($r, \theta$ rad ve derece).

### 9. 📊 Dinamik Fonksiyon & Grafik Çizici
* **Asimptot & Süreksizlik Algılama:** $\tan(x)$ ve $\frac{1}{x^2-1}$ gibi fonksiyonlarda dikey çizgi hataları önleyen eğri parçalama.
* **İnteraktif Tuval:** Fare ile eksen kaydırma (Pan), tekerlek ile yakınlaştırma (Zoom), tıklama ile nokta işaretleme.
* **Otomatik Eksen Ölçekleme (Auto-fit):** Fonksiyon genliğine göre Y eksenini otomatik sığdırma.

---

## 🛡️ Güvenilirlik & Sistem Kararlılığı

* **Tek Kopya Çalışma (Single Instance Mutex):** Uygulama arka planda zaten açıkken ikinci kez açıldığında var olan pencere öne getirilir ve odaklanır.
* **Temiz Süreç Sonlandırma (Zero Background Leakage):** Pencere kapatıldığında arka planda hiçbir süreç veya iş parçacığı asılı kalmaz (`Environment.Exit(0)`).
* **Test Doğrulaması:** 1.169 adet birim ve uçtan uca otomatik testin tamamı %100 başarıyla geçmektedir.

---

## 🛠️ Kaynak Koddan Derleme

```bash
# Depoyu klonlayın
git clone https://github.com/LilDosto/ttCalculator.git
cd ttCalculator

# Projeyi derleyin
dotnet build MathApp/MathApp/MathApp.csproj -c Release

# Tek parça .exe olarak yayımlayın
dotnet publish MathApp/MathApp/MathApp.csproj -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -o "Release"
```

---

## 📄 Lisans
Bu proje [MIT Lisansı](LICENSE) altında lisanslanmıştır.
