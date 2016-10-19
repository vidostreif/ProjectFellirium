using UnityEngine;
using System.Collections;

//создание экземпляра частицы
public class SpecialEffectsHelper : MonoBehaviour
{
    // Синглтон
    public static SpecialEffectsHelper Instance;

    public ParticleSystem smokeEffect;
    public ParticleSystem fireEffect;

    void Awake()
    {
        // регистрация синглтона
        if (Instance != null)
        {
            Debug.LogError("Несколько экземпляров SpecialEffectsHelper!");
        }

        Instance = this;
    }

    // Создать взрыв в данной точке
    public void Explosion(Vector3 position)
    {
        // Дым над водой
        instantiate(smokeEffect, position);

        // да-даам

        // Огонь в небе
        instantiate(fireEffect, position);
    }

    // Создание экземпляра системы частиц из префаба
    private ParticleSystem instantiate(ParticleSystem prefab, Vector3 position)
    {
        ParticleSystem newParticleSystem = Instantiate(
          prefab,
          position,
          Quaternion.identity
        ) as ParticleSystem;

        // Убедитесь, что это будет уничтожено
        Destroy(
          newParticleSystem.gameObject,
          newParticleSystem.startLifetime
        );

        return newParticleSystem;
    }
}
