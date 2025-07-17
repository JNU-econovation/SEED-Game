using System;
using UnityEngine;

[RequireComponent(typeof(InteractionTrigger))]
public class CardKey : MonoBehaviour
{
    [Header("회전 속도")]
    [SerializeField] private float rotateSpeed = 20f;

    [Header("위아래 움직임 설정")]
    [SerializeField] private float bounceSpeed = 5f;
    [SerializeField] private float bounceHeight = 0.05f;

    [Header("단서 정보")]
    public ClueInfos clueInfos;

    private Vector3 startPos;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

        float newY = Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.position = startPos + new Vector3(0, newY, 0);
    }
}