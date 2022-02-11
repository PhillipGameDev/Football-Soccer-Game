using UnityEngine;

[CreateAssetMenu(fileName = "Key", menuName = "Tzolkin/Key", order = 0)]
public class Key : ScriptableObject 
{
    public enum Type { Air, Fire, Earth, Rock, Water }

    [SerializeField] private Type type;
    [SerializeField] private Sprite sprite;

    public Type KeyType { get => type; }
    public Sprite Sprite { get => sprite; }
}