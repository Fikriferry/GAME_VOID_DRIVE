using UnityEngine;

public class mobil : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentBrakeForce;
    private bool isBraking;

    [Header("Settings")]
    [SerializeField] private float motorForce = 1500f;
    [SerializeField] private float brakeForce = 3000f;
    [SerializeField] private float maxSteerAngle = 30f;
    [SerializeField] private float decelerationForce = 500f; // Rem otomatis saat tidak gas

    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [Header("Wheel Meshes")]
    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    // Variabel internal untuk menampung status tombol HP
    private bool mobileGas = false;
    private bool mobileBrake = false;
    private bool mobileLeft = false;
    private bool mobileRight = false;

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        // --- 1. INPUT WINDOWS / PC (Keyboard) ---
        float keyboardH = Input.GetAxis("Horizontal");  // Tombol A/D atau Panah Kanan/Kiri
        float keyboardV = Input.GetAxis("Vertical");    // Tombol W/S atau Panah Atas/Bawah
        bool keyboardBrake = Input.GetKey(KeyCode.Space); // Tombol Spasi untuk rem

        // --- 2. INPUT HP (Tombol UI) ---
        float mobileH = 0f;
        if (mobileRight) mobileH += 1f;
        if (mobileLeft) mobileH -= 1f;

        float mobileV = 0f;
        if (mobileGas) mobileV += 1f;

        // --- 3. GABUNGKAN INPUT ---
        // Mathf.Clamp digunakan agar nilainya tidak melebihi angka 1 atau -1 jika PC & HP ditekan bersamaan
        horizontalInput = Mathf.Clamp(keyboardH + mobileH, -1f, 1f);
        verticalInput = Mathf.Clamp(keyboardV + mobileV, -1f, 1f);

        // Rem aktif jika Spasi di PC ditekan ATAU Tombol Rem di HP ditahan
        isBraking = keyboardBrake || mobileBrake;
    }

    // --- FUNGSI PUBLIC UNTUK DIHUBUNGKAN KE TOMBOL UI HP ---
    public void SetGas(bool state)
    {
        mobileGas = state;
    }

    public void SetBrake(bool state)
    {
        mobileBrake = state;
    }

    public void SetLeft(bool state)
    {
        mobileLeft = state;
    }

    public void SetRight(bool state)
    {
        mobileRight = state;
    }

    private void HandleMotor()
    {
        // Front Wheel Drive
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;

        // Logika pengereman
        if (isBraking)
        {
            currentBrakeForce = brakeForce;
        }
        else if (Mathf.Abs(verticalInput) < 0.1f)
        {
            // Saat tidak menekan gas sama sekali
            currentBrakeForce = decelerationForce;
        }
        else
        {
            currentBrakeForce = 0f;
        }

        ApplyBraking();
    }

    private void ApplyBraking()
    {
        frontLeftWheelCollider.brakeTorque = currentBrakeForce;
        frontRightWheelCollider.brakeTorque = currentBrakeForce;
        rearLeftWheelCollider.brakeTorque = currentBrakeForce;
        rearRightWheelCollider.brakeTorque = currentBrakeForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;

        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateWheelPos(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateWheelPos(frontRightWheelCollider, frontRightWheelTransform);
        UpdateWheelPos(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateWheelPos(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void UpdateWheelPos(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;

        wheelCollider.GetWorldPose(out pos, out rot);

        // Sesuaikan rotasi ban biar nggak miring
        wheelTransform.rotation = rot * Quaternion.Euler(0, 0, 90f);

        // Posisi ban mengikuti collider
        wheelTransform.position = pos;
    }
}