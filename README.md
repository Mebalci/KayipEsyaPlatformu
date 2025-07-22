# Kayıp Eşya Platformu

Bu platform, kullanıcıların **kayıp** veya **bulunan** eşyaları dijital ortamda bildirmesine, benzer ilanlarla **akıllı eşleştirme** yapılmasına ve sahiplerine ulaştırılmasına olanak tanır.

Proje ASP.NET Core MVC ile geliştirilmiş olup Python tabanlı bir Flask servisi ile entegre çalışmaktadır.

---

## 🚀 Özellikler

- Kayıp / bulunan eşya kayıt sistemi
- Açıklama, kategori, renk, marka, model gibi alanlara göre semantik eşleşme
- Konum (il / ilçe / mahalle) tabanlı filtreleme
- Flask tabanlı eşleştirme servisi ile TF-IDF + BERT destekli benzerlik
- Anlık eşleşme bildirimi (SignalR veya AJAX tabanlı)
- Anonim mesajlaşma sistemi
- Google Maps ve Leaflet.js ile harita entegrasyonu

---

## Kullanılan Teknolojiler

| Katman       | Teknolojiler |
|--------------|--------------|
| Backend      | ASP.NET Core MVC, C#, Entity Framework Core |
| Frontend     | HTML, CSS, Bootstrap (Bootswatch - Vapor), JavaScript, AJAX |
| Veritabanı   | MySQL Server |
| ML Servisi   | Python, Flask, scikit-learn, sentence-transformers |
| Harita       | Google Maps API / Leaflet.js |
| Bildirim     | SignalR, BackgroundService |
| Deployment   | Railway / Render / GitHub Actions |

---
