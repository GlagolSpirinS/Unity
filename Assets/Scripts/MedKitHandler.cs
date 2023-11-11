using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKitHandler : MonoBehaviour
{
    //Частицы при подборе аптечки
    public ParticleSystem destroyParticles;

    //Звук при подборе аптечки
    public AudioSource healingSound;

    //кол-во восполняемых очков жизней
    public int HealthPointCount;

    private GameManager _GameManager;

    private void Start()
    {
        _GameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {

    }

    //Обработчик события при соприкосновение триггера с другими коллайдерами
    private void OnTriggerEnter(Collider other)
    {
        //Убираем родительский объект, чтобы наши частицы и звук не удалились вместе с ним
        destroyParticles.transform.parent = null;
        healingSound.transform.parent = null;
        //Проигрываем частицы и звук
        destroyParticles.Play();
        healingSound.Play();
        //Уничтожаем аптечку
        Destroy(gameObject);
        //Лечим игрока
        _GameManager.Healing(HealthPointCount);
    }
}
