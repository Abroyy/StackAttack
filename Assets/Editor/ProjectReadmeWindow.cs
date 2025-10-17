using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class StackAttackReadmeWindow : EditorWindow
{
    private Vector2 scrollPos;
    private const string SHOW_ON_STARTUP_KEY = "StackAttack_ShowReadmeOnStartup";

    static StackAttackReadmeWindow()
    {
        EditorApplication.update += ShowOnStartup;
    }

    [MenuItem("Tools/Proje Bilgilendirmesi")]
    public static void OpenWindow()
    {
        var window = GetWindow<StackAttackReadmeWindow>("Stack Attack Bilgilendirme");
        window.Show();
    }

    private static void ShowOnStartup()
    {
        EditorApplication.update -= ShowOnStartup;

        if (EditorPrefs.GetBool(SHOW_ON_STARTUP_KEY, true))
            OpenWindow();
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        GUILayout.Label(" STACK ATTACK - PROJE BÝLGÝLENDÝRME", EditorStyles.boldLabel);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        GUILayout.Label(
@"Geliþtirici: Enes Aba  
Oyun Adý: Stack Attack  
Motor: Unity 6000.0.58f2
Tarz: Dikey oynanan blok kýrma / stack türü arcade oyun  

---

### OYUN HAKKINDA
Stack Attack; oyuncunun yukarýdan aþaðýya doðru inen bloklarý kýrarak ilerlediði, refleks ve stratejiyi birleþtiren eðlenceli bir arcade oyunudur.  
Amaç, her segmentteki bloklarý projectile(mermi) ile yok ederek ilerlemek, özel yetenekleri (ör. Frenzy, Ray) doðru þekilde kullanmak ve leveli tamamlamaktýr.

---

### NASIL ÇALIÞTIRILIR
1. Unity 6000.0.58f2 veya üzeri sürümle projeyi açýn.  
2. Ana sahne: `Assets/Scenes/MainMenu.unity`  
3. Play tuþuna basarak oyunu baþlatabilirsiniz.  

> Eðer referans hatalarý alýrsanýz:  
> Assets/ Reimport All yapýn veya eksik prefablarý kontrol edin.

---

### KONTROLLER
- Mouse veya Dokunma: Karakteri yatay eksende hareket ettirir.  
- Atýþ: Otomatik olarak gerçekleþir, ancak bazý itemlar ateþ hýzýný veya özel yetenekleri etkiler.  
- UI Butonlarý: Yükseltmeler, duraklatma, menüye dönüþ iþlemlerini kontrol eder.

---

### BLOK TÜRLERÝ
- Normal Bloklar: Temel hedeflerdir, segmentlere göre can deðerleri deðiþir.  
- Frenzy Blok: Kýsa süreliðine atýþ hýzýný artýrýr.  
- Boss Segment: Özel boss barý ve farklý can deðerlerine sahiptir.  

---

### PROJE YAPISI
- Scripts/  Oyun sistemleri, kontrol, flow yönetimi  
- Prefabs/  Bloklar, oyuncu, UI objeleri  
- Scenes/  Menü ve oyun sahneleri  
- Resources/  Level verileri, ScriptableObject’ler  
- Art/  Efektler ve UI elementleri  

---

### SES & MÜZÝK
- Atýþ Sesleri:  Her projectile türüne özgü ses çalýnýr.  
- Buton SFX: Tüm UI butonlarý týklanýnca kýsa bir efekt çalar.  
- Arka Plan Müziði: Menüden itibaren sürekli çalar.  

---

### OYUNUN GENEL MANTIÐI ve NOTLAR

Stack Attack, oyuncunun bloklardan oluþan dikey segmentleri kýrarak yukarý doðru ilerlediði, refleks ve stratejiyi harmanlayan bir arcade oyunudur.

Her segment farklý yapýdadýr:
- Bloklarýn can deðerleri, yoðunluklarý ve türleri segment bazýnda deðiþir.
- Bazý segmentlerde özel yetenek türleri (örneðin Frenzy, Ray) bulunur.
- Segmentler arasýndaki geçiþler FlowController scripti tarafýndan otomatik yönetilir.

---

### BLOKLAR ve YETENEKLER
- Normal Bloklar: Hareketsiz blocklardýr. Oyuncu tarafýndan vuruldukça yok olur. Segmentin zorluk seviyesine göre can deðerleri deðiþir.  
- Dikey Hareketli Bloklar: Dikey hareketler yapan blocklardýr. Oyuncu tarafýndan vuruldukça yok olur. Segmentin zorluk seviyesine göre can deðerleri deðiþir.
- Yatay Hareketli Bloklar: Yatay hareketler yapan blocklardýr. Oyuncu tarafýndan vuruldukça yok olur. Segmentin zorluk seviyesine göre can deðerleri deðiþir.  
- Dairesel Hareketli Bloklar: Dairesel hareketler yapan blocklardýr. Oyuncu tarafýndan vuruldukça yok olur. Segmentin zorluk seviyesine göre can deðerleri deðiþir.  
- Frenzy Yeteneði: Oyuncuya kýsa süreli ateþ hýzý artýþý saðlar.  

---

### BOSS
- Boss Segment: Level sonunda gelir. Bu segmentte oyuncu, özel bir Boss ile karþýlaþýr.  
  Boss’un sahneye girmesiyle birlikte BossHealthUI otomatik olarak aktifleþir ve bossun can durumunu gösterir.

---

### SAVAÞ ve YETENEKLER
- Oyuncu sürekli ateþ eder; bazý item’lar ateþ hýzýný, mermi sayýsýný veya hasar gücünü etkiler.  
- “Ability System” sayesinde oyuncu belirli aralýklarla rastgele yükseltmeler (upgrade) seçer.
- Her yükseltme, karakterin pasif veya aktif özelliklerini deðiþtirir (örneðin: çift atýþ, zýplama, alan hasarý vb.).  
- Seçim ekraný açýldýðýnda oyun duraklar, seçim yapýldýktan sonra oyun kaldýðý yerden devam eder.

---

### AMAÇ
Oyuncu, segmentleri temizleyerek ve boss’larý yenerek sona ulaþmayý hedefler.  
Her tamamlanan level sonunda:
- Rastgele blueprint ve item ödülleri kazanýlýr.  
- Yeni bir level otomatik olarak kilidi açýlýr (LevelSelectManager tarafýndan).  

---

### SÝSTEMLER
- LevelData (ScriptableObject): Her level’in segment dizilimini, blok yapýlarýný ve zorluk ayarlarýný tutar.  
- FlowController: Oyunun akýþýný, segment geçiþlerini ve blok spawn iþlemlerini yönetir.  
- InventoryManager: Oyuncunun sahip olduðu item ve blueprint verilerini saklar.  
- AbilitySystem: Oyun sýrasýnda çýkan yükseltme seçim ekranlarýný kontrol eder.  
- LevelCompletedManager: Seviye bitiþini algýlar, ödülleri verir ve UI panelini açar.  
- PauseMenu: Oyunu duraklatýr, devam ettirir, yeniden baþlatýr veya ana menüye döndürür.  

---

### GENEL DENEYÝM
Stack Attack, hýzlý refleks, dikkat ve strateji isteyen bir oyun.  
Her segmentte zorluk artar, oyuncu yeteneklerini akýllýca seçmeli ve kaynaklarýný doðru kullanmalýdýr.  
Amaç sadece hayatta kalmak deðil; ayný zamanda sistemleri birbirine entegre þekilde kullanarak yüksek skor ve ilerleme saðlamaktýr.

---

### ÝLETÝÞÝM
Herhangi bir konuda iletiþim için:  
E-posta: enesaba.dev@gmail.com  
GitHub: github.com/Abroyy
", EditorStyles.wordWrappedLabel);

        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Kapat"))
            Close();

        bool showOnStartup = EditorPrefs.GetBool(SHOW_ON_STARTUP_KEY, true);
        bool toggle = GUILayout.Toggle(showOnStartup, " Baþlangýçta göster");

        if (toggle != showOnStartup)
            EditorPrefs.SetBool(SHOW_ON_STARTUP_KEY, toggle);

        EditorGUILayout.EndHorizontal();
    }
}
