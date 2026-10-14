using System.Collections.Generic;
using Code.Gameplay.AI.Behaviours;
using UnityEngine;

namespace Code.Gameplay.AI.Services
{
    public class BotTickService : IBotTickService
    {
        private readonly List<AICharacter> _characters = new List<AICharacter>();

        public void Register(AICharacter character)
        {
            if (character == null || _characters.Contains(character))
                return;

            _characters.Add(character);
        }

        public void Unregister(AICharacter character)
        {
            if (character == null)
                return;

            _characters.Remove(character);
        }

        public void Clear() => _characters.Clear();

        public void Tick()
        {
            float deltaTime = Time.deltaTime;

            for (int i = 0; i < _characters.Count; i++)
            {
                AICharacter character = _characters[i];

                if (character != null && character.IsActive)
                    character.Tick(deltaTime);
            }
        }
    }
}
