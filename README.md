# Northwind Veritabanına Sipariş Eklemek

>Northwind veritabanını kullanarak sipariş ekleme uygulamasıdır. Program ilk açıldığında sistemde kayıtlı tüm siparişler listelenir. Sipariş No, Sipariş Tarihi, Son Teslim Tarihi, Müşteri Adı, Çalışan Adı Soyadı, Teslimat Adresi(şehir ve ülke bilgisiyle birlikte) ve Kargo Ücreti olacak şekilde filtrelenir.

![OrderList](https://resimag.com/p1/6815f91f389.png)

>Yeni Sipariş butonuna basıldığında açılan ekranda, kullanıcı seçilen her bir ürün için istenilen kadar adet girilebilir. Siparişe eklenen her ürün için ilk başta adet bilgisi 1, fiyat bilgisi de birim fiyat olarak veritabanından çekilir. Ekledikten sonra bu bilgiler değiştirilebilir. Ürün indirim yapılarak daha ucuza satılabilir.

>Siparişin toplam tutarı kullanıcıya gösterilir. Fiyat ve indirim bilgisi değiştirildiğinde toplam tutar anlık güncellenir.
Sipariş Ekle butonuna basıldığında sipariş veritabanına kaydedilir, oluşturma ekranı kapanır ve listeleme ekranına dönülür. Yeni eklenen sipariş listeleme ekranında hemen güncel bir şekilde düşer kullanıcı kontrol eder.

![AddOrder](https://resimag.com/p1/55d71628ab2.png)


## EKSTRA
1) Siparişleri listelerken geç teslim edilenlerin arkaplanı otomatik kırmızı renk olur. Kaç gün geciktiği bilgisi ayrı bir kolonda oluşturulup kullanıcıya gösterilir.
2) Siparişte yer alan ürünler için program stok kontrolü yapar. Stokta yer alan miktardan fazla adet bilgisi girilirse kullanıcıya uyarı mesajı gönderilir. Sipariş oluşturulamaz. Kullanıcı yeterli stokta ürün girdiğinde kabul eder. Sipariş eklenince stok otomatik düşer.
3) Siparişte yer alan ürünlere yapılan indirimlerin toplamı, siparişin indirimsiz toplam fiyatının yüzde 15’inden fazla olmaması için program kontrol eder ve uyarı verir.
