using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    /*
    public Rigidbody playerRigidbody;
    public Camera fpsCam;

    float MoveSpeed = 5.0f;
    float rotSpeed = 3.0f;
    float currentRot = 0f;

    // Update is called once per frame
    void Update()
    {
        Move();
        RotCtrl();
    }

    void Move()
    {
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        float xSpeed = xInput * MoveSpeed;
        float zSpeed = zInput * MoveSpeed;

        transform.Translate(Vector3.forward * zSpeed * Time.deltaTime);
        transform.Translate(Vector3.right * xSpeed * Time.deltaTime);
    }

    void RotCtrl()
    {
        float rotX = Input.GetAxis("Mouse Y") * rotSpeed;
        float rotY = Input.GetAxis("Mouse X") * rotSpeed;

        // ���콺 ����
        currentRot -= rotX;

        // ���콺�� Ư�� ������ �Ѿ�� �ʰ� ����ó��
        currentRot = Mathf.Clamp(currentRot, -80f, 80f);

        // Camera�� Player�� �ڽ��̹Ƿ� �÷��̾��� Y�� ȸ���� Camera���Ե� �Ȱ��� �����
        this.transform.localRotation *= Quaternion.Euler(0, rotY, 0);
        // Camera�� transform ������Ʈ�� ���÷����̼��� ���Ϸ����� 
        // ����X�� �����̼��� ��Ÿ���� ���Ϸ����� �Ҵ����ش�.
        fpsCam.transform.localEulerAngles = new Vector3(currentRot, 0f, 0f);
    }
    */
    
    public CharacterController SelectPlayer; // ������ ĳ���� ��Ʈ�ѷ�
    public Camera fpsCam;
    public float Speed = 5.0f;  // �̵��ӵ�
    public float JumpPow = 5.0f;

    private float Gravity = 10.0f; // �߷�   
    private Vector3 MoveDir = Vector3.zero; // ĳ������ �����̴� ����.
    private bool JumpButtonPressed = false;  //  ���� ���� ��ư ���� ����
    private bool FlyingMode = false;  // ��۶��̴� ��忩��

    float rotSpeed = 3.0f;
    float currentRot = 0f;

    public AudioClip footStepSound;
    public float footStepDelay;
    private float nextFootstep = 0;

    // Update is called once per frame
    void Update()
    {
        Move();
        RotCtrl();
        if (Input.GetKeyDown(KeyCode.E))
        {

            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("2Fstair"))
                {
                    SceneManager.LoadScene("2Floor");
                }
                else if (hit.collider.CompareTag("JouhyunRoom"))
                {
                    SceneManager.LoadScene("JouhyunRoom");
                }    
              
                    Debug.Log(hit.transform.gameObject.name);
            }
        }
    }

    private void Move()
    {
        if (SelectPlayer == null) return;
        // ĳ���Ͱ� �ٴڿ� �پ� �ִ� ��츸 �۵��մϴ�.
        // ĳ���Ͱ� �ٴڿ� �پ� ���� �ʴٸ� �ٴ����� �߶��ϰ� �ִ� ���̹Ƿ�
        // �ٴ� �߶� ���߿��� ���� ��ȯ�� �� �� ���� �����Դϴ�.
        if (SelectPlayer.isGrounded)
        {
            // Ű���忡 ���� X, Z �� �̵������� ���� �����մϴ�.
            MoveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            // ������Ʈ�� �ٶ󺸴� �չ������� �̵������� ������ �����մϴ�.
            MoveDir = SelectPlayer.transform.TransformDirection(MoveDir);
            // �ӵ��� ���ؼ� �����մϴ�.
            MoveDir *= Speed;

            // �����̽� ��ư�� ���� ���� : ���� ������ư�� �������� �ʾҴ� ��츸 �۵�
            if (JumpButtonPressed == false && Input.GetButton("Jump"))
            {
                JumpButtonPressed = true;
                MoveDir.y = JumpPow;
            }
        }
        // ĳ���Ͱ� �ٴڿ� �پ� ���� �ʴٸ�
        else
        {
            // �ϰ��߿� �����̽� ��ư�� ������ ���ο� ���ϸ�� �ߵ�!
            if (MoveDir.y < 0 && JumpButtonPressed == false && Input.GetButton("Jump"))
            {
                FlyingMode = true;
            }

            if (FlyingMode)
            {
                JumpButtonPressed = true;

                // �߷� ��ġ�� �����մϴ�.
                MoveDir.y *= 0.95f;

                // ������ �ϴÿ��� ������ �ִ� ���� �������� �ʰ� �ϱ� ����
                // �ּ� �ʴ� -1�� �ϰ� �ӵ��� �����մϴ�.
                if (MoveDir.y > -1) MoveDir.y = -1;

                // ���� �� ���� ������ȯ�� �����մϴ�.
                MoveDir.x = Input.GetAxis("Horizontal");
                MoveDir.z = Input.GetAxis("Vertical");
            }
            else
                // �߷��� ������ �޾� �Ʒ������� �ϰ��մϴ�.           
                MoveDir.y -= Gravity * Time.deltaTime;
        }

        // ������ư�� �������� ���� ���
        if (!Input.GetButton("Jump"))
        {
            JumpButtonPressed = false;  // �������� ��ư ���� ���� ����
            FlyingMode = false;         // ��۶��̴� ��� ����
        }
        // �� �ܰ������ ĳ���Ͱ� �̵��� ���⸸ �����Ͽ�����,
        // ���� ĳ������ �̵��� ���⼭ ����մϴ�.
        SelectPlayer.Move(MoveDir * Time.deltaTime);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) && !JumpButtonPressed)
        {
            nextFootstep -= Time.deltaTime;
            if (nextFootstep <= 0)
            {
                GetComponent<AudioSource>().PlayOneShot(footStepSound, 0.7f);
                nextFootstep += footStepDelay;
            }
        }
}
    void RotCtrl()
    {
        float rotX = Input.GetAxis("Mouse Y") * rotSpeed;
        float rotY = Input.GetAxis("Mouse X") * rotSpeed;

        // ���콺 ����
        currentRot -= rotX;

        // ���콺�� Ư�� ������ �Ѿ�� �ʰ� ����ó��
        currentRot = Mathf.Clamp(currentRot, -80f, 80f);

        // Camera�� Player�� �ڽ��̹Ƿ� �÷��̾��� Y�� ȸ���� Camera���Ե� �Ȱ��� �����
        this.transform.localRotation *= Quaternion.Euler(0, rotY, 0);
        // Camera�� transform ������Ʈ�� ���÷����̼��� ���Ϸ����� 
        // ����X�� �����̼��� ��Ÿ���� ���Ϸ����� �Ҵ����ش�.
        fpsCam.transform.localEulerAngles = new Vector3(currentRot, 0f, 0f);
    }
}