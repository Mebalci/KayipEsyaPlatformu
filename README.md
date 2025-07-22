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

<img width="1695" height="930" alt="image" src="https://github.com/user-attachments/assets/f63f279d-8acb-400d-8358-39da4bbbb163" />

<img width="843" height="932" alt="image" src="https://github.com/user-attachments/assets/8d2049df-b3d7-4830-bcfb-f8401b440c51" />

<img width="1654" height="926" alt="image" src="https://github.com/user-attachments/assets/20d7c931-c874-451f-b848-8980eb3db11c" />
