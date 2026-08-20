# APIWar

![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)
![Status](https://img.shields.io/badge/status-early%20development-orange)

**APIWar**, REST API’lerin güvenlik, dayanıklılık ve erişim kontrolü davranışlarını senaryo tabanlı HTTP istekleriyle değerlendirmek için geliştirilen açık kaynaklı bir komut satırı aracıdır.

APIWar; kullanıcının test etme yetkisine sahip olduğu bir API’ye kontrollü istekler gönderir, normal davranış ile güvenlik senaryolarında oluşan sonuçları karşılaştırır ve potansiyel riskleri raporlar.

Araç yalnızca bir endpoint’in çalışıp çalışmadığını kontrol etmez. Aynı endpoint’i farklı kullanıcılar, roller, token’lar, HTTP metotları, parametreler, request body’leri, header’lar, istek hızları ve hata koşulları altında tekrar çalıştırmayı hedefler.

Rate limiting, authentication, authorization, injection, güvenlik yapılandırması, hassas iş akışlarının kötüye kullanımı ve beklenmeyen hata koşulları APIWar’ın temel test alanlarıdır.

> [!WARNING]
> APIWar yalnızca sahibi olduğunuz veya test etmek için açıkça izin aldığınız sistemlerde kullanılmalıdır. İzinsiz tarama, güvenlik testi veya trafik üretimi hukuka aykırı olabilir. Aracı kullanan kişi gerçekleştirdiği işlemlerden tamamen kendisi sorumludur.

## Proje durumu

APIWar şu anda erken geliştirme aşamasındadır.

Solution yapısı, Application ve Infrastructure katmanları, Console uygulaması ve temel raporlama sözleşmesi oluşturulmuştur. Tarama motoru ve bu README içerisinde açıklanan CLI komutları henüz tamamlanmamıştır.

Bu doküman projenin hedeflenen özelliklerini, güvenlik kapsamını ve geliştirme yol haritasını açıklamaktadır.

Henüz production kullanımına hazır bir sürüm veya yayımlanmış .NET global tool paketi bulunmamaktadır.

## APIWar neyi test eder?

Bir endpoint’in yalnızca `200 OK` dönmesi onun güvenli olduğu anlamına gelmez.

Aynı endpoint aşağıdaki koşullarda farklı ve güvensiz davranabilir:

- Token gönderilmediğinde
- Geçersiz veya süresi dolmuş token kullanıldığında
- Farklı kullanıcı veya rollerle çağrıldığında
- Başka kullanıcıya ait kaynak kimliği kullanıldığında
- HTTP metodu değiştirildiğinde
- Request body içerisine beklenmeyen alanlar eklendiğinde
- Query ve route parametreleri değiştirildiğinde
- Beklenmeyen veri tipleri gönderildiğinde
- Kontrollü injection girdileri kullanıldığında
- Çok sayıda veya eş zamanlı istek gönderildiğinde
- Bozuk JSON veya büyük request body kullanıldığında
- Harici servis veya timeout hatası meydana geldiğinde

APIWar bu senaryoları tekrar çalıştırılabilir testlere dönüştürmeyi hedefler.

Sonuç değerlendirmesi yalnızca HTTP durum koduna dayanmaz. Araç aşağıdaki verileri birlikte karşılaştıracaktır:

- Response status code
- Response body
- Response header’ları
- Response boyutu
- Response süresi
- Dönen veri alanları
- Hata mesajları
- Redirect hedefleri
- Rate limit bilgileri
- Kullanıcılar arasındaki veri farklılıkları
- İşlem öncesi ve sonrası kaynak durumu

Tek bir farklı durum kodu otomatik olarak güvenlik açığı kabul edilmez. APIWar birden fazla sinyali ilişkilendirerek bulgunun güven seviyesini belirlemeyi hedefler.

## Güvenlik standartları

APIWar iki ayrı OWASP çalışmasını referans alır:

1. [OWASP Top 10:2025](https://owasp.org/Top10/)  
   Web uygulamaları ve genel uygulama güvenliği için yayımlanan güncel risk sınıflandırmasıdır.

2. [OWASP API Security Top 10:2023](https://owasp.org/API-Security/editions/2023/en/0x10-api-security-risks/)  
   API’lere özel yayımlanmış en güncel OWASP API Security Top 10 sürümüdür.

2026 itibarıyla API’ye özel resmî bir OWASP API Security Top 10:2025 veya 2026 sürümü yayımlanmamıştır.

Bu nedenle APIWar:

- Genel uygulama güvenliği kontrollerini OWASP Top 10:2025 ile
- API’ye özgü güvenlik kontrollerini OWASP API Security Top 10:2023 ile

eşleştirir.

APIWar, OWASP tarafından sertifikalandırılmış veya OWASP ile bağlantılı bir ürün değildir. OWASP adları yalnızca test kapsamını sınıflandırmak ve bulguları bilinen güvenlik kategorileriyle eşlemek için kullanılmaktadır.

## OWASP Top 10:2025 kapsamı

### A01:2025 — Broken Access Control

APIWar erişim kontrolü problemlerini farklı kullanıcı, rol, token ve kaynak bağlamlarını karşılaştırarak test etmeyi hedefler.

Planlanan senaryolar:

- Token olmadan korumalı endpoint’e erişme
- Farklı kullanıcılarla aynı kaynağı çağırma
- Başka kullanıcıya ait kaynak kimliğini kullanma
- Kullanıcı ve yönetici rollerini karşılaştırma
- Yetkisiz `GET`, `POST`, `PUT`, `PATCH` ve `DELETE` istekleri
- UI üzerinde görünmeyen fonksiyonlara doğrudan erişim
- Kaynak sahipliği kontrolleri
- Beklenen `401`, `403` ve `404` davranışlarının doğrulanması
- Route, query ve body içerisindeki ID alanlarının değiştirilmesi
- Yetki seviyesi değiştiğinde response alanlarının karşılaştırılması

### A02:2025 — Security Misconfiguration

API ve HTTP katmanındaki yaygın güvenlik yapılandırma problemleri incelenecektir.

Planlanan kontroller:

- CORS davranışının incelenmesi
- Güvenlik header’larının kontrol edilmesi
- Gereksiz HTTP metotlarının tespit edilmesi
- Debug ve test endpoint’lerinin kontrol edilmesi
- Varsayılan hata sayfalarının tespit edilmesi
- Framework ve sunucu bilgilerinin açığa çıkması
- Stack trace ve hassas hata ayrıntılarının aranması
- HTTP ve HTTPS davranışlarının karşılaştırılması
- Eski veya unutulmuş API sürümlerinin aranması
- Swagger/OpenAPI dokümanlarının erişim durumunun kontrol edilmesi
- Development ayarlarının production ortamında açık kalmasının araştırılması

### A03:2025 — Software Supply Chain Failures

Supply chain güvenliği yalnızca uzak bir API’ye HTTP isteği gönderilerek tamamen doğrulanamaz.

İlerleyen sürümlerde aşağıdaki yardımcı özelliklerin sunulması hedeflenmektedir:

- Dependency ve paket envanteri oluşturma
- SBOM çıktılarının içe aktarılması
- Bilinen zafiyet tarayıcılarının sonuçlarının rapora eklenmesi
- Kullanılan SDK ve üçüncü taraf servislerin envanteri
- Eski veya desteklenmeyen dependency uyarıları
- CI/CD güvenlik araçlarıyla entegrasyon
- Harici tarama sonuçlarının APIWar raporunda birleştirilmesi

### A04:2025 — Cryptographic Failures

Planlanan kontroller:

- HTTPS kullanımının kontrol edilmesi
- HTTP’den HTTPS’e yönlendirme davranışı
- Hassas verilerin URL içerisinde bulunması
- Token, API key veya parola benzeri değerlerin response içerisinde aranması
- Cookie güvenlik özelliklerinin incelenmesi
- Güvensiz transport davranışlarının raporlanması
- Sertifika ve protokol bilgilerinin rapora eklenmesi
- Hassas verilerin response içerisinde gereksiz yere açığa çıkması

APIWar black-box bir HTTP aracı olarak sunucu tarafındaki veri saklama ve encryption mekanizmalarını tek başına doğrulayamaz. Bu kontroller manuel inceleme gerektiren alanlar olarak raporlanacaktır.

### A05:2025 — Injection

APIWar kontrollü ve veri kaybına neden olmayacak injection senaryoları çalıştırmayı hedefler.

Planlanan kontroller:

- Query string alanlarında SQL injection belirtileri
- Route parametrelerinde injection denemeleri
- JSON body alanlarında injection denemeleri
- Request header’larında beklenmeyen girdiler
- Veritabanı ve ORM hata mesajlarının tespiti
- Framework ve parser hata mesajlarının tespiti
- Response süresindeki anormal değişimlerin incelenmesi
- Durum kodu ve response body farklılıklarının karşılaştırılması
- Bozuk JSON gönderilmesi
- Beklenmeyen veri tiplerinin kullanılması
- Uzun değer ve sınır değeri senaryoları
- Content type ve encoding varyasyonları

Varsayılan testler:

- Veri silmez
- Veritabanı şemasını değiştirmez
- Kalıcı hasar oluşturmaz
- Tehlikeli payload’ları otomatik çalıştırmaz

### A06:2025 — Insecure Design

İş mantığı ve tasarım problemleri kullanıcı tarafından tanımlanan senaryolarla test edilecektir.

Planlanan senaryolar:

- İşlem adımlarını yanlış sırada çağırma
- Tek kullanımlık işlemleri tekrar gönderme
- Ön koşul tamamlanmadan sonraki adıma geçme
- Kullanıcı başına işlem sınırlarını kontrol etme
- Kaynak başına iş kuralı sınırlarını kontrol etme
- Kupon, rezervasyon, kayıt ve doğrulama işlemlerini tekrarlama
- Aynı işlemi kısa sürede birden fazla kez gönderme
- İş akışındaki adımları atlama
- Beklenen iş kurallarını kullanıcı tarafından tanımlama

İş mantığı testleri hedef uygulamanın kurallarını otomatik olarak bilemeyeceği için senaryo yapılandırması ve güvenli test verisi gerektirebilir.

### A07:2025 — Authentication Failures

Planlanan authentication senaryoları:

- Authentication header’ı olmadan istek gönderme
- Geçersiz token kullanma
- Bozuk token kullanma
- Süresi dolmuş token kullanma
- Yanlış authentication scheme kullanma
- Boş token gönderme
- Token kapsamını farklı endpoint’lerde karşılaştırma
- Login endpoint’lerinde rate limit kontrolü
- Token endpoint’lerinde rate limit kontrolü
- Kullanıcı varlığını açığa çıkaran hata mesajlarını inceleme
- Başarısız authentication cevaplarını karşılaştırma

APIWar:

- Parola kırma aracı değildir
- Credential stuffing aracı değildir
- Hesap ele geçirme amacıyla tasarlanmamıştır

### A08:2025 — Software or Data Integrity Failures

Planlanan kontroller:

- Webhook isteklerinde imza kontrolü
- Callback isteklerinde doğrulama kontrolü
- Beklenmeyen veya değiştirilmiş veri alanları
- Harici API cevaplarının doğrulanmadan kullanılması
- Content type ve veri bütünlüğü kontrolleri
- Tekrarlanan request ve replay davranışı
- Güven sınırları arasında taşınan verilerin doğrulanması
- Eksik veya değiştirilmiş imza/header davranışlarının karşılaştırılması

### A09:2025 — Security Logging and Alerting Failures

Sunucu loglarının oluşup oluşmadığı yalnızca black-box HTTP testiyle kesin olarak kanıtlanamaz.

Yetkili log, webhook veya SIEM entegrasyonu sağlandığında aşağıdaki kontroller hedeflenmektedir:

- Güvenlik testinin log kaydıyla eşleştirilmesi
- Request correlation ID takibi
- Başarısız authentication olaylarının kayda alınması
- Başarısız authorization olaylarının kayda alınması
- Hassas verilerin loglarda maskelenmesi
- Beklenen alarm veya webhook olayının oluşması
- APIWar bulgularıyla sunucu loglarının ilişkilendirilmesi

### A10:2025 — Mishandling of Exceptional Conditions

Planlanan hata ve istisna senaryoları:

- Eksik zorunlu alanlar
- `null` değerler
- Boş değerler
- Beklenmeyen veri tipleri
- Bozuk JSON
- Desteklenmeyen content type
- Negatif sayılar
- Aşırı büyük sayılar
- Sınır değerleri
- Çok uzun metinler
- Büyük request body
- Tekrarlanan parametreler
- Timeout senaryoları
- Bağlantı kesintileri
- Yetersiz yetki durumları
- Hassas hata mesajları
- Stack trace çıktıları
- Beklenmeyen koşullarda erişim kontrolünün açık kalması
- Sistem hatalarında güvenli şekilde başarısız olma davranışı

## OWASP API Security Top 10:2023 kapsamı

| Kategori | APIWar test yaklaşımı |
| --- | --- |
| API1 — Broken Object Level Authorization | Kaynak kimliklerini ve kullanıcı bağlamlarını karşılaştırarak BOLA/IDOR belirtilerini araştırır. |
| API2 — Broken Authentication | Eksik, geçersiz ve süresi dolmuş kimlik bilgileriyle kontrollü istekler gönderir. |
| API3 — Broken Object Property Level Authorization | Hassas response alanlarını ve değiştirilmemesi gereken request alanlarını kontrol eder. |
| API4 — Unrestricted Resource Consumption | Rate limit, request boyutu, concurrency, timeout ve kaynak tüketimi davranışını ölçer. |
| API5 — Broken Function Level Authorization | Aynı fonksiyonu farklı roller, token’lar ve HTTP metotlarıyla karşılaştırır. |
| API6 — Unrestricted Access to Sensitive Business Flows | Hassas iş akışlarının tekrar ve otomasyon yoluyla kötüye kullanım direncini test eder. |
| API7 — Server-Side Request Forgery | URL alan endpoint’lerde yalnızca izin verilen güvenli callback hedefleriyle doğrulama yapar. |
| API8 — Security Misconfiguration | CORS, security header, hata çıktısı, HTTP metodu ve ortam yapılandırmalarını inceler. |
| API9 — Improper Inventory Management | OpenAPI envanteriyle erişilebilir endpoint ve API sürümlerini karşılaştırmayı hedefler. |
| API10 — Unsafe Consumption of APIs | Harici servislerden gelen eksik, bozuk veya beklenmeyen verilere verilen tepkiyi test eder. |

## Senaryo tabanlı test motoru

APIWar’ın merkezinde tekrar kullanılabilir güvenlik senaryoları bulunacaktır.

Bir senaryo aşağıdaki bilgileri içerebilir:

- Hedef API adresi
- Endpoint yolu
- HTTP metodu
- Authentication profili
- Kullanıcı veya rol bağlamı
- Route parametreleri
- Query parametreleri
- Request header’ları
- Request body
- Değiştirilecek alanlar
- Beklenen durum kodları
- Beklenen response alanları
- Yasaklanan response alanları
- Maksimum response süresi
- Request sayısı
- Eş zamanlılık seviyesi
- Risk seviyesi
- Test öncesi ve sonrası adımlar

Tipik bir test senaryosu aşağıdaki şekilde çalışacaktır:

1. Geçerli bilgilerle baseline istek gönderilir.
2. Response yapısı, süresi, boyutu ve header’ları kaydedilir.
3. Seçilen güvenlik senaryosuna göre istek kontrollü şekilde değiştirilir.
4. Değiştirilmiş istek hedef API’ye gönderilir.
5. Baseline ve test response’ları karşılaştırılır.
6. Şüpheli farklılıklar kanıtlarıyla birlikte bulguya dönüştürülür.
7. Hassas değerler maskelenerek rapor oluşturulur.

## Örnek senaryolar

### Yetkisiz kaynak erişimi

1. Birinci kullanıcı kendi kaynağına erişir.
2. Response baseline olarak kaydedilir.
3. Aynı istek ikinci kullanıcının token’ıyla gönderilir.
4. Kaynak kimliği başka kullanıcıya ait bir kimlikle değiştirilir.
5. Response durum kodu ve içeriği karşılaştırılır.
6. Yetkisiz veri dönerse potansiyel erişim kontrolü problemi raporlanır.

### Rate limit kontrolü

1. Endpoint’e düşük hızda baseline istekleri gönderilir.
2. İstek hızı güvenli sınırlar içerisinde kademeli olarak artırılır.
3. `429 Too Many Requests` cevabı kontrol edilir.
4. `Retry-After` ve rate limit header’ları incelenir.
5. Token, kullanıcı ve endpoint bazlı limitler karşılaştırılır.
6. Limit sonrası toparlanma süresi ölçülür.

### Input validation kontrolü

1. Geçerli bir request baseline olarak gönderilir.
2. Seçilen alanın veri tipi değiştirilir.
3. Boş, `null`, uzun veya sınır değerindeki girdiler kullanılır.
4. Response durumu, süresi ve hata mesajı karşılaştırılır.
5. Hassas sistem veya veritabanı bilgisi açığa çıkarsa raporlanır.

### Function level authorization kontrolü

1. Yönetici yetkisi gerektiren bir işlem tanımlanır.
2. İşlem yönetici token’ıyla çağrılır.
3. Aynı işlem standart kullanıcı token’ıyla tekrarlanır.
4. HTTP metodu ve endpoint varyasyonları denenir.
5. Yetkisiz işlem başarılı olursa bulgu oluşturulur.

## Rate limit testleri

APIWar aşağıdaki rate limit senaryolarını desteklemeyi hedefler:

- Kısa süreli kontrollü request burst’leri
- Belirli süre boyunca sabit istek hızı
- Yapılandırılabilir eş zamanlı istek sayısı
- `429 Too Many Requests` kontrolü
- `Retry-After` header analizi
- Rate limit header analizi
- Token bazlı limit karşılaştırması
- Kullanıcı bazlı limit karşılaştırması
- Endpoint bazlı limit karşılaştırması
- Limit sonrası toparlanma süresinin ölçülmesi
- Başarılı, reddedilen ve timeout olan isteklerin sayılması
- Response süresindeki bozulmanın raporlanması

APIWar bir DDoS aracı değildir.

Güvenli profil düşük limitlerle çalışır. Yoğun trafik testleri ayrıca etkinleştirilmediği sürece yürütülmez.

## Test profilleri

### Safe

Varsayılan profildir.

- Düşük istek sayısı
- Düşük concurrency
- Kalıcı veri değiştirmeyen testler
- Security header kontrolleri
- CORS kontrolleri
- Hata davranışı kontrolleri
- Sınırlı rate limit denemeleri
- Riskli senaryolar devre dışı

### Standard

Daha kapsamlı güvenlik kontrolleri sağlar.

- Authentication karşılaştırmaları
- Authorization karşılaştırmaları
- Kaynak kimliği varyasyonları
- Request body alanı varyasyonları
- Kontrollü injection testleri
- HTTP metodu denemeleri
- Orta seviyeli rate limit kontrolleri

### Intensive

Yalnızca açıkça etkinleştirilir.

Test ve staging ortamları için tasarlanır.

- Daha yüksek istek sayısı
- Paralel istekler
- Büyük request senaryoları
- Daha geniş endpoint varyasyonları
- Daha geniş parametre varyasyonları
- Hassas iş akışı tekrarları

## Güvenli çalışma yaklaşımı

APIWar aşağıdaki güvenlik prensiplerine göre geliştirilmektedir:

- Testler yalnızca açıkça belirtilen hedeflerde çalıştırılır.
- Varsayılan test profili kalıcı veri değişikliği oluşturmaz.
- Trafik miktarı kullanıcı tarafından sınırlandırılabilir.
- Rate limit ve concurrency değerleri kontrollü tutulur.
- Hassas header ve token değerleri raporlarda maskelenir.
- Riskli testler açıkça etkinleştirilmeden çalıştırılmaz.
- Her bulgu tekrar üretilebilir kanıtlarla raporlanır.
- Servis engelleme oluşturabilecek değerler varsayılan olarak kullanılmaz.
- Harici callback testleri yalnızca izin verilen adreslere gönderilir.
- Destructive payload’lar varsayılan test paketinde bulunmaz.

## Hedeflenen CLI kullanımı

> Aşağıdaki komutlar planlanan CLI arayüzünü göstermektedir. Mevcut erken geliştirme sürümünde henüz çalışmayabilir.

### Temel güvenli tarama

```bash
apiwar scan --target https://api.example.com --profile safe
```

### Belirli test gruplarını çalıştırma

```bash
apiwar scan \
  --target https://api.example.com \
  --tests access-control,authentication,injection,rate-limit
```

### OpenAPI dokümanı kullanma

```bash
apiwar scan \
  --target https://api.example.com \
  --openapi ./openapi.json \
  --profile standard
```

### Authentication profili kullanma

```bash
apiwar scan \
  --target https://api.example.com \
  --auth-profile standard-user
```

### Rapor oluşturma

```bash
apiwar scan \
  --target https://api.example.com \
  --format markdown \
  --output ./reports/apiwar-report.md
```

Token ve diğer hassas değerlerin doğrudan terminal geçmişine yazılması önerilmez.

Environment variable ve güvenli secret sağlayıcılarının desteklenmesi planlanmaktadır.

## Planlanan yapılandırma

Aşağıdaki örnek hedeflenen yapılandırma yaklaşımını göstermektedir. Nihai şema geliştirme sırasında değişebilir.

```yaml
target:
  baseUrl: https://api.example.com
  openApi: ./openapi.json

execution:
  profile: safe
  timeout: 10s
  maxRequestsPerSecond: 5
  concurrency: 2

authentication:
  profiles:
    - name: standard-user
      type: bearer
      tokenFromEnvironment: APIWAR_USER_TOKEN

    - name: admin-user
      type: bearer
      tokenFromEnvironment: APIWAR_ADMIN_TOKEN

report:
  format: markdown
  output: ./reports/apiwar-report.md
  maskSecrets: true
```

## Raporlama

Planlanan rapor formatları:

- Console
- JSON
- Markdown
- HTML
- SARIF

Her bulgu mümkün olduğunda aşağıdaki bilgileri içerecektir:

- Bulgu başlığı
- Açıklama
- İlgili OWASP kategorisi
- Risk seviyesi
- Güven seviyesi
- Etkilenen endpoint
- HTTP metodu
- Kullanılan test senaryosu
- Beklenen davranış
- Gözlemlenen davranış
- Response status code
- Response süresi
- Response boyutu
- Maskelenmiş request özeti
- Maskelenmiş response özeti
- Tekrar üretme adımları
- Önerilen düzeltme
- Test zamanı

API key, token, cookie, parola ve kişisel veri gibi hassas bilgiler rapora yazılmadan önce maskelenmelidir.

## Sınırlamalar

APIWar kullanılırken aşağıdaki sınırlamalar dikkate alınmalıdır:

- Black-box HTTP testleri kaynak kod güvenlik analizinin yerini tutmaz.
- Bir API’nin bütün iş kuralları otomatik olarak çıkarılamaz.
- Authorization testleri birden fazla kullanıcı veya rol profili gerektirebilir.
- Sunucu logları sağlanmadan logging kontrolleri kesin olarak doğrulanamaz.
- Dependency ve supply chain riskleri yalnızca uzak endpoint üzerinden tamamen belirlenemez.
- Otomatik bulgular false positive veya false negative içerebilir.
- OWASP Top 10 kapsamı eksiksiz bir penetration test anlamına gelmez.
- Bazı bulguların güvenlik uzmanı tarafından doğrulanması gerekir.

APIWar sonuçları aşağıdaki süreçlerle birlikte kullanılmalıdır:

- Güvenlik uzmanı değerlendirmesi
- Kod incelemesi
- Threat modeling
- Static analysis
- Dependency scanning
- Güvenli geliştirme süreçleri
- Manuel penetration testleri

## Proje yapısı

```text
ApiWar/
├── ApiWar.Application/
│   └── Uygulama sözleşmeleri ve kullanım senaryoları
├── ApiWar.Infrastructure/
│   └── HTTP, dosya ve raporlama implementasyonları
├── ApiWar.Console/
│   └── Komut satırı giriş noktası
└── ApiWar.slnx
```

### ApiWar.Application

Tarama akışlarını ve dış bağımlılıklara ait sözleşmeleri içerir.

`IDocsWriter`, farklı rapor formatlarının uygulama katmanından bağımsız şekilde üretilebilmesi için oluşturulan raporlama sözleşmesidir.

### ApiWar.Infrastructure

Application katmanındaki sözleşmelerin gerçek implementasyonlarını içerir.

Aşağıdaki bileşenler bu katmanda konumlandırılacaktır:

- HTTP istemcileri
- Dosya işlemleri
- Rapor üretimi
- OpenAPI işlemleri
- Harici servis entegrasyonları
- Güvenli callback servisleri

### ApiWar.Console

Kullanıcının APIWar’ı terminal üzerinden çalıştırmasını sağlayacak CLI giriş noktasıdır.

## Gereksinimler

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Git

## Kaynak koddan çalıştırma

Repoyu klonlayın:

```bash
git clone https://github.com/emreucbudak/ApiWar.git
cd ApiWar
```

Projeyi restore edin:

```bash
dotnet restore ApiWar.slnx
```

Projeyi derleyin:

```bash
dotnet build ApiWar.slnx
```

Console uygulamasını çalıştırın:

```bash
dotnet run --project ApiWar.Console
```

Console uygulaması henüz erken geliştirme aşamasında olduğundan tarama komutları tamamlanmamıştır.

## Planlanan .NET global tool yayını

APIWar NuGet üzerinde yayımlandıktan sonra hedeflenen kurulum şekli:

```bash
dotnet tool install --global ApiWar
```

Güncelleme:

```bash
dotnet tool update --global ApiWar
```

Kaldırma:

```bash
dotnet tool uninstall --global ApiWar
```

Bu komutlar paket yayımlanana kadar çalışmayacaktır.

## Yol haritası

- [x] Temel solution yapısı
- [x] Application ve Infrastructure sınıf kitaplıkları
- [x] Console uygulaması
- [x] `IDocsWriter` raporlama sözleşmesi
- [x] Proje güvenlik kapsamının belirlenmesi
- [ ] CLI command ve argument altyapısı
- [ ] Hedef URL ve endpoint yapılandırması
- [ ] HTTP istemci altyapısı
- [ ] Scenario runner
- [ ] Baseline request karşılaştırması
- [ ] Rate limit testleri
- [ ] Authentication profilleri
- [ ] Authorization profilleri
- [ ] Kontrollü injection kontrolleri
- [ ] CORS kontrolleri
- [ ] Security header kontrolleri
- [ ] OpenAPI/Swagger içe aktarma
- [ ] Endpoint envanteri
- [ ] JSON raporu
- [ ] Markdown raporu
- [ ] HTML raporu
- [ ] Hassas veri maskeleme
- [ ] Safe, Standard ve Intensive profilleri
- [ ] SARIF desteği
- [ ] CI/CD entegrasyonu
- [ ] Unit testler
- [ ] Integration testler
- [ ] NuGet üzerinden .NET global tool yayını

## Etik ve yasal kullanım

APIWar aşağıdaki kullanım alanları için geliştirilmektedir:

- Kendi API’nizi test etmek
- Açıkça izin verilen penetration test çalışmaları
- Yerel geliştirme ortamları
- Test ortamları
- Staging ortamları
- Eğitim amaçlı güvenli laboratuvarlar
- CI/CD güvenlik kontrolleri
- Kapsamı açıkça tanımlanmış bug bounty programları
- API regression testleri
- Mikroservis güvenlik kontrolleri

APIWar aşağıdaki amaçlarla kullanılmamalıdır:

- İzinsiz sistem taraması
- Servis engelleme saldırıları
- Trafik saldırıları
- Veri çalma
- Veri silme veya değiştirme
- Yetkisiz erişim elde etme
- Hesap ele geçirme
- Başka kullanıcıların hizmetlerini bozma
- Test kapsamı dışındaki sistemlere istek gönderme

Test başlamadan önce aşağıdaki bilgiler açıkça belirlenmelidir:

- Test edilmesine izin verilen hedefler
- İzin verilen endpoint’ler
- Yasaklı endpoint ve işlemler
- Trafik sınırları
- Test zamanı
- Kullanılabilecek test verileri
- İzin verilen kullanıcı hesapları
- Acil durdurma yöntemi

## Katkıda bulunma

Katkılar, hata bildirimleri ve özellik önerileri memnuniyetle karşılanır.

1. Repoyu fork edin.
2. Yeni bir feature dalı oluşturun.
3. Değişikliklerinizi ve testlerinizi ekleyin.
4. Değişikliklerinizi commit edin.
5. Dalınızı pushlayın.
6. Açıklayıcı bir pull request oluşturun.

Güvenlik testi davranışını değiştiren katkılar:

- Güvenli varsayılanlara sahip olmalıdır.
- Trafik sınırlarını korumalıdır.
- Oluşturabileceği yan etkileri açıklamalıdır.
- Destructive davranışları varsayılan olarak kapalı tutmalıdır.
- Test kapsamını açıkça belgelemelidir.

## Güvenlik bildirimi

APIWar içerisinde bir güvenlik açığı tespit ederseniz gerçek token, parola, connection string, kişisel veri veya hedef sistem çıktısını herkese açık issue’lara eklemeyin.

Hassas güvenlik bildirimlerini proje sahibiyle özel olarak paylaşın.

---

**APIWar — API’nizin yalnızca çalıştığını değil, beklenmeyen ve kötüye kullanım senaryolarında nasıl davrandığını da test edin.**
