using UnityEngine;
using UnityEngine.SceneManagement; // Wajib untuk mengatur perpindahan scene

public class MenuManager : MonoBehaviour
{
    // Fungsi untuk tombol PLAY (Langsung masuk ke Level 1)
    public void PlayGame()
    {
        SceneManager.LoadScene("Lv 1");
    }

    // Fungsi untuk tombol SELECT LEVEL (Bisa disesuaikan nanti)
    public void BukaLevel(string namaLevel)
    {
        SceneManager.LoadScene(namaLevel);
    }

    // Fungsi untuk tombol EXIT
    public void ExitGame()
    {
        Debug.Log("Game Keluar!"); // Muncul di console saat testing
        Application.Quit(); // Akan menutup game saat sudah dibilik (build) (.exe)
    }
}