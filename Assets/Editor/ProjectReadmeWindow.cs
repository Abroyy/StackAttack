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
        GUILayout.Label(" STACK ATTACK - PROJE B�LG�LEND�RME", EditorStyles.boldLabel);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        GUILayout.Label(
@"Geli�tirici: Enes Aba  
Oyun Ad�: Stack Attack  
Motor: Unity 6000.0.58f2
Tarz: Dikey oynanan blok k�rma / stack t�r� arcade oyun  

---

### OYUN HAKKINDA
Stack Attack; oyuncunun yukar�dan a�a��ya do�ru inen bloklar� k�rarak ilerledi�i, refleks ve stratejiyi birle�tiren e�lenceli bir arcade oyunudur.  
Ama�, her segmentteki bloklar� projectile(mermi) ile yok ederek ilerlemek, �zel yetenekleri (�r. Frenzy, Ray) do�ru �ekilde kullanmak ve leveli tamamlamakt�r.

---

### NASIL �ALI�TIRILIR
1. Unity 6000.0.58f2 veya �zeri s�r�mle projeyi a��n.  
2. Ana sahne: `Assets/Scenes/MainMenu.unity`  
3. Play tu�una basarak oyunu ba�latabilirsiniz.  

> E�er referans hatalar� al�rsan�z:  
> Assets/ Reimport All yap�n veya eksik prefablar� kontrol edin.

---

### KONTROLLER
- Mouse veya Dokunma: Karakteri yatay eksende hareket ettirir.  
- At��: Otomatik olarak ger�ekle�ir, ancak baz� itemlar ate� h�z�n� veya �zel yetenekleri etkiler.  
- UI Butonlar�: Y�kseltmeler, duraklatma, men�ye d�n�� i�lemlerini kontrol eder.

---

### BLOK T�RLER�
- Normal Bloklar: Temel hedeflerdir, segmentlere g�re can de�erleri de�i�ir.  
- Frenzy Blok: K�sa s�reli�ine at�� h�z�n� art�r�r.  
- Boss Segment: �zel boss bar� ve farkl� can de�erlerine sahiptir.  

---

### PROJE YAPISI
- Scripts/  Oyun sistemleri, kontrol, flow y�netimi  
- Prefabs/  Bloklar, oyuncu, UI objeleri  
- Scenes/  Men� ve oyun sahneleri  
- Resources/  Level verileri, ScriptableObject�ler  
- Art/  Efektler ve UI elementleri  

---

### SES & M�Z�K
- At�� Sesleri:  Her projectile t�r�ne �zg� ses �al�n�r.  
- Buton SFX: T�m UI butonlar� t�klan�nca k�sa bir efekt �alar.  
- Arka Plan M�zi�i: Men�den itibaren s�rekli �alar.  

---

### OYUNUN GENEL MANTI�I ve NOTLAR

Stack Attack, oyuncunun bloklardan olu�an dikey segmentleri k�rarak yukar� do�ru ilerledi�i, refleks ve stratejiyi harmanlayan bir arcade oyunudur.

Her segment farkl� yap�dad�r:
- Bloklar�n can de�erleri, yo�unluklar� ve t�rleri segment baz�nda de�i�ir.
- Baz� segmentlerde �zel yetenek t�rleri (�rne�in Frenzy, Ray) bulunur.
- Segmentler aras�ndaki ge�i�ler FlowController scripti taraf�ndan otomatik y�netilir.

---

### BLOKLAR ve YETENEKLER
- Normal Bloklar: Hareketsiz blocklard�r. Oyuncu taraf�ndan vurulduk�a yok olur. Segmentin zorluk seviyesine g�re can de�erleri de�i�ir.  
- Dikey Hareketli Bloklar: Dikey hareketler yapan blocklard�r. Oyuncu taraf�ndan vurulduk�a yok olur. Segmentin zorluk seviyesine g�re can de�erleri de�i�ir.
- Yatay Hareketli Bloklar: Yatay hareketler yapan blocklard�r. Oyuncu taraf�ndan vurulduk�a yok olur. Segmentin zorluk seviyesine g�re can de�erleri de�i�ir.  
- Dairesel Hareketli Bloklar: Dairesel hareketler yapan blocklard�r. Oyuncu taraf�ndan vurulduk�a yok olur. Segmentin zorluk seviyesine g�re can de�erleri de�i�ir.  
- Frenzy Yetene�i: Oyuncuya k�sa s�reli ate� h�z� art��� sa�lar.  

---

### BOSS
- Boss Segment: Level sonunda gelir. Bu segmentte oyuncu, �zel bir Boss ile kar��la��r.  
  Boss�un sahneye girmesiyle birlikte BossHealthUI otomatik olarak aktifle�ir ve bossun can durumunu g�sterir.

---

### SAVA� ve YETENEKLER
- Oyuncu s�rekli ate� eder; baz� item�lar ate� h�z�n�, mermi say�s�n� veya hasar g�c�n� etkiler.  
- �Ability System� sayesinde oyuncu belirli aral�klarla rastgele y�kseltmeler (upgrade) se�er.
- Her y�kseltme, karakterin pasif veya aktif �zelliklerini de�i�tirir (�rne�in: �ift at��, z�plama, alan hasar� vb.).  
- Se�im ekran� a��ld���nda oyun duraklar, se�im yap�ld�ktan sonra oyun kald��� yerden devam eder.

---

### AMA�
Oyuncu, segmentleri temizleyerek ve boss�lar� yenerek sona ula�may� hedefler.  
Her tamamlanan level sonunda:
- Rastgele blueprint ve item �d�lleri kazan�l�r.  
- Yeni bir level otomatik olarak kilidi a��l�r (LevelSelectManager taraf�ndan).  

---

### S�STEMLER
- LevelData (ScriptableObject): Her level�in segment dizilimini, blok yap�lar�n� ve zorluk ayarlar�n� tutar.  
- FlowController: Oyunun ak���n�, segment ge�i�lerini ve blok spawn i�lemlerini y�netir.  
- InventoryManager: Oyuncunun sahip oldu�u item ve blueprint verilerini saklar.  
- AbilitySystem: Oyun s�ras�nda ��kan y�kseltme se�im ekranlar�n� kontrol eder.  
- LevelCompletedManager: Seviye biti�ini alg�lar, �d�lleri verir ve UI panelini a�ar.  
- PauseMenu: Oyunu duraklat�r, devam ettirir, yeniden ba�lat�r veya ana men�ye d�nd�r�r.  

---

### GENEL DENEY�M
Stack Attack, h�zl� refleks, dikkat ve strateji isteyen bir oyun.  
Her segmentte zorluk artar, oyuncu yeteneklerini ak�ll�ca se�meli ve kaynaklar�n� do�ru kullanmal�d�r.  
Ama� sadece hayatta kalmak de�il; ayn� zamanda sistemleri birbirine entegre �ekilde kullanarak y�ksek skor ve ilerleme sa�lamakt�r.

---

### �LET���M
Herhangi bir konuda ileti�im i�in:  
E-posta: enesaba.dev@gmail.com  
GitHub: github.com/Abroyy
", EditorStyles.wordWrappedLabel);

        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Kapat"))
            Close();

        bool showOnStartup = EditorPrefs.GetBool(SHOW_ON_STARTUP_KEY, true);
        bool toggle = GUILayout.Toggle(showOnStartup, " Ba�lang��ta g�ster");

        if (toggle != showOnStartup)
            EditorPrefs.SetBool(SHOW_ON_STARTUP_KEY, toggle);

        EditorGUILayout.EndHorizontal();
    }
}
