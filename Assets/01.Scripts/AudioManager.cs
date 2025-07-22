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
    [Header("Keypad & Gate SFX")]
    public AudioClip keypadFailSound;
    public AudioClip keypadSuccessSound;
    public AudioClip eastRoomOpenSound;
    public AudioClip gateCardSuccessSound;


    [Header("Player SFX")]
    public AudioClip[] playerAttackSounds;
    public AudioClip playerHitSound;
    public AudioClip playerDieSound;
    public AudioClip playerWalkSound;   // 걷기
    public AudioClip playerRunSound;    // 달리기
    public AudioClip playerRollSound;   // 구르기
    public AudioClip playerJumpSound;   // 점프

    [Header("Door SFX")]
    public AudioClip doorOpenSound;
    public AudioClip doorCloseSound;

    [Header("Monster SFX")]
    public AudioClip[] monsterAttackSounds;
    public AudioClip monsterHitSound;

    [Header("Boss Skills SFX")]
    public AudioClip bossSkill1Sound;
    public AudioClip bossSkill2Sound;

    private AudioSource bgmSource;
    private AudioSource sfxSource;
    private AudioSource stepSource; 
    [Header("Cutscene BGM")]
    public AudioClip endingTimelineBGM;

    [Range(0f, 1f)] public float bgmVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
            bgmSource.volume = bgmVolume;

            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.volume = sfxVolume;

            stepSource = gameObject.AddComponent<AudioSource>();
            stepSource.loop = true;
            stepSource.volume = sfxVolume;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // BGM
    public void PlayBGM(AudioClip clip = null)
    {
        if (clip == null)
            clip = defaultBGM;

        bgmSource.clip = clip;
        bgmSource.volume = bgmVolume;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void ChangeToBossBGM()
    {
        StopStep();
        sfxSource.Stop();
        PlayBGM(bossBGM);
    }

    // SFX
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip, sfxVolume);
    }
    // Keypad & Gate
    public void PlayKeypadFail()
    {
        PlaySFX(keypadFailSound);
    }

    public void PlayKeypadSuccess()
    {
        PlaySFX(keypadSuccessSound);
    }

    public void PlayEastRoomOpen()
    {
        PlaySFX(eastRoomOpenSound);
    }

    public void PlayGateCardSuccess()
    {
        PlaySFX(gateCardSuccessSound);
    }


    // Player
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

    public void PlayPlayerRoll()
    {
        PlaySFX(playerRollSound);
    }

    public void PlayPlayerJump()
    {
        PlaySFX(playerJumpSound);
    }

    public void PlayPlayerWalkLoop()
    {
        if (playerWalkSound == null) return;

        if (!stepSource.isPlaying || stepSource.clip != playerWalkSound)
        {
            stepSource.clip = playerWalkSound;
            stepSource.Play();
        }
    }

    public void PlayPlayerRunLoop()
    {
        if (playerRunSound == null) return;

        if (!stepSource.isPlaying || stepSource.clip != playerRunSound)
        {
            stepSource.clip = playerRunSound;
            stepSource.Play();
        }
    }

    public void StopStep()
    {
        if (stepSource.isPlaying)
            stepSource.Stop();
    }

    // Door
    public void PlayDoorOpen()
    {
        PlaySFX(doorOpenSound);
    }

    public void PlayDoorClose()
    {
        PlaySFX(doorCloseSound);
    }

    // Monster
    public void PlayMonsterAttack(int attackIndex)
    {
        if (attackIndex >= 0 && attackIndex < monsterAttackSounds.Length)
            PlaySFX(monsterAttackSounds[attackIndex]);
    }

    public void PlayMonsterHit()
    {
        PlaySFX(monsterHitSound);
    }

    // UI
    public void PlayUIOpen()
    {
        PlaySFX(uiOpenSound);
    }

    // Item & Clue
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

    // Boss Skills
    public void PlayBossSkill1()
    {
        PlaySFX(bossSkill1Sound);
    }

    public void PlayBossSkill2()
    {
        PlaySFX(bossSkill2Sound);
    }
    public void PlayEndingOnly()
    {
        StopStep();
        sfxSource.Stop(); 
        sfxSource.volume = 0f;
        stepSource.volume = 0f;

        PlayBGM(endingTimelineBGM); 
    }


    void Start()
    {
        PlayBGM();
    }
}
