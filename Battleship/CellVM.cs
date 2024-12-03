using System;
using System.Windows;

namespace Battleship
{
  class CellVM : ViewModelBase
  {
    static Random rnd = new Random();

    public int Angle { get; } = rnd.Next(-5, 5);
    public int AngleX { get; } = rnd.Next(-5, 5);
    public int AngleY { get; } = rnd.Next(-5, 5);

    public float ScaleX { get; } = 1 + rnd.Next(-15, 3) / 100.0f;
    public float ScaleY { get; } = 1 + rnd.Next(-15, 3) / 100.0f;
    public float ShiftX { get; } = rnd.Next(-20, 20) / 10.0f;
    public float ShiftY { get; } = rnd.Next(-20, 20) / 10.0f;

    bool ship, shot, IsShipVisibility;
        
    private bool isShot;

        public CellVM(char state = '*') {
        ship = state == 'X';
    }

    public Visibility Miss => 
      shot && !ship ? Visibility.Visible : Visibility.Collapsed; 

    public Visibility Shot => 
      shot && ship ? Visibility.Visible : Visibility.Collapsed;

        /*
      public Visibility IsShipVisibility =>
      shot && ship ?Visibility.Visible : Visibility.Collapsed;
        */
      public void ToShot() {
      shot = true;
      Notify("Miss", "Shot");
    }

    public void ToShip() {
      ship = true;
    }

    public override string ToString() {
      if (ship && shot) return "X";
      if (ship && !shot) return "#";
      if (!ship && shot) return "*";
      if (!ship && !shot) return " ";
      return "";
    }

    public bool IsShot
    {
    get => isShot;
    private set => Set(ref isShot, value);

    }
    public void MarkShot()
    {
        IsShot = true;
    }
  }
}
