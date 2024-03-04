﻿namespace CodeInject
{
    /// 0x08 Weapon
    /// 0x09 Shield
    /// 0xA - usable items potions etc
    /// 0x02 - hat
    /// 0x03 - chest armor
    /// 0x04 - gloves
    /// 0x05 - shoes
    /// 0x0C - Material
    public enum ItemType{
        Weapon = 0x08,
        Shield=0x09,
        UsableItem =0x0A,
        Hat = 0x02,
        ChestArmor = 0x03,
        Gloves = 0x04,
        Shoes = 0x05,
        Accesories = 0x07,
        Material = 0x0C,
        Mount = 0x0F,
        Gem = 0x0B
    }
}
