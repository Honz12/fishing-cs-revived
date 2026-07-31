# 🎣 Kde Jsou Ryby?!

Tento projekt je založený na `Honz12/fishing-cs-hackathon`

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat&logo=dotnet)
![License](https://img.shields.io/badge/license-MIT-green)
![C#](https://img.shields.io/badge/C%23-12.0-239120?style=flat&logo=csharp)

**Kde Jsou Ryby?!** je konzolová textová rybářská hra napsaná v C# s barevnou ANSI grafikou přímo v terminálu. Chytej ryby, vylepšuj vybavení a dokaž tátovi, že rybaření má smysl!

---

## 📖 O hře

Hráč se ocitá v roli mladého rybáře, který se rozhodne jít za svým snem i přes otcovo nesouhlas. Pomocí minihry s pohybujícím se kurzorem chytáš ryby, které se zobrazují jako **16×16 pixelové ANSI obrázky** — přímo v terminálu!

Hra obsahuje **20+ druhů ryb** (sladkovodní i mořské) + tajný **Kraken**, každá s vlastní vahou, raritou a požadavky na výbavu.

### Herní smyčka

```
Hlavní menu
 ├── 🎣 Jít chytat ryby — minihra s pohyblivým ukazatelem
 ├── 🏪 Jít do obchodu — nákup vylepšení (prut, loď, dům)
 ├── 📦 Chladicí box — přehled a prodej chycených ryb
 └── 🚪 Opustit hru
```

### 🎮 Minihra

Rybaření probíhá v reálném čase:
- Uprostřed lišty je **zelené pole** — v něm musíš udržet kurzor
- **Červená pole** po stranách — kurzor zde znamená ztrátu progresu
- Ryba táhne a zelené pole se pohybuje — čím vzácnější ryba, tím rychleji
- Klávesou **jakéhokoliv písmena** skáče kurzor dopředu
- Vyhráváš, když naplníš ukazatel progresu

### 🐟 Ryby

| Rarita | Barva | Příklady |
|---|---|---|
| Běžná | ⬜ | Kapr, Okoun, Lín |
| Neobyčejná | 🟩 | Pstruh, Štika, Candát |
| Epická | 🟪 | Úhoř, Mořský ďas, Mečoun |
| Mytická | 🟥 | Tuňák, Čtverzubec fugu |
| KRAKEN | 🟨 | Kraken 🦑 |

### 🛒 Obchod

V obchodě můžeš utrácet peníze za:

| Vylepšení | Max úroveň | Efekt |
|---|---|---|
| 🎣 **Prut** | 11 (0–10) | Odemyká vzácnější ryby |
| 🚢 **Loď** | 5 (0–4) | Zvětšuje chladicí box |
| 🏠 **Obydlí** | 5 (0–4) | Postup v příběhu |

Po koupi vily (obydlí úroveň 5) se odemkne **závěrečná scéna**.

---

## 🚀 Jak spustit

### Požadavky

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) nebo novější
- Terminál podporující ANSI escape sekvence (Windows Terminal, GNOME Terminal, iTerm2 apod.)

### Spuštění

```bash
# Naklonovat repozitář
git clone https://github.com/Honz12/fishing-cs-hackathon.git
cd fishing-cs-hackathon

# Spustit hru
dotnet run
```

### Ovládání

| Klávesa | Akce |
|---|---|
| ⬆ / ⬇ | Pohyb v menu |
| Enter / Mezerník | Potvrzení volby |
| Escape | Zpět / Přeskočit příběh / Ukončit rybaření |
| S | Prodat rybu v chladicím boxu |
| F1 | Otevřít debug konzoli |

---

## 🛠️ Debug konzole (F1)

Pro vývojáře a odvážné hráče:

```
>>> money 9999
>>> upgrade rod 5
>>> upgrade ship 3
>>> upgrade house 2
>>> help
>>> quit
```

---

## 📁 Struktura projektu

```
src/
├── Program.cs          — Hlavní smyčka hry, vykreslování, minihra
├── MainMenu.cs         — Hlavní menu
├── CommandProc.cs      — Debug konzole (F1)
├── Shop.cs             — Obchod a vylepšení
├── Inventory.cs        — Chladicí box
├── Fish.cs             — Třída ryby
├── Image.cs            — Načítání a vykreslování ANSI grafiky
├── data/
│   ├── TFish.cs        — Šablona ryby a enum FishRarity
│   ├── TFishFinder.cs  — Náhodný výběr ryby
│   └── FishData.cs     — Databáze všech ryb
└── images/
    ├── fish/            — 16×16 pixel art ryb (.txt)
    ├── rod/             — Obrázky prutů
    ├── ship/            — Obrázky lodí
    ├── characters/      — Obrázky postav (prodavači)
    └── houses/          — Obrázky obydlí
```

---

## 👥 Autoři

| Jméno | Role |
|---|---|
| **Honz12** 🧑‍💻 | Hlavní programátor — herní mechaniky, minihra, vykreslování |
| **matejalbert** ✍️ | Obchod, data ryb, texty, README, tutoriál a příběh |
| **sebastianjecny-green** 🎨 | Pixel art — ryby, pruty, lodě a další grafika |

---

## 📜 Licence

Tento projekt je licencován pod MIT licencí — podrobnosti viz soubor [LICENSE](LICENSE).

---

*"A teď už vím, kde ty ryby jsou!"* 🐟
