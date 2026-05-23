# 📐 ttCalculator

[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![Unity Version](https://img.shields.io/badge/Unity-2021.3+-brightgreen.svg)](https://unity.com/)
[![Platform](https://img.shields.io/badge/Platform-Windows-lightgrey.svg)](https://microsoft.com)

**ttCalculator**, gelişmiş matematiksel analizler, nümerik yöntemler, matris dekompozisyonları, grafik çizimleri ve çok değişkenli hesaplamalar gerçekleştirebilen son derece güçlü ve premium bir matematik uygulamasıdır. Proje hem bir **Unity** görsel arayüzü hem de **Windows Forms (MathApp)** masaüstü arayüzü barındırmaktadır.

---

## ✨ Özellikler

### 1. 🧮 Gelişmiş Matris Hesaplamaları (Matrix Decompositions)
* **LU Ayrımsaması (LU Decomposition)**
* **QR Ayrımsaması (QR Decomposition)**
* **Cholesky Ayrımsaması (Cholesky Decomposition)**
* **Tekil Değer Ayrışımı (Singular Value Decomposition - SVD)**
* **Özdeğer & Özvektör Hesaplama (Eigenvalue & Eigenvector)**

### 2. 📈 Fonksiyon ve Grafik Çizimi (Graph Plotting)
* Dinamik fonksiyon tanımlama ve analitik değerlendirme.
* 2D/3D grafik çizim kontrolleri ile görselleştirme.

### 3. 🔍 Nümerik Yöntemler & Denklem Çözücüler (Numerical Methods)
* **Kök Bulma (Root Finder):** Gelişmiş nümerik algoritmalar ile karmaşık denklemlerin köklerini hassas bir şekilde bulur.
* **Denklem Sistemleri Çözücü (System Solver):** Çok bilinmeyenli denklem sistemlerinin matris tabanlı çözümü.
* **Taylor Serisi Açılımı (Taylor Series):** Tek değişkenli ve çok değişkenli fonksiyonlar için dinamik Taylor serisi açılımları ve yaklaşım grafikleri.

---

## 🚀 Kurulum ve Çalıştırma (.EXE)

Uygulamanın hazır derlenmiş Windows sürümünü (`ttCalculator.exe`) çalıştırmak son derece kolaydır:

1. Bu GitHub deposunun sağ tarafındaki **[Releases](https://github.com)** (Sürümler) bölümüne gidin.
2. En son yayınlanan sürümün altındaki **`ttCalculator.exe`** (veya sıkıştırılmış `Math.zip`) dosyasını indirin.
3. İndirdiğiniz dosyayı çalıştırarak gelişmiş matematik motorunu anında deneyimleyin!

---

## 🛠️ Teknolojiler & Proje Yapısı

* **Unity Engine:** Görsel arayüz ve etkileşimli grafik çizimleri için kullanılmıştır (`Assets` klasörü).
* **.NET C# Core:** Arka plandaki tüm yüksek hassasiyetli matematik, matris ve nümerik yöntem kütüphaneleri C# dilinde yüksek performanslı olarak geliştirilmiştir (`MathApp/MathApp/Core`).
* **Windows Forms (MathApp):** Masaüstü entegrasyonu ve kontrol panelleri için alternatif gelişmiş masaüstü arayüzü sunar.

---

## 📦 Geliştiriciler İçin Derleme (Build)

Projeyi kendi bilgisayarınızda açıp geliştirmek isterseniz:
1. Depoyu bilgisayarınıza klonlayın:
   ```bash
   git clone https://github.com/KULLANICI_ADINIZ/ttCalculator.git
   ```
2. **Unity Projesi için:** Unity Hub üzerinden projeyi ekleyin (Unity 2021.3 veya üzeri önerilir).
3. **Masaüstü App için:** `Math.sln` dosyasını **Visual Studio** ile açarak derleyebilirsiniz.

---

## 📄 Lisans
Bu proje **MIT Lisansı** altında lisanslanmıştır. Daha fazla bilgi için `LICENSE` dosyasına bakabilirsiniz.
