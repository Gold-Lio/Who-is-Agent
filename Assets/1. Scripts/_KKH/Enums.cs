
public enum CanvasType : byte
{
    Nick = 0,
    Lobby,
    Create
}

public enum ItemIDCode
{
    Package = 0,
    Bag,
    Goat_IDCard = 11,
    Raccoon_IDCard,
    Sloth_IDCard,
    Buffalo_IDCard,
    Monkey_IDCard,
    Rabbit_IDCard,
    Syringe = 21,
    Knife,
}

public enum ItemType
{
    Collection = 0,
    Weapon,
    IDCard,
}

public enum IDCard
{
    Goat = 0,
    Raccoon,
    Sloth,
    Buffalo,
    Monkey,
    Rabbit,
}

public enum SlotType : byte
{
    Inventory = 0,
    Box,
    NPC,
    WeaponBox,
    IDScanner,
    Temp,
}