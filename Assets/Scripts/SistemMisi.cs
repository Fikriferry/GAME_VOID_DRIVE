using UnityEngine;
using TMPro;

public class SistemMisi : MonoBehaviour
{
    [Header("Pengaturan Paket 1")]
    public GameObject markerAmbil1;
    public GameObject markerAntar1;

    [Header("Pengaturan Paket 2 (Untuk Level 2)")]
    public bool iniLevel2 = false; // Centang ini di Inspector khusus untuk Level 2
    public GameObject markerAmbil2;
    public GameObject markerAntar2;

    [Header("Pengaturan Waktu Total (Detik)")]
    public float waktuMisi = 60f; // Karena langsung lanjut, beri waktu total yang cukup untuk semua paket
    private float sisaWaktu;

    [Header("Pengaturan UI")]
    public TextMeshProUGUI teksTimer;

    private bool misiDimulai = false;
    private int tahapanMisi = 0; 
    // tahapanMisi: 
    // 0 = Belum ambil paket 1
    // 1 = Sedang antar paket 1
    // 2 = Sedang pergi ambil paket 2
    // 3 = Sedang antar paket 2

    void Start()
    {
        // Kondisi Awal: Aktifkan tempat ambil paket 1
        markerAmbil1.SetActive(true);
        markerAntar1.SetActive(false);
        
        if (iniLevel2)
        {
            markerAmbil2.SetActive(false);
            markerAntar2.SetActive(false);
        }

        teksTimer.gameObject.SetActive(false);
    }

    void Update()
    {
        if (misiDimulai)
        {
            if (sisaWaktu > 0)
            {
                sisaWaktu -= Time.deltaTime;
                UpdateTampilanTimer(sisaWaktu);
            }
            else
            {
                SisaWaktuHabis();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Menabrak: " + other.gameObject.name);

        // --- LOGIKA PAKET 1 ---
        // 1. Ambil Paket 1 -> Timer Mulai Berhitung Mundur
        if (tahapanMisi == 0 && other.gameObject.name == markerAmbil1.name)
        {
            tahapanMisi = 1;
            misiDimulai = true;
            sisaWaktu = waktuMisi; // Waktu total dimulai dari sini

            markerAmbil1.SetActive(false);
            markerAntar1.SetActive(true);
            teksTimer.gameObject.SetActive(true);
            Debug.Log("Paket 1 Diambil! Waktu total dimulai.");
        }
        // 2. Antar Paket 1
        else if (tahapanMisi == 1 && other.gameObject.name == markerAntar1.name)
        {
            markerAntar1.SetActive(false);

            if (iniLevel2)
            {
                // Jika Level 2, ganti target ke Ambil Paket 2, tapi TIMER TETAP JALAN (TIDAK BERHENTI)
                tahapanMisi = 2;
                markerAmbil2.SetActive(true); 
                Debug.Log("Paket 1 Sukses! Timer lanjut terus, cepat ambil Paket 2!");
            }
            else
            {
                // Jika Level 1, selesai
                misiDimulai = false;
                teksTimer.text = "LEVEL 1 SELESAI!";
                Debug.Log("Selamat! Level 1 Selesai.");
            }
        }

        // --- LOGIKA PAKET 2 (KHUSUS LEVEL 2) ---
        // 3. Ambil Paket 2 -> Timer tidak di-reset, langsung lanjut menggunakan sisa waktu yang ada
        else if (iniLevel2 && tahapanMisi == 2 && other.gameObject.name == markerAmbil2.name)
        {
            tahapanMisi = 3;
            markerAmbil2.SetActive(false);
            markerAntar2.SetActive(true); // Ganti ke lingkaran tempat antar terakhir
            Debug.Log("Paket 2 Diambil! Sisa waktu terus berjalan, cepat antar!");
        }
        // 4. Antar Paket 2 -> Finish Level 2
        else if (iniLevel2 && tahapanMisi == 3 && other.gameObject.name == markerAntar2.name)
        {
            misiDimulai = false;
            markerAntar2.SetActive(false);
            teksTimer.text = "LEVEL 2 SELESAI!";
            Debug.Log("Luar Biasa! Semua paket di Level 2 berhasil diantar berturut-turut!");
        }
    }

    void UpdateTampilanTimer(float waktu)
    {
        float menit = Mathf.FloorToInt(waktu / 60);
        float detik = Mathf.FloorToInt(waktu % 60);
        teksTimer.text = string.Format("{0:00}:{1:00}", menit, detik);
    }

    void SisaWaktuHabis()
    {
        misiDimulai = false;
        teksTimer.text = "GAME OVER!";
        Debug.Log("Waktu habis! Anda gagal.");
    }
}