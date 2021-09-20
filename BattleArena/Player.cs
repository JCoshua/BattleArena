using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BattleArena
{
    class Player:Entity
    {
        private Item[] _items;
        private Item _currentItem;
        private int _currentItemIndex;
        private string _job;

        public override float AttackPower
        {
            get
            {
                if (_currentItem.Type == ItemType.ATTACK)
                    return base.AttackPower + CurrentItem.StatBoost;

                return base.AttackPower;
            }
           
        }

        public override float DefensePower
        {
            get
            {
                if (_currentItem.Type == ItemType.DEFENSE)
                    return base.DefensePower + CurrentItem.StatBoost;

                return base.DefensePower;
            }

        }


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

            _currentItemIndex = index;

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

            _currentItemIndex = -1;
            _currentItem = new Item();
            _currentItem.Name = "Nothing";

            return true;
        }

        /// <returns>The names of all items in the player inventory</returns>
        public string[] GetItemNames()
        {
            string[] itemNames = new string[_items.Length];

            for (int i = 0; i < _items.Length; i ++)
            {
                itemNames[i] = _items[i].Name;
            }

            return itemNames;
        }

        public string Job
        {
            get
            {
                return _job;
            }
            set
            {
                Job = value;
            }
        }

        public Player()
        {
            _items = new Item[0];
            _currentItem.Name = "nothing";
        }

        public Player(Item[] items) : base()
        {
            _currentItem.Name = "Nothing";
            _items = items;
            _currentItemIndex = -1;
        }

        public Player(string name, float health, float attackPower, float defensePower, Item[] items, string job) : base(name, health, attackPower, defensePower)
        {
            _items = items;
            _currentItem.Name = "nothing";
            _job = job;
        }

        public override void Save(StreamWriter writer)
        {
            writer.WriteLine(_job);
            base.Save(writer);
            writer.WriteLine(_currentItemIndex);
            
        }

        public override bool Load(StreamReader reader)
        {
           // If the base Load Function fails
            if (!base.Load(reader))
                return false;

            // If the current line can't be converted into an int
            if (!int.TryParse(reader.ReadLine(), out _currentItemIndex))
                return false;

            //Reutrn whether or not the item was equipped successfully
            return TryEquipItem(_currentItemIndex);
        }
    }
}
