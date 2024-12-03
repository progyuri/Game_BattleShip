using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace Battleship
{
  internal class MapVM : ViewModelBase
  {
    static Random rnd = new Random();
    CellVM[,] map; // y, x

    public List<ShipVM> Ships { get; } = new List<ShipVM>();
    public CellVM this[int x, int y] { get { return map[y, x]; } }
    public List<List<CellVM>> Map { 
      get {
        var viewMap = new List<List<CellVM>>();
        for (int y = 0; y < 10; y++) {
          viewMap.Add( new List<CellVM>());
          for (int x = 0; x < 10; x++) {
            viewMap[y].Add(this[x,y]);
          }
        }
        return viewMap;
      } 
    }

    internal void SetShips(params ShipVM[] ships) {
      foreach (var ship in ships) {
        Ships.Add(ship);
        var (x, y) = ship.Pos;
        var rang = ship.Rang;
        var dir = ship.Direct;
        if (dir == DirectionShip.Horisont)
          for (int j = x; j < x + rang; j++)
            this[j, y].ToShip();
        else
          for (int i = y; i < y + rang; i++)
            this[x, i].ToShip();
      }
    }

    public MapVM(string str) : this() {
      var mp = str.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
      for (int i = 0; i < 10; i++) {
        for (int j = 0; j < 10; j++) {
          if (mp[i][j] == 'X')
            map[i,j].ToShip();
        }
      }
    }

    public MapVM() {
      map = new CellVM[10, 10];
      for (int i = 0; i < 10; i++) {
        for (int j = 0; j < 10; j++) {
          map[i, j] = new CellVM();
        }
      }
    }

    // Растановка кораблей на карте
    // FillMap(0,4,3,2,1);
    private List<Ship> fillMap(List<Ship> ships, params int[] navy) {

      var p = navy.Length - 1;

      while (p > 0 && navy[p] == 0) p--;

      if (p < 1) {
        return ships;
      }
      else {
        var ship = new Ship();
        ship.Rang = p;
        navy[p]--;
        int k = 0;
        while (k<10) {
          if (rnd.Next(2) == 0) {
            ship.Dir = DirectionShip.Horisont;
            ship.X = rnd.Next(10 - p);
            ship.Y = rnd.Next(10);
          }
          else {
            ship.Dir = DirectionShip.Vertical;
            ship.X = rnd.Next(10);
            ship.Y = rnd.Next(10 - p);
          }
          // проверяем не пересекается ли корабль с другими используется Linq метод
          if (ships.All(other => !ship.Croos(ref other))) {
            ships.Add(ship);
            var res = fillMap(ships, navy);
            if (res != null)
              return res;
            ships.RemoveAt(ships.Count - 1);
          }
          k++;
        }
        navy[p]++;
      }
      return null;
    }


    // Метод создания кораблей компьютера на карте
    public void FillMap(params int[] navy) {
      List<Ship> ships = null;

      while (ships == null) {
        ships = fillMap(new List<Ship>(), navy);
      }

      foreach (var ship in ships) {
        if (ship.Dir == DirectionShip.Horisont) {
          for (int x = ship.X; x < ship.X + ship.Rang; x++) {
            this[x, ship.Y].ToShip();
          }
        }
        else {
          for (int y = ship.Y; y < ship.Y + ship.Rang; y++) {
            this[ship.X, y].ToShip();
          }
        }
      }

      Ships.Clear();
      foreach (var ship in ships) {
        Ships.Add(new ShipVM(ship));
      }
    }

    internal struct Ship
    {
      public int X = 0, Y = 0, Rang = 1;
      public DirectionShip Dir = DirectionShip.Horisont ;

      public Ship(int x, int y, int rang, DirectionShip dir) {
        X = x; Y = y; Rang = rang; Dir = dir;
      }


      //Метод проверки на пересечение других кораблей
      public bool Croos(ref Ship other) {
        int x1 = X - 1, y1 = Y - 1, x2 = X, y2 = Y;
        if (Dir == DirectionShip.Horisont)
            x2 = x1 + Rang + 1;
        else
            y2 = y1 + Rang + 1;

        int ox1 = other.X - 1, oy1 = other.Y - 1, ox2 = other.X, oy2 = other.Y;
        if (other.Dir == DirectionShip.Horisont)
            ox2 = ox1 + other.Rang + 1;
        else
            oy2 = oy1 + other.Rang + 1;

        return !(x1 > ox2 || x2 < ox1 || y1 > oy2 || y2 < oy1);
      }
      public override string ToString() {
        return $"x:{X} y:{Y} R:{Rang}{(Dir == DirectionShip.Horisont ? '-' : '|')}";
      }
    }
  }
}
