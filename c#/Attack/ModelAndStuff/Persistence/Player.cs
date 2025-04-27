using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ModelAndStuff.Persistence
{
    public interface IFieldElement
    {   bool IsPlayer { get; }
        public bool IsLeftPlayer
        {
            get { return false; }

        }
        int X { get; set; }
        int Y { get; set; }
        public int Id
        {
            get;

        }
        public bool IsAlive
        {
            get;
            set;
        }
    }
    public class Field : IFieldElement
    {
        private int x;
        private int y;
        public bool IsPlayer { get { return false; } }
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        public int Id
        {
            get { return 0; }

        }
        public bool IsAlive
        {
            get { return false; }
            set { }
        }
    }
    public class Player : IFieldElement
    {
        private int _id;
        private bool _isAlive;
        private bool _isLeftPlayer;
        private int x;
        private int y;
        public bool IsPlayer { get { return true; } }
        public bool IsLeftPlayer
        {
            get { return _isLeftPlayer; }
           
        }
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        public int Id
        {
            get { return _id; }
            
        }
        public bool IsAlive
        {
            get { return _isAlive; }
            set { _isAlive = value; }
        }
        public Player(int id, bool IsLeftPlayer)
        {
            _id = id;
            _isAlive = true;
            _isLeftPlayer = IsLeftPlayer;
        }
    }
}
