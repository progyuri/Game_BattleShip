using System;
using System.Windows.Threading;

namespace Battleship
{
  class BattleshipVM : ViewModelBase {
    DispatcherTimer timer;
    DateTime startTime;
    string time = "";

   
    public MapVM OurMap { get; private set; } 
    public MapVM EnemyMap { get; private set; }
    public string Time { 
      get => time; 
      private set => Set(ref time, value); 
    }
    public BattleshipVM() {
      timer = new DispatcherTimer();
      timer.Interval = TimeSpan.FromMilliseconds(100);
      timer.Tick += Timer_Tick;

        // создание карты и расстановка своих кораблей
      OurMap = new MapVM();
        // Rang - палубность,   Pos - координаты, Direct - направление
            OurMap.SetShips(
        new ShipVM { Rang = 4, Pos = (1, 1) },
        new ShipVM { Rang = 3, Pos = (6, 1), Direct = DirectionShip.Vertical, },
        new ShipVM { Rang = 3, Pos = (8, 1), Direct = DirectionShip.Vertical, },
        new ShipVM { Rang = 2, Pos = (1, 3), },
        new ShipVM { Rang = 2, Pos = (1, 5), },
        new ShipVM { Rang = 2, Pos = (4, 3), Direct = DirectionShip.Vertical, },
        new ShipVM { Rang = 1, Pos = (1, 9), },
        new ShipVM { Rang = 1, Pos = (2, 7), },
        new ShipVM { Rang = 1, Pos = (4, 7), },
        new ShipVM { Rang = 1, Pos = (8, 9), }
        );

      // cоздание карты 
      EnemyMap = new MapVM();
      // расстановка кораблей компьютера 
      // передаем кол-во палуб в зависимости от индекса
      EnemyMap.FillMap(0,4,3,2,1);
    }

      // выстрел в нашу карту
    internal void ShotToOurMap(int x, int y) {
      OurMap[x, y].ToShot();
    }

     // кол-во времени прошедшее с начала игры
    private void Timer_Tick(object? sender, EventArgs e) {
      var now = DateTime.Now;
      var dt = now - startTime;
      Time = dt.ToString(@"mm\:ss");
    }
     
     // запуск таймера
    public void Start() {
      startTime = DateTime.Now;
      timer.Start();
    }


    public void Stop() {
      timer.Stop();
    }
  }
}
