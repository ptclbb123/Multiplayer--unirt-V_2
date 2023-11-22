using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject viewPoint;
    [SerializeField] private float mouseSensitivity = 1f;
    [SerializeField] private float verticalRotationStore;
    private Vector2 mouseInput;

    // Start is called before the first frame update
    private void Start()
    {
        viewPoint = this.transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    private void Update()
    {
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;
        this.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);
        verticalRotationStore += mouseInput.y;
        verticalRotationStore = Mathf.Clamp(verticalRotationStore, -60f, 60f);
        viewPoint.transform.rotation = Quaternion.Euler(verticalRotationStore, viewPoint.transform.rotation.eulerAngles.y, viewPoint.transform.rotation.eulerAngles.z);
    }
}