using Code.Gameplay.AI.Behaviours;
using UnityEngine;

namespace Code.Gameplay.AI.Services
{
    public class BotRespawnZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            TryRespawn(other.gameObject);
        }

        private void OnCollisionEnter(Collision other)
        {
            TryRespawn(other.gameObject);
        }

        private static void TryRespawn(GameObject target)
        {
            if (target == null || !target.TryGetComponent(out AICharacter character))
                return;

            GameAIContext.Instance?.SpawnService.TryRespawn(character);
        }
    }
}
