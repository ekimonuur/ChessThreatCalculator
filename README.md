# ChessThreatCalculator
## Problem tanımı:
### Satranç tahtası üzerinde bulunan taşlara göre iki tarafın (siyah — beyaz) mevcut durumlarının puan
hesaplaması.

Açıklama:
Mevcut puanı hesaplama algoritması şöyledir:
1.	Bir taşın tehdit altında olup olmadığının kontrolü, o taşı tehdit eden karşı renkte bir veya birden fazla taş olması durumunda oluşur.
2.	Eğer bir taş tehdit edilmiyorsa tablodaki puanı alır.
3.	Eğer bir taş karşı tarafın taşlarından herhangi biri tarafından tehdit ediliyorsa, tehdit edilen taşın puanı tablodaki puanının yarısı alınır.

Taş İsmi	Kısaltma	Puanı
Piyon	        p	      1
At	          a	      3
Fil	          f	      3
Kale	        k	      5
Vezir	        v	      9
Şah	          s	      100

## Programı Çalıştırmak
### 1.)
### Visual Studio üzerinden direkt çalıştırmak için dosya içerisinde ki ChessThreatCalculator.sln solution dosyası ile çalıştılabilir. Program derlendiğinde board2.txt otomatik çalışacaktır. Bağımlı olduğu dosyalar uygun çözüme göre değiştirilebilir. 

### 2.)
### Command (cmd) üzerinden lokasyon ile ChessThreatCalculator/ChessThreatCalculator/ChessThreatCalculator/bin/Debug/net5.0/  lokasyonun da "ChessThreatCalcutor.exe board1.txt" yazarak komut satırını çalıştırarak sonuç görülebilir. Burada board1.txt yerine klasörde bulunan board2 ve board3 txt dosyaları da aynı şekilde exe'nin yanına yazılarak çalıştırılabilir. Ayrıca istenilen formatta txt dosyası oluşturup kendi değerlerinizi atamak isterseniz de program çalışacaktır. 

Teşekkürler
