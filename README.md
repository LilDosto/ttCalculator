# 📐 ttCalculator v3.6.2

[![GitHub release](https://img.shields.io/github/v/release/LilDosto/ttCalculator?color=brightgreen&label=S%C3%BCr%C3%BCm)](https://github.com/LilDosto/ttCalculator/releases)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8.0--windows-purple.svg)](https://dotnet.microsoft.com/)
[![Platform](https://img.shields.io/badge/Platform-Windows%20x64-lightgrey.svg)](https://microsoft.com)

**ttCalculator**, 128-bit yüksek hassasiyetli matematik motoru, diferansiyel denklem çözücüleri (ODE & PDE), gelişmiş nümerik yöntemler, çoklu fonksiyon grafik stüdyosu, matris dekompozisyonları ve çok değişkenli analizler gerçekleştirebilen modern ve profesyonel bir masaüstü matematik yazılımıdır.

Tüm matematiksel çekirdek, otomatik test paketinden **%100 başarıyla (0 hata, 0 uyarı)** geçirilmiş ve son kullanıcıya hazır hale getirilmiştir.

---

## 🚀 Hızlı Başlangıç & Kurulum

1. **[Releases](https://github.com/LilDosto/ttCalculator/releases/tag/v3.6.2)** sayfasına gidin.
2. İhtiyacınıza göre indirin:
   - **`ttCalculator-v3.6.2-Setup.exe` (Önerilen)**: Standart Windows kurulum sihirbazı. Masaüstüne ve Başlat Menüsüne kısayol oluşturur, Denetim Masası / Program Ekle-Kaldır entegrasyonu sağlar.
   - **`ttCalculator.exe`**: Kurulumsuz doğrudan çalıştırılabilen tek dosya taşınabilir (portable) sürüm.
   - **`ttCalculator-v3.6.2-win-x64.zip`**: Taşınabilir arşiv paketi.
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

### 2. ∫ Diferansiyel Denklem Çözücü (1 Değişkenli ODE - v3.6)
* **Desteklenen Algoritmalar:** Euler (1. Derece), Heun (RK2 Predictor-Corrector), Runge-Kutta 4 (RK4) ve 8. Derece Yüksek Hassasiyetli Runge-Kutta (RK8).
* **İnteraktif Çözüm Eğrisi Tuvali:** $y(x)$ çözüm eğrisini çizdiren, farenin üzerinde gezdiği koordinatları (X, Y) takip eden artı imleci (crosshair) ve koordinat ipucu (tooltip).
* **Görsel Durum & KPI Kartı:** $y(x_{son})$ nihai değer kartı, adım sayısı ($N$), adım büyüklüğü ($h$), yürütme süresi (ms) ve kullanılan yöntem bilgisi.
* **Adım Adım Çözüm Tablosu:** Zebra desenli `DataGridView` tablosu ($i, x_i, y_i, \Delta y$), TSV formatında panoya kopyalama ve yüksek çözünürlüklü PNG grafik dışa aktarma.

### 3. ∭ Diferansiyel Denklem Sistemi Çözücü (System of ODEs - v3.6)
* **Vektörel Diferansiyel Sistemler:** $\frac{d\vec{Y}}{dt} = \vec{F}(t, \vec{Y})$ ($n \in [1, 6]$ dinamik değişken desteği).
* **Hazır Modeller:** Harmonik Osilatör, Av-Avcı (Lotka-Volterra), Sönümlü Sarkaç, Lorenz Kaotik Çekicisi.
* **Faz Portresi Tuvali (Phase Portrait):** Seçilen iki eksen ($y_1$ vs $y_2$) arasında yörünge eğrisi, hareket yönünü gösteren vektör okları, renk geçişli degrade çizgi ve fareyle koordinat inceleme.
* **Zaman Serisi Tuvali (Time Series):** $t$ parametresine bağlı $y_1(t), y_2(t), \dots$ eğrilerinin renk kodlu çizimi ve grafik lejantı.
* **Sistem Veri Tablosu:** Tüm adımları listeleyen DataGridView tablosu ve CSV/PNG dışa aktarım desteği.

### 4. 🌊 Kısmi Diferansiyel Denklem Çözücü (1D PDE Solver - v3.6)
* **Desteklenen Modeller:** 1B Isı Denklemi (FTCS ve Crank-Nicolson), 1B Dalga Denklemi (Leapfrog), 1B Taşınım / Advection (Upwind).
* **Dinamik CFL Kararlılık Analiz Rozeti:** $r = \alpha \frac{\Delta t}{\Delta x^2}$ veya $CFL = c \frac{\Delta t}{\Delta x}$ değerlerini canlı hesaplayan, kararlı/kararsız durumu bildiren renkli rozet.
* **2B Uzay-Zaman Isı Haritası (Heatmap):** $x \in [0, L]$ (X ekseni) ve $t \in [0, T]$ (Y ekseni) üzerinde renk gradyanı (Turbo/Thermal colormap) ile sıcaklık/dalga yoğunluğu dağılımı.
* **Canlı Zaman Animasyonu:** 30 FPS hızında dalga/ısı profilinin zamana bağlı salınımını canlandıran oynat/durdur denetleyicisi, hız çarpanı ($1\times, 2\times, 5\times$) ve interaktif zaman kaydırıcısı (time scrubber).
* **Zaman Dilimleri Tablosu:** $t = 0, T/4, T/2, 3T/4, T$ anlarındaki uzaysal profili özetleyen tablo.

### 5. 🌀 Kısmi Diferansiyel Denklem Sistemi Çözücü (Coupled PDE System - v3.6)
* **1B Bağlaşımlı Reaksiyon-Difüzyon Sistemleri:**
  $$u_t = D_u \cdot u_{xx} + F(u, v)$$
  $$v_t = D_v \cdot v_{xx} + G(u, v)$$
* **Hazır Biyokimyasal & Fiziksel Modeller:** Turing Desenleri (Gray-Scott), Schnakenberg Sistemi, FitzHugh-Nagumo Sinir Modeli, Çift Difüzyon.
* **Çift Konsantrasyon Canlı Dalga Çizimi:** $u(x)$ (gök mavisi) ve $v(x)$ (mercan kırmızısı) yarı saydam alan dolgulu konsantrasyon eğrileri ve canlı animasyon.
* **2B Turing Deseni Haritası & Faz Uzayı Trajektorisi:** Uzay-zaman morfolojik desen haritası ve $(u, v)$ durum faz portresi.

### 6. 🔍 Denklem Kökü Bulucu (Root Finder v3.4)
* **Dinamik Parametre Girişi:** Seçilen yönteme göre (Newton-Raphson, Bisection, Secant, Regula Falsi, Çoklu Kök Taraması) dinamik parametre alanları.
* **Vurgulu KPI Kartı & Durum Rozetleri:** Büyük tipografiyle hesaplanan kök $x^*$, adım sayısı, son hata ve yakınsama durumu.
* **İterasyon Adımları Tablosu & Yakınsama Hızı Grafiği:** DataGridView tablosu (zebra striping) ve $\log_{10}(\epsilon)$ hata düşüşünü gösteren canlı 2B yakınsama grafiği.
* **Grafik Stüdyosu Entegrasyonu:** `[📈 Kökü Grafikte Göster]` butonu ile kök noktasını ve fonksiyonu anında Grafik Çizici sekmesine aktarma.
* **Nümerik Koruma:** $f'(x) \approx 0$ sıfıra bölme ve ıraksama (divergence) denetimleri.

### 7. 📜 Taylor Serisi Hesaplayıcı (Tek Değişkenli - v3.5)
* **Gelişmiş Formül & Merkez Girişi:** $f(x)$ girdi kartı, Maclaurin ($a=0$) hızlı butonu, $n \in [1, 20]$ derece seçimi ve hızlı fonksiyon şablonları ($\sin, \cos, e^x, \ln(1+x), \frac{1}{1-x}, \arctan$).
* **Vurgulu Polinom Kartı:** Büyük matematiksel $P_n(x)$ gösterimi, `[📈 Grafikte Göster]`, `[📋 Polinomu Kopyala]`, `[LaTeX Kopyala]` entegrasyonu.
* **Mini 2D Yakınsama Önizleme Tuvali:** Gerçek fonksiyon $f(x)$ ile Taylor yaklaşımı $P_n(x)$'in merkez $a$ etrafındaki yakınsamasını gerçek zamanlı çizen çift tamponlu 2B kanvas.
* **3 Sekmeli Analitik Sonuçlar:**
  1. *Katsayılar & Türev Tablosu (DataGridView):* Her $k$ mertebesi için $f^{(k)}(x)$, $f^{(k)}(a)$, $k!$, kesirli ve ondalıklı $c_k$ katsayıları ve terimler.
  2. *Hata Analizi & Lagrange Kalanı ($R_n(x)$):* Teorik formül ve test noktalarında ($a \pm 0.1, a \pm 0.5, a \pm 1, a \pm 2$) mutlak ve bağıl hata analizi tablosu.
  3. *Detaylı Rapor:* Sadeleştirilmiş kesirli, faktöriyelli ve ondalıklı tam açılımlar.

### 8. 🌐 Çok Değişkenli Taylor Serisi & Teğet Düzlem Stüdyosu (v3.5)
* **Kompakt Etiket/Chip Mimarisi:** $f(x_1, \dots, x_k)$ için dinamik değişken ve merkez noktası chip kartları (`[Var: x | x₀: 0 | ✕]`), `[+ Değişken Ekle]`, `[Tümü = 0]`.
* **Katmanlı Analiz Blokları:**
  - *Blok 1 (Doğrusal Yaklaşım $n=1$):* Teğet düzlem denklemi $L(x, y)$, $\nabla f(a)$ gradyan vektörü ve `[📋 Teğet Düzlemini Kopyala]`.
  - *Blok 2 (Kuadratik Yaklaşım $n=2$):* Hessian matrisi $H(a)$, ikinci derece terimler $Q(x, y)$ ve 2 değişkenli ekstremum teşhisi ($\det(H) > 0 \implies$ min/max, $\det(H) < 0 \implies$ eyer noktası).
  - *Blok 3 (Tam Polinom):* $T_n$ kesirli, faktöriyelli ve ondalıklı tam çok değişkenli açılım.
* **Kısmi & Çapraz Türevler Tablosu (DataGridView):** Çoklu indeks $|\alpha|$, diferansiyel operatörleri, sayısal değerler, $\alpha!$ paydası ve katsayılar.

### 9. 🎲 Kombinatorik, Polar Dönüşüm ve Özel Fonksiyonlar (v3.4)
* **4 Kartlı Modern Mimari:** Ayrık Matematik ($n!, P, C$), Asenkron Dizi Üreteci (güvenlik koruması ve sayfalamalı gösterim), Vektörel Polar Dönüşüm (canlı vektör açısı ve büyüklük diyagramı) ve İleri Analitik Fonksiyonlar ($\sinh, \cosh, \tanh, \log_b$).
* **Zengin Sekmeli Çıktı Alanı:** Özet Kartı, Pascal Üçgeni Piramidi, Sayfalamalı Kombinasyon Listesi, `[JSON İndir]` ve `[Kopyala]` araçları.

### 10. 📐 Nümerik Türev, Kısmi Türev & Hessian Stüdyosu (v3.4)
* **Nümerik Ayar Çubuğu:** $h$ adım boyutu hızlı seçimleri (`1e-3`, `1e-5`, `1e-7`), Merkezi/İleri/Geri fark formülleri, $O(h^2), O(h^4), O(h^6)$ doğruluk derecesi.
* **Tek Değişkenli Türev & Teğet Çizici:** $f^{(n)}(x_0)$ hesabı, teğet ve normal doğru denklemleri, `[📈 Teğeti Grafikte Çiz]` entegrasyonu.
* **Çok Değişkenli Analiz, Gradyan & Hessian:** Dinamik değişken ekleme, $\nabla f$ gradyan vektörü, birim $\vec{u}$ vektörü ile yönlü türev ($D_{\vec{u}} f$) ve 2. mertebe Hessian simetrik matrisi ile yerel ekstremum (min/max/eyer) testi.

### 11. 🧮 Matris Cebiri & Ayrıştırma Stüdyosu (v3.5)
* **Dinamik Mini Tablolar (DataGridView):** A ve B matrisleri için otomatik sütun genişlemeli, klavye gezintili ve Excel / CSV panosundan doğrudan yapıştırma (`Ctrl+V`) ve kopyalama (`Ctrl+C`) destekli gridler.
* **Hızlı Matris Araçları:** `[Birim (I)]`, `[Sıfır]`, `[Rastgele]`, `[SPD Üret]` ($M = R^T R + n I$ simetrik pozitif tanımlı matris oluşturucu), `[A ↔ B Takas]`, `[A → B Kopyala]`.
* **Kategorize İşlem Sekmeleri:**
  1. *Tekil Analiz (Matrix A):* Tam Analiz, $A^{-1}$ (Ters Matris), $A^T$ (Transpoz), $-A$, İz & Determinant, 1/2/$\infty$/Frobenius Normları, Kondisyon Sayısı $\kappa(A)$ ve Rank.
  2. *Ayrıştırmalar (Decompositions):* LU ($P\cdot A = L\cdot U$), QR (Householder yansımaları), Cholesky ($A = L\cdot L^T$, otomatik SPD doğrulaması), SVD ($A = U\cdot\Sigma\cdot V^T$), Özdeğerler ve Özvektörler ($\lambda, V$).
  3. *İki Matrisli Cebir & Sistem Çözücü:* $A + B$, $A - B$, $A \times B$, $k \cdot A$, $A \odot B$ (Hadamard), $A \oslash B$ (Noktasal Böl), Lineer Sistem Çözücü ($A\cdot X = B$), Transpoz Denklem Çözücü ($A^T\cdot X^T = B^T$).
* **Üst KPI Kartları & Rapor:** $\det(A)$, $\text{rank}(A)$ ve $\kappa(A)$ durum rozetleri, `[📥 Ans → A]`, `[📥 Ans → B]`, `[LaTeX Kopyala]` (bmatrix) ve `[📋 Kopyala]`.
* **Nümerik Koruma:** Tekil matris ($|\det| < 10^{-12}$) uyarısı ve non-SPD Cholesky uyarıları.

### 12. 🔢 Basit & Bilimsel Hesap Makinesi
* 128-bit `decimal` motoru, RAD/DEG modları, otomatik parantez tamamlama ve geçmiş paneli.

### 13. ⚖️ Doğrusal & Doğrusal Olmayan Denklem Sistemleri
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
