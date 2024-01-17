# Online Platformer Prototype

Oyun ikili çatışmanın online versiyonunu yapma fikriyle yapıldı ve amacım hızlıca oyunu prototipleyip arkadaşlarımla oynamak olduğundan kötü bir kod tabanı var ama fikir verebilecek bir proje olduğunu düşünüyorum.

### Oyundaki silah modelleri para ile aldığım bir asset olduğumdan sildim, lütfen kendi modellerinizi koyun ve korsan asset dağıtımına yardımcı olmayınız !

Uzun bir süre önce geliştirdiğim bir proje ve bu projeyle uğraşırken OOP nedir, git nedir vs. hiç bilmiyordum...
Photon Bolt Networking ile geliştirdiğim bu projeyi belki üç beş kod çalarsınız ya da build alıp arkadaşlarınızla oynarsınız diye yükledim, çok gelecek vadeden bir proje tabanına sahip olduğunu düşünmüyorum.

Photon'un sitesine üye olup bir app id alarak bu projeyi online sistemi çalışır yapabilir ve sonrasında build alarak afiyetle oynayabilirsiniz.

Oyun client sided olduğundan çok kolay hile yapılabiliyor haberiniz olsun.

## Oyundaki Modlar
- Boss Battle
- Team Deathmatch
- Free For All Deathmatch
- Free For ALl Sandbox Deathmatch

## Oyundaki Silahlar
- Sniper
- Ligth Machine Gun
- 2 Assault Rifle (AK-47 ve M4)
- Rocket Launcher
- Cluster Bomb Launcher
- 3 Pistol (bir adet otomatik, bir adet yavaş ama güçlü, bir adet de dengeli)
- Auto Shotgun
- Shotgun

## Oyun Modları (Detaylı Açıklama)
### Boss Battle
Bir oyuncu boss olur ve boss olan oyuncu zor aşağıya atılmakla birlikte
- LMG (Ligth Machine Gun)
- Rocket Launcher
- Auto Shotgun
- Cluster Bomb Launcher
Silahlarına erişim sağlar fakat diğer oyuncular menüden seçtikleri tabancayla başlayıp yukardaki kutulardan düşen silahları alıp savaşır.

Bossu öldüren oyuncu boss olur.

### Team Deathmatch
Free For All Deathmatch ama başlangıçta mavi ya da kırmızı takım seçilir.

### Free For All Deathmatch
Herkes tek tabancalarıyla başlayarak kapışır ve yukardan silah kutuları düşer.

### Free For ALl Sandbox Deathmatch
Free For All Deathmatch ama her silah g tuşu ile seçilerek alınabilir.
