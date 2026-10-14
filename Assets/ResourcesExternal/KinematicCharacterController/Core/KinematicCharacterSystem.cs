using System.Collections.Generic;
using UnityEngine;

namespace KinematicCharacterController
{
    public class KinematicCharacterSystem : MonoBehaviour
    {
        public static bool AutoSimulation = true;

        private static KinematicCharacterSystem _instance;
        private static readonly List<KinematicCharacterMotor> CharacterMotors = new List<KinematicCharacterMotor>();

        public static void EnsureCreation()
        {
            if (_instance != null)
                return;

            var systemObject = new GameObject(nameof(KinematicCharacterSystem));
            _instance = systemObject.AddComponent<KinematicCharacterSystem>();
            DontDestroyOnLoad(systemObject);
        }

        public static void RegisterCharacterMotor(KinematicCharacterMotor motor)
        {
            if (motor == null || CharacterMotors.Contains(motor))
                return;

            CharacterMotors.Add(motor);
        }

        public static void UnregisterCharacterMotor(KinematicCharacterMotor motor)
        {
            CharacterMotors.Remove(motor);
        }

        private void FixedUpdate()
        {
            if (!AutoSimulation)
                return;

            Simulate(Time.deltaTime);
        }

        public static void Simulate(float deltaTime)
        {
            for (var i = 0; i < CharacterMotors.Count; i++)
            {
                var motor = CharacterMotors[i];
                if (motor == null || !motor.enabled || motor.CharacterController == null)
                    continue;

                motor.UpdatePhase1(deltaTime);
            }

            for (var i = 0; i < CharacterMotors.Count; i++)
            {
                var motor = CharacterMotors[i];
                if (motor == null || !motor.enabled || motor.CharacterController == null)
                    continue;

                motor.UpdatePhase2(deltaTime);
                motor.Transform.SetPositionAndRotation(motor.TransientPosition, motor.TransientRotation);
            }
        }
    }
}
