# 📐 ttCalculator v3.4.0

[![GitHub release](https://img.shields.io/github/v/release/LilDosto/ttCalculator?color=brightgreen&label=S%C3%BCr%C3%BCm)](https://github.com/LilDosto/ttCalculator/releases)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8.0--windows-purple.svg)](https://dotnet.microsoft.com/)
[![Platform](https://img.shields.io/badge/Platform-Windows%20x64-lightgrey.svg)](https://microsoft.com)

**ttCalculator**, 128-bit yüksek hassasiyetli matematik motoru, diferansiyel denklem çözücüleri (ODE & PDE), gelişmiş nümerik yöntemler, çoklu fonksiyon grafik stüdyosu, matris dekompozisyonları ve çok değişkenli analizler gerçekleştirebilen modern ve profesyonel bir masaüstü matematik yazılımıdır.

Tüm matematiksel çekirdek, otomatik test paketinden **%100 başarıyla (0 hata, 0 uyarı)** geçirilmiş ve son kullanıcıya hazır hale getirilmiştir.

---

## 🚀 Hızlı Başlangıç & Kurulum

1. **[Releases](https://github.com/LilDosto/ttCalculator/releases/tag/v3.4.0)** sayfasına gidin.
2. İhtiyacınıza göre indirin:
   - **`ttCalculator-v3.4.0-Setup.exe` (Önerilen)**: Standart Windows kurulum sihirbazı. Masaüstüne ve Başlat Menüsüne kısayol oluşturur, Denetim Masası / Program Ekle-Kaldır entegrasyonu sağlar.
   - **`ttCalculator.exe`**: Kurulumsuz doğrudan çalıştırılabilen tek dosya taşınabilir (portable) sürüm.
   - **`ttCalculator-v3.4.0-win-x64.zip`**: Taşınabilir arşiv paketi.
3. Çift tıklayarak doğrudan çalıştırın — ek bağımlılık gerektirmez!

---

## 📁 Proje Mimarisi

Proje, modern ve modüler bir .NET 8 WinForms çözüm mimarisine sahiptir:

```
ttCalculator/
├── ttCalculator.sln             # Ana Visual Studio / .NET Çözüm Dosyası
├── src/
│   └── ttCalculator/            # Masaüstü Uygulaması (WinForms / .NET 8)
│       ├── Controls/            # 13 Temel Arayüz Modülü ve Kullanıcı Kontrolleri
│       │   ├── SimpleCalculatorControl.cs
│       │   ├── GraphPlotterControl.cs       # Gelişmiş Çoklu Eğri & Geometri Çizici
│       │   ├── CombinatoricsControl.cs
│       │   ├── RootFinderControl.cs         # Çoklu Kök Taramalı Kök Bulucu
│       │   ├── SystemSolverControl.cs
│       │   ├── DerivativeCalculatorControl.cs
│       │   ├── MatrixCalculatorControl.cs
│       │   ├── TaylorSeriesControl.cs       # Kesirli & Faktöriyelli Taylor
│       │   ├── MultivariableTaylorControl.cs
│       │   ├── OdeSolverControl.cs          # 1D ODE Çözücü (Euler, RK4, RK8)
│       │   ├── OdeSystemSolverControl.cs    # ODE Sistemi Çözücü
│       │   ├── PdeSolverControl.cs          # 1D Kısmi Diferansiyel (Isı/Dalga/Taşınım)
│       │   └── PdeSystemSolverControl.cs    # PDE Sistemi (Reaksiyon-Difüzyon)
│       ├── Core/                # 128-bit Matematik, ODE/PDE ve Nümerik Yöntem Çekirdeği
│       │   ├── DifferentialEquations.cs     # Euler, Heun, RK4, RK8, FTCS, Leapfrog, Upwind
│       │   ├── NumericalMethods.cs          # Çoklu Kök Tarama, Kesirli Açılım, Türev
│       │   ├── Function.cs                  # Hızlı İfade Derleyici
│       │   ├── DecimalMath.cs               # 128-bit Yüksek Hassasiyetli Trigonometri/Logaritma
│       │   └── Matrix/                      # LU, QR, Cholesky, Eigenvalue, SVD
│       ├── MainForm.cs          # Ana Pencere ve Akışkan Gezinme Yönetimi
│       ├── ThemeHelper.cs       # Modern Koyu Tema ve Çift Tamponlama
│       ├── Program.cs           # Tekil Örnek (Single-Instance Mutex) Giriş Noktası
│       └── ttCalculator.csproj  # Uygulama Proje Yapılandırması
├── tests/
│   └── ttCalculator.Tests/      # Otomatik Doğrulama Testleri
│       └── Program.cs
├── Release/                     # Taşınabilir Tek Dosya Dağıtımları
├── LICENSE                      # MIT Lisansı
└── README.md
```

---

## ✨ Temel Özellikler & Modüller

Uygulama modern koyu tema (Dark Theme) arayüzü ile 13 ana modülden oluşmaktadır:

### 1. 📈 Gelişmiş Çoklu Fonksiyon & Geometri Grafiği Çizici (v3.3)
* **GeoGebra / Desmos Kalitesinde Manyetik Mıknatıslanma (Multi-tier Magnetic Snapping):** Tuval üzerinde nokta eklerken veya imleç gezdirirken 4 kademeli akıllı yakalama motoru:
  - **1. Kademe:** Kritik Noktalar (Kökler, Yerel Maks/Min, Büküm, Fonksiyon Kesişimleri - $\le 14$ px tolerans).
  - **2. Kademe:** Eğriye Yapışma (Snap-to-Curve $f(x)$ veya düşey doğrular - $\le 12$ px tolerans, Glider/Bağlı nokta entegrasyonu).
  - **3. Kademe:** Eksen ve Orijin Mıknatısı (Orijin $(0, 0)$ ve $x=0$, $y=0$ eksen çizgileri - $\le 8$ px tolerans).
  - **4. Kademe:** Dinamik Izgara Kesişimi (Grid Snap - `Ctrl` tuşu ile devre dışı bırakılabilir serbest mod).
* **Hayalet Halka & Koordinat Rozeti (Ghost Ring Indicator):** İmleç tuvalde gezinirken hedef noktayı canlı olarak yarı-saydam konsantrik halka ve renkli tip etiketi (`[Kök]`, `[Eğri]`, `[Orijin]`, `[Izgara]`) ile anlık gösterir.
* **Tıklama Düşmelerine Karşı Kararlı Olay Yönetimi (Zero Dropped Clicks):** Fare tıklamalarındaki mikroskobik titremeleri filtreleyen eşik denetimi ile tıklamalar asla sürükleme olarak algılanıp yutulmaz.
* **Katman & Nokta Yönetim Kartları:** Eklenen tüm serbest ve bağlı noktalar sağ panelde renk, şekil ve tip rozetleri (`[Kök]`, `[Eğri]`, `[Orijin]`) ile listelenir, tek tıkla veya sağ tıkla silinebilir.
* **Çoklu Fonksiyon Desteği:** Aynı tuval üzerinde sınırsız sayıda fonksiyon çizimi; sağ panelden renk, çizgi tipi (Düz, Kesikli, Noktalı) ve kalınlık (1-4 px) değiştirme, açma/kapama ve silme.
* **Aralık Kısıtlama vs Tüm Ekran:** `Min X` ve `Max X` aralığına kısıtlı çizim veya $-\infty$ ile $+\infty$ arasında tam ekran çizim seçeneği.
* **Bağımsız Eksen Ölçekleme:** X eksenini Y'ye dokunmadan 10 kat veya serbestçe sündürme/daraltma (`Shift + Tekerlek` ile X sündürme, `Ctrl + Tekerlek` ile Y sündürme).
* **Tekil Nokta Yönetimi:** Tıklayarak veya koordinat girerek nokta ekleme; renk, boyut ve şekil (Daire, Kare, Çarpı, Elmas) seçimi; **nokta üzerine sağ tıklayarak veya listeden tekil silme**.
* **Ortak Teğet Doğrusu Çizici:** İki fonksiyon seçildiğinde aralarındaki ortak teğet doğrularını, değme noktalarını ve $y = m\cdot x + c$ doğru denklemini hesaplayıp grafiğe ekleme.
* **En Yakın Noktalar & Doğru:** Kesişmeyen iki fonksiyonun aralarındaki minimum mesafeyi veren en yakın noktaları ve bu noktalardan geçen dik doğru denklemini hesaplama.
* **Noktadan Teğet Çizici:** Dışarıdaki bir noktadan seçilen fonksiyona çizilen teğet doğru denklemi ve grafiği.
* **Hazır Parametrik Şekiller:** Daire, Kare, Dikdörtgen, Eşkenar Üçgen, Elips ve Düzgün Altıgen şekillerini parametrik olarak fonksiyon listesine ekleyip çizdirme.

### 2. ∫ Diferansiyel Denklem Çözücü (1 Değişkenli ODE)
* **Desteklenen Algoritmalar:** Euler (1. Derece), Heun (RK2 Predictor-Corrector), Runge-Kutta 4 (RK4) ve 8. Derece Yüksek Hassasiyetli Runge-Kutta (RK8).
* Başlangıç değer problemi ($y' = f(x, y)$, $y(x_0) = y_0$) için adım büyüklüğü, eğimler ve adım adım çözüm tablosu.

### 3. ∭ Diferansiyel Denklem Sistemi Çözücü (System of ODEs)
* Vektörel diferansiyel denklem sistemleri: $\frac{d\vec{Y}}{dt} = \vec{F}(t, \vec{Y})$.
* **Yüksek Dereceli ODE Dönüşümü:** $n$. derece diferansiyel denklemleri durum-uzayı (state-space) vektör sistemine dönüştürerek çözebilme.
* **Hazır Modeller:** Harmonik Osilatör, Av-Avcı (Lotka-Volterra), Sönümlü Sarkaç, Lorenz Kaotik Çekicisi.
* Vektörel Euler, RK4 ve RK8 çözücüleri ile zaman adımları tablosu.

### 4. 🌊 Kısmi Diferansiyel Denklem Çözücü (1D PDE)
* **1 Boyutlu Isı Denklemi:** $u_t = \alpha \cdot u_{xx}$ (FTCS sonlu farklar yöntemi, kararlılık koşulu $r \le 0.5$ denetimi).
* **1 Boyutlu Dalga Denklemi:** $u_{tt} = c^2 \cdot u_{xx}$ (Leapfrog merkezi farklar, Courant-Friedrichs-Lewy / CFL denetimi).
* **1 Boyutlu Taşınım (Advection) Denklemi:** $u_t + v \cdot u_x = 0$ (Upwind stabil şeması).
* Farklı zaman dilimlerinde ($t=0, T/4, T/2, 3T/4, T$) mekan profili tablosu.

### 5. 🌀 Kısmi Diferansiyel Denklem Sistemi Çözücü (Coupled PDE System)
* 1 Boyutlu Bağlaşımlı Reaksiyon-Difüzyon Sistemleri:
  $$u_t = D_u \cdot u_{xx} + F(u, v)$$
  $$v_t = D_v \cdot v_{xx} + G(u, v)$$
* Sıfır-akı (Neumann) sınır koşulları, Turing deseni oluşumu (Gray-Scott, Schnakenberg, FitzHugh-Nagumo modelleri).

### 6. 🔍 Denklem Kökü Bulucu (Root Finder v3.4)
* **Dinamik Parametre Girişi:** Seçilen yönteme göre (Newton-Raphson, Bisection, Secant, Regula Falsi, Çoklu Kök Taraması) dinamik parametre alanları.
* **Vurgulu KPI Kartı & Durum Rozetleri:** Büyük tipografiyle hesaplanan kök $x^*$, adım sayısı, son hata ve yakınsama durumu.
* **İterasyon Adımları Tablosu & Yakınsama Hızı Grafiği:** DataGridView tablosu (zebra striping) ve $\log_{10}(\epsilon)$ hata düşüşünü gösteren canlı 2B yakınsama grafiği.
* **Grafik Stüdyosu Entegrasyonu:** `[📈 Kökü Grafikte Göster]` butonu ile kök noktasını ve fonksiyonu anında Grafik Çizici sekmesine aktarma.
* **Nümerik Koruma:** $f'(x) \approx 0$ sıfıra bölme ve ıraksama (divergence) denetimleri.

### 7. 📜 Taylor Serisi Açılımı (Kesirli & Faktöriyelli Form)
* Tek ve çok değişkenli Taylor açılımlarında katsayılar hem ondalıklı, hem **sadeleştirilmiş kesirli formda**, hem de formülasyona tam uygun **analitik faktöriyelli formda** ($\frac{f^{(k)}(a)}{k!}$) gösterilir.

### 8. 🎲 Kombinatorik, Polar Dönüşüm ve Özel Fonksiyonlar (v3.4)
* **4 Kartlı Modern Mimari:** Ayrık Matematik ($n!, P, C$), Asenkron Dizi Üreteci (güvenlik koruması ve sayfalamalı gösterim), Vektörel Polar Dönüşüm (canlı vektör açısı ve büyüklük diyagramı) ve İleri Analitik Fonksiyonlar ($\sinh, \cosh, \tanh, \log_b$).
* **Zengin Sekmeli Çıktı Alanı:** Özet Kartı, Pascal Üçgeni Piramidi, Sayfalamalı Kombinasyon Listesi, `[JSON İndir]` ve `[Kopyala]` araçları.

### 9. 📐 Nümerik Türev, Kısmi Türev & Hessian Stüdyosu (v3.4)
* **Nümerik Ayar Çubuğu:** $h$ adım boyutu hızlı seçimleri (`1e-3`, `1e-5`, `1e-7`), Merkezi/İleri/Geri fark formülleri, $O(h^2), O(h^4), O(h^6)$ doğruluk derecesi.
* **Tek Değişkenli Türev & Teğet Çizici:** $f^{(n)}(x_0)$ hesabı, teğet ve normal doğru denklemleri, `[📈 Teğeti Grafikte Çiz]` entegrasyonu.
* **Çok Değişkenli Analiz, Gradyan & Hessian:** Dinamik değişken ekleme, $\nabla f$ gradyan vektörü, birim $\vec{u}$ vektörü ile yönlü türev ($D_{\vec{u}} f$) ve 2. mertebe Hessian simetrik matrisi ile yerel ekstremum (min/max/eyer) testi.

### 10. 🔢 Basit & Bilimsel Hesap Makinesi
* 128-bit `decimal` motoru, RAD/DEG modları, otomatik parantez tamamlama ve geçmiş paneli.

### 10. ⚖️ Doğrusal & Doğrusal Olmayan Denklem Sistemleri
* LU, QR, Ters Matris, Gauss-Seidel ve Çok Boyutlu Newton-Raphson çözücüleri.

### 11. 🧮 Gelişmiş Matris Hesaplayıcı & 5 Temel Ayrışım
* LU, QR, Cholesky, Özdeğer/Özvektör ve SVD (Tekil Değer Ayrışımı) dekompozisyonları.

---

## 🛠️ Kaynak Koddan Derleme & Yayımlama

```bash
# Depoyu klonlayın
git clone https://github.com/LilDosto/ttCalculator.git
cd ttCalculator

# Çözümü derleyin
dotnet build ttCalculator.sln -c Release

# Doğrulama testlerini çalıştırın
dotnet run --project tests/ttCalculator.Tests/ttCalculator.Tests.csproj

# Tek dosya .exe olarak yayımlayın
dotnet publish src/ttCalculator/ttCalculator.csproj -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -o "Release"
```

---

## 📄 Lisans
Bu proje [MIT Lisansı](LICENSE) altında lisanslanmıştır.
