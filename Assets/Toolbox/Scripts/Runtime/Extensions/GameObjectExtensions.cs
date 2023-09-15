using UnityEngine;

namespace Toolbox.Extensions
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Returns component if found otherwise adds it to the game object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject"></param>
        /// <returns>Component</returns>
        public static T GetOrAdd<T>(this GameObject gameObject) where T : Component
        {
            var component = gameObject.GetComponent<T>();
            return component ? component : gameObject.AddComponent<T>();
        }
    }
}
