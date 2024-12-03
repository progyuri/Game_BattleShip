using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Battleship
{
  public partial class MainWindow : Window
  {
    BattleshipVM bs = new BattleshipVM();
    Random rnd = new Random();
    public MainWindow() {
      DataContext = bs;
      InitializeComponent();
    }

    private void Border_MouseDown(object sender, MouseButtonEventArgs e) {
      var brd = sender as Border;
      var cellVM = brd.DataContext as CellVM;
      cellVM.ToShot();
      var x = rnd.Next(10);
      var y = rnd.Next(10);

        // Ход компьютера
        //int x, y;
        //do
        //{
        //x = rnd.Next(10);
        //y = rnd.Next(10);
        //} while (bs.IsShot(x, y)); // проверка, была ли уже сделана попытка выстрела по этим координатам

      // Выстрел по нашей карте
      bs.ShotToOurMap(x, y);
    }
  }
}
