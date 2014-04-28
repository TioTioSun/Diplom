using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Diplom
{
    public class Camera
    {
        Vector3 position, target, up;
        float aspectRatio, nearClip, farClip;
        float rotationSpeed, translationSpeed, zoomSpeed;
        Matrix worldMatrix, viewMatrix, projectionMatrix;

        public float AspectRatio
        {
            get { return aspectRatio; }
            set 
            { 
                aspectRatio = value;
                projectionMatrix = Matrix.CreatePerspectiveFieldOfView(1, aspectRatio, nearClip, farClip);
            }
        }

        public Matrix WorldMatrix { get { return worldMatrix; } }
        public Matrix ViewMatrix { get { return viewMatrix; } }
        public Matrix ProjectionMatrix { get { return projectionMatrix; } }

        public Camera(float aspectRatio, Vector3 position, Vector3 target)
        {
            up = Vector3.Up;

            nearClip = 0.1f;
            farClip = 20000f;

            rotationSpeed = 0.01f;
            translationSpeed = 0.1f;
            zoomSpeed = 0.05f;

            this.aspectRatio = aspectRatio;
            this.position = position;
            this.target = target;

            worldMatrix = Matrix.Identity;
            viewMatrix = Matrix.CreateLookAt(position, target, up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(1, aspectRatio, nearClip, farClip);
        }

        public void RotateAroundTarget(float deltaX, float deltaY)
        {
            Vector3 right = Vector3.Normalize(Vector3.Cross(Vector3.Normalize(position - target), up));
            Quaternion qRotate = Quaternion.CreateFromAxisAngle(right, deltaY * rotationSpeed) * 
                                 Quaternion.CreateFromAxisAngle(up, -deltaX * rotationSpeed);
            position = Vector3.Transform(position - target, qRotate);

            Vector3 newRight = Vector3.Normalize(Vector3.Cross(Vector3.Normalize(position - target), up));
            if (Vector3.Dot(right, newRight) <= -0.94f) up *= -1;

            viewMatrix = Matrix.CreateLookAt(position, target, up);
        }

        public void MoveToTarget(float dist)
        {
            Vector3 direction = dist < 0 ? position - target : target - position;
            Matrix translationMatrix = Matrix.CreateTranslation(direction * zoomSpeed);
            position = Vector3.Transform(position - target, translationMatrix);
            viewMatrix = Matrix.CreateLookAt(position, target, up);
        }
    }
}
