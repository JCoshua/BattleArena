using System;
using System.Collections.Generic;
using System.Text;

namespace BattleArena
{
    class Player:Entity
    {
        private Item[] _items;
        private Item _currentItem;

        public Item CurrentItem
        {
            get
            {
                return _currentItem;
            }
        }

        public Player(string name, float health, float attackPower, float defensePower, Item[] items) : base(name, health, attackPower, defensePower)
        {
            _items = items;
            _currentItem.Name = "Nothing";
        }

        /// <summary>
        /// Sets the item given at the current index
        /// </summary>
        /// <param name="index">The index of the item in the array</param>
        /// <returns>False if outside the bounds of the array</returns>
        public bool TryEquipItem (int index)
        {
            //If the index is out of bounds
            if (index >= _items.Length || index < 0)
                return false;

            //Sets the current item 
            _currentItem = _items[index];
                return true;
        }

        /// <summary>
        /// Unequips the current item
        /// </summary>
        /// <returns>false if there is no item</returns>
        public bool TryUnequip()
        {
            if (CurrentItem.Name == "Nothing")
                return false;

            _currentItem = new Item();
            _currentItem.Name = "Nothing";

            return true;
        }
    }
}
