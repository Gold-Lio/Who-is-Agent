public enum ItemIDCode
{
    Package = 0,
    Bag,
    IDCard_NPC0 = 11,
    IDCard_NPC1,
    IDCard_NPC2,
    IDCard_NPC3,
    IDCard_NPC4,
    IDCard_NPC5,
    IDCard_NPC6,
    IDCard_NPC7,
    IDCard_NPC8,
    IDCard_NPC9,
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
    NPC0 = 0,
    NPC1,
    NPC2,
    NPC3,
    NPC4,
    NPC5,
    NPC6,
    NPC7,
    NPC8,
    NPC9,
}

public enum SlotType : byte
{
    Inventory = 0,
    Box,
    NPC,
    WeaponBox,
    Temp,
}