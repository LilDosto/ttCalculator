# 📐 ttCalculator v2.5.0

[![GitHub release](https://img.shields.io/github/v/release/LilDosto/ttCalculator?color=brightgreen&label=S%C3%BCr%C3%BCm)](https://github.com/LilDosto/ttCalculator/releases)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8.0--windows-purple.svg)](https://dotnet.microsoft.com/)
[![Platform](https://img.shields.io/badge/Platform-Windows%20x64-lightgrey.svg)](https://microsoft.com)

**ttCalculator**, 128-bit yüksek hassasiyetli matematik motoru, diferansiyel denklem çözücüleri (ODE & PDE), gelişmiş nümerik yöntemler, çoklu fonksiyon grafik stüdyosu, matris dekompozisyonları ve çok değişkenli analizler gerçekleştirebilen modern ve profesyonel bir masaüstü matematik yazılımıdır.

Tüm matematiksel çekirdek, otomatik test paketinden **%100 başarıyla (0 hata, 0 uyarı)** geçirilmiş ve son kullanıcıya hazır hale getirilmiştir.

---

## 🚀 Hızlı Başlangıç (Kurulumsuz Doğrudan Çalıştırma)

Uygulamanın hazır derlenmiş Windows sürümünü (`ttCalculator.exe`) hemen çalıştırmak için:

1. **[Releases](https://github.com/LilDosto/ttCalculator/releases/tag/v2.5.0)** sayfasına gidin.
2. **`ttCalculator.exe`** (veya `ttCalculator-v2.5.0-win-x64.zip`) dosyasını indirin.
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

### 1. 📈 Gelişmiş Çoklu Fonksiyon & Geometri Grafiği Çizici (v2.3)
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

### 6. 🔍 Çoklu Kök Taramalı Kök Bulucu (Root Finder)
* **🎯 Otomatik Çoklu Kök Taraması:** Verilen $[a, b]$ aralığındaki tüm tekil ve çoklu kökleri ardışık/paralel nümerik tarama ile bularak temiz bir kök tablosunda sunar.
* **Klasik Nümerik Yöntemler:** Newton-Raphson, Bisection (İkiye Bölme), Secant (Kiriş), Regula Falsi (Illinois hızlandırmalı).

### 7. 📜 Taylor Serisi Açılımı (Kesirli & Faktöriyelli Form)
* Tek ve çok değişkenli Taylor açılımlarında katsayılar hem ondalıklı, hem **sadeleştirilmiş kesirli formda**, hem de formülasyona tam uygun **analitik faktöriyelli formda** ($\frac{f^{(k)}(a)}{k!}$) gösterilir.

### 8. 🎲 Kombinatorik, Polar Dönüşüm ve Özel Fonksiyonlar
* Düzeltilmiş Taban ($b$) arayüz kutusu, taşmasız $C(n, k)$ ve $P(n, k)$ kombinatorik motoru, Pascal üçgeni ve polar/kartezyen dönüşümler.

### 9. 🔢 Basit & Bilimsel Hesap Makinesi
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
