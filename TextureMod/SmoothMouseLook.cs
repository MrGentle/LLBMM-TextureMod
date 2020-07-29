using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TextureMod
{
    public class SmoothMouseLook : MonoBehaviour
    {
        public float speed = 3;
        private Vector3 moveDirection = Vector3.zero;
        public Vector3 movementMultiplier;

        public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
        public RotationAxes axes = RotationAxes.MouseXAndY;
        public float sensitivityX = 1F;
        public float sensitivityY = 1F;

        public float minimumX = -360F;
        public float maximumX = 360F;

        public float minimumY = -60F;
        public float maximumY = 60F;

        float rotationX = 0F;
        float rotationY = 0F;

        private List<float> rotArrayX = new List<float>();
        float rotAverageX = 0F;

        private List<float> rotArrayY = new List<float>();
        float rotAverageY = 0F;

        public float frameCounter = 20;

        Quaternion originalRotation;

        public bool isActive = false;

        void Update()
        {
            if (isActive)
            {

                moveDirection = new Vector3(movementMultiplier.x, movementMultiplier.y, movementMultiplier.z);
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= speed;

                movementMultiplier.x = 0;
                movementMultiplier.z = 0;
                movementMultiplier.y = 0;

                if (Input.GetKey(KeyCode.D))
                {
                    movementMultiplier.x += 1f;
                    movementMultiplier.z += 0;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    movementMultiplier.x += -1f;
                    movementMultiplier.z += 0;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    movementMultiplier.x += 0;
                    movementMultiplier.z += -1;
                }
                if (Input.GetKey(KeyCode.W))
                {
                    movementMultiplier.x += 0;
                    movementMultiplier.z += 1;
                }
                if (Input.GetKey(KeyCode.Q))
                {
                    movementMultiplier.y += 0.5f;
                }
                if (Input.GetKey(KeyCode.E))
                {
                    movementMultiplier.y -= 0.5f;
                }


                transform.position += moveDirection * Time.deltaTime;


                if (axes == RotationAxes.MouseXAndY)
                {
                    rotAverageY = 0f;
                    rotAverageX = 0f;

                    rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                    rotationX += Input.GetAxis("Mouse X") * sensitivityX;

                    rotArrayY.Add(rotationY);
                    rotArrayX.Add(rotationX);

                    if (rotArrayY.Count >= frameCounter)
                    {
                        rotArrayY.RemoveAt(0);
                    }
                    if (rotArrayX.Count >= frameCounter)
                    {
                        rotArrayX.RemoveAt(0);
                    }

                    for (int j = 0; j < rotArrayY.Count; j++)
                    {
                        rotAverageY += rotArrayY[j];
                    }
                    for (int i = 0; i < rotArrayX.Count; i++)
                    {
                        rotAverageX += rotArrayX[i];
                    }

                    rotAverageY /= rotArrayY.Count;
                    rotAverageX /= rotArrayX.Count;

                    rotAverageY = ClampAngle(rotAverageY, minimumY, maximumY);
                    rotAverageX = ClampAngle(rotAverageX, minimumX, maximumX);

                    Quaternion yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
                    Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);

                    transform.localRotation = originalRotation * xQuaternion * yQuaternion;
                }
                else if (axes == RotationAxes.MouseX)
                {
                    rotAverageX = 0f;

                    rotationX += Input.GetAxis("Mouse X") * sensitivityX;

                    rotArrayX.Add(rotationX);

                    if (rotArrayX.Count >= frameCounter)
                    {
                        rotArrayX.RemoveAt(0);
                    }
                    for (int i = 0; i < rotArrayX.Count; i++)
                    {
                        rotAverageX += rotArrayX[i];
                    }
                    rotAverageX /= rotArrayX.Count;

                    rotAverageX = ClampAngle(rotAverageX, minimumX, maximumX);

                    Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);
                    transform.localRotation = originalRotation * xQuaternion;
                }
                else
                {
                    rotAverageY = 0f;

                    rotationY += Input.GetAxis("Mouse Y") * sensitivityY;

                    rotArrayY.Add(rotationY);

                    if (rotArrayY.Count >= frameCounter)
                    {
                        rotArrayY.RemoveAt(0);
                    }
                    for (int j = 0; j < rotArrayY.Count; j++)
                    {
                        rotAverageY += rotArrayY[j];
                    }
                    rotAverageY /= rotArrayY.Count;

                    rotAverageY = ClampAngle(rotAverageY, minimumY, maximumY);

                    Quaternion yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
                    transform.localRotation = originalRotation * yQuaternion;
                }
            }
        }

        void Start()
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb)
                rb.freezeRotation = true;
            originalRotation = transform.localRotation;
        }

        public static float ClampAngle(float angle, float min, float max)
        {
            angle = angle % 360;
            if ((angle >= -360F) && (angle <= 360F))
            {
                if (angle < -360F)
                {
                    angle += 360F;
                }
                if (angle > 360F)
                {
                    angle -= 360F;
                }
            }
            return Mathf.Clamp(angle, min, max);
        }
    }
}