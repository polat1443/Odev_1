namespace Odev_1
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // Statü Stepper'ı değiştiğinde çalışır
        private void stpStatu_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            int seviye = (int)stpStatu.Value;

            if (seviye == 1)
                lblStatu.Text = "Seviye 1: Stajyer / Çırak (%0 Bonus)";
            else if (seviye == 2)
                lblStatu.Text = "Seviye 2: Normal İşçi (%10 Bonus)";
            else if (seviye == 3)
                lblStatu.Text = "Seviye 3: Usta / Kıdemli (%25 Bonus)";
            else if (seviye == 4)
                lblStatu.Text = "Seviye 4: Şef / Yönetici (%50 Bonus)";
        }

        // Tatil Slider'ı kaydırıldığında çalışır: Stepper'ı ve Label'ı günceller
        private void sldTatil_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            int tatilGunu = (int)sldTatil.Value;
            stpTatil.Value = tatilGunu;

            int calisilanGun = 30 - tatilGunu;
            lblTatilBilgi.Text = $"Tatil Gün Sayısı: {tatilGunu} Gün (Çalışılan: {calisilanGun} Gün)";
        }

        // Tatil Stepper'ı (+ / -) tıklandığında çalışır: Slider'ı ve Label'ı günceller
        private void stpTatil_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            int tatilGunu = (int)stpTatil.Value;
            sldTatil.Value = tatilGunu;

            int calisilanGun = 30 - tatilGunu;
            lblTatilBilgi.Text = $"Tatil Gün Sayısı: {tatilGunu} Gün (Çalışılan: {calisilanGun} Gün)";
        }

        // Mesai saati Slider'ı kaydırıldığında çalışır
        private void sldMesaiSaat_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            int mesaiSaati = (int)sldMesaiSaat.Value;
            lblMesaiSaat.Text = $"Aylık Fazla Mesai Süresi: {mesaiSaati} Saat";
        }

        // Hesapla Butonuna Tıklandığında Çalışacak Metot
        private async void btnHesapla_Clicked(object sender, EventArgs e)
        {
            // 1. Ad Soyad kontrolü
            if (string.IsNullOrWhiteSpace(entAd.Text))
            {
                await DisplayAlertAsync("Uyarı", "Lütfen çalışan adını giriniz!", "Tamam");
                return;
            }

            // 2. Günlük maaş ve saatlik mesai ücretini sayıya (double) çevirme kontrolü
            double gunlukMaas = 0;
            double saatlikMesaiUcreti = 0;

            if (!double.TryParse(entGunlukMaas.Text, out gunlukMaas))
            {
                await DisplayAlertAsync("Uyarı", "Lütfen geçerli bir günlük maaş giriniz!", "Tamam");
                return;
            }

            // Eğer mesai ücreti girilmemişse 0 kabul et veya sayıya çevir
            double.TryParse(entMesaiUcreti.Text, out saatlikMesaiUcreti);

            // 3. Değerleri Kontrollerden Alma
            string adSoyad = entAd.Text.ToUpper();
            int statuSeviye = (int)stpStatu.Value;
            int tatilGunu = (int)sldTatil.Value;
            int calisilanGun = 30 - tatilGunu;
            int mesaiSaati = (int)sldMesaiSaat.Value;

            // 4. Statü Adı ve Bonus Oranını Belirleme
            string statuAdi = "";
            double bonusOrani = 0;

            if (statuSeviye == 1)
            {
                statuAdi = "Stajyer/Çırak";
                bonusOrani = 0.0;
            }
            else if (statuSeviye == 2)
            {
                statuAdi = "Normal İşçi";
                bonusOrani = 0.10; // %10
            }
            else if (statuSeviye == 3)
            {
                statuAdi = "Usta/Kıdemli";
                bonusOrani = 0.25; // %25
            }
            else if (statuSeviye == 4)
            {
                statuAdi = "Şef/Yönetici";
                bonusOrani = 0.50; // %50
            }

            // 5. Matematiksel Hesaplamalar (Hocanın notundaki Math.Round kullanımı ile)
            double aylikYevmiyeToplami = Math.Round((calisilanGun * gunlukMaas) * 100) / 100;
            double statuBonusu = Math.Round((aylikYevmiyeToplami * bonusOrani) * 100) / 100;
            double aylikMesaiToplami = Math.Round((mesaiSaati * saatlikMesaiUcreti) * 100) / 100;
            double genelToplamMaas = Math.Round((aylikYevmiyeToplami + statuBonusu + aylikMesaiToplami) * 100) / 100;

            // 6. Sonuçları Grid İçindeki Label'lara Yazdırma
            lblSonucAd.Text = adSoyad;
            lblSonucStatuGun.Text = $"{statuAdi} ({calisilanGun} Gün)";
            lblSonucYevmiye.Text = $"{aylikYevmiyeToplami} TL";
            lblSonucBonus.Text = $"{statuBonusu} TL";
            lblSonucMesai.Text = $"{aylikMesaiToplami} TL";
            lblSonucToplam.Text = $"{genelToplamMaas} TL";

            // 7. Ekrana Özet Mesaj Kutusu Çıkarma
            await DisplayAlertAsync("Hesaplama Tamamlandı",
                $"Sayın {adSoyad},\nBu ayki net maaşınız: {genelToplamMaas} TL olarak hesaplanmıştır.",
                "Tamam");
        }
    }
}