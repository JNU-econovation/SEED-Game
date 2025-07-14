using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("BGM Clips")]
    public AudioClip defaultBGM;
    public AudioClip bossBGM;

    [Header("UI SFX")]
    public AudioClip uiOpenSound;

    [Header("Item & Clue SFX")]
    public AudioClip clueGetSound;
    public AudioClip weaponGetSound;
    public AudioClip clueBoxOpenSound;
    public AudioClip clueBoxCloseSound;
    public AudioClip clueCombineSound;

    [Header("Player SFX")]
    public AudioClip[] playerAttackSounds; // 무기별
    public AudioClip playerHitSound;
    public AudioClip playerDieSound;

    [Header("Monster SFX")]
    public AudioClip[] monsterAttackSounds; // 몬스터/스킬별
    public AudioClip monsterHitSound;

    [Header("Boss Skills SFX")]
    public AudioClip bossSkill1Sound;
    public AudioClip bossSkill2Sound;

    private AudioSource bgmSource;
    private AudioSource sfxSource;

    [Range(0f, 1f)] public float bgmVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // BGM AudioSource
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
            bgmSource.volume = bgmVolume;

            // SFX AudioSource
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.volume = sfxVolume;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 🎵 BGM 관련
    public void PlayBGM(AudioClip clip = null)
    {
        if (clip == null)
            clip = defaultBGM;

        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void ChangeToBossBGM()
    {
        PlayBGM(bossBGM);
    }

    // 🔊 공통 SFX
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip, sfxVolume);
    }

    // 🗡️ 플레이어 관련
    public void PlayPlayerAttack(int weaponIndex)
    {
        if (weaponIndex >= 0 && weaponIndex < playerAttackSounds.Length)
            PlaySFX(playerAttackSounds[weaponIndex]);
    }

    public void PlayPlayerHit()
    {
        PlaySFX(playerHitSound);
    }

    public void PlayPlayerDie()
    {
        PlaySFX(playerDieSound);
    }

    // 👾 몬스터 관련
    public void PlayMonsterAttack(int attackIndex)
    {
        if (attackIndex >= 0 && attackIndex < monsterAttackSounds.Length)
            PlaySFX(monsterAttackSounds[attackIndex]);
    }

    public void PlayMonsterHit()
    {
        PlaySFX(monsterHitSound);
    }

    // 🧩 UI
    public void PlayUIOpen()
    {
        PlaySFX(uiOpenSound);
    }

    // 💡 아이템 & 단서
    public void PlayClueGet()
    {
        PlaySFX(clueGetSound);
    }

    public void PlayWeaponGet()
    {
        PlaySFX(weaponGetSound);
    }

    public void PlayClueBoxOpen()
    {
        PlaySFX(clueBoxOpenSound);
    }

    public void PlayClueBoxClose()
    {
        PlaySFX(clueBoxCloseSound);
    }

    public void PlayClueCombine()
    {
        PlaySFX(clueCombineSound);
    }

    // 👾 보스 스킬
    public void PlayBossSkill1()
    {
        PlaySFX(bossSkill1Sound);
    }

    public void PlayBossSkill2()
    {
        PlaySFX(bossSkill2Sound);
    }
    void Start()
    {
        PlayBGM();
    }

}
