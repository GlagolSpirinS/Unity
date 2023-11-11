using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKitHandler : MonoBehaviour
{
    //������� ��� ������� �������
    public ParticleSystem destroyParticles;

    //���� ��� ������� �������
    public AudioSource healingSound;

    //���-�� ������������ ����� ������
    public int HealthPointCount;

    private GameManager _GameManager;

    private void Start()
    {
        _GameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {

    }

    //���������� ������� ��� ��������������� �������� � ������� ������������
    private void OnTriggerEnter(Collider other)
    {
        //������� ������������ ������, ����� ���� ������� � ���� �� ��������� ������ � ���
        destroyParticles.transform.parent = null;
        healingSound.transform.parent = null;
        //����������� ������� � ����
        destroyParticles.Play();
        healingSound.Play();
        //���������� �������
        Destroy(gameObject);
        //����� ������
        _GameManager.Healing(HealthPointCount);
    }
}
