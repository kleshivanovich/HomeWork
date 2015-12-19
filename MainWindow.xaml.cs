using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Xaml;
using HelixToolkit.Wpf;
using HomeworkFinal;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

namespace HomeWorkFinal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
      
        
        private double _x, _y, _z, _sizeOfCube;
        private System.Drawing.Color _Col;
        private ImageBrush _Solid;
        private bool _flagOfColor;
        private bool _flagOfSolid;
        private MainWindowViewModel _viewModel;
        //public CubeVisual3D current;
        public MainWindow()
        {
            InitializeComponent();
            CrBut.IsEnabled = false;
            _sizeOfCube = 1;
            DelBut.IsEnabled = false;
            Txp.IsEnabled = false;
            Txm.IsEnabled = false;
            Typ.IsEnabled = false;
            Tym.IsEnabled = false;
            Tzp.IsEnabled = false;
            Tzm.IsEnabled = false;

            this.InitializeComponent();
            _viewModel = new MainWindowViewModel(this.myView.Viewport);
            this.DataContext = _viewModel;
            this.myView.InputBindings.Add(new MouseBinding(_viewModel.PointSelectionCommand, new MouseGesture(MouseAction.LeftDoubleClick)));
            if (myView.Children.Count == 3) DelBut.IsEnabled = false;
        }


       
        private void ChooseColor(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.ShowDialog();
            this._Col = colorDialog.Color;
            ColRec.Fill = new SolidColorBrush(Color.FromArgb(this._Col.A, this._Col.R, this._Col.G, this._Col.B));
            _flagOfColor = true;
            _flagOfSolid = false;
            
        }

        private void CreateClick(object sender, RoutedEventArgs e)
        {
            try
            {

                Txp.IsEnabled = true;
                Txm.IsEnabled = true;
                Typ.IsEnabled = true;
                Tym.IsEnabled = true;
                Tzp.IsEnabled = true;
                Tzm.IsEnabled = true;

                _x = double.Parse(Tx.Text);
                _y = double.Parse(Ty.Text);
                _z = double.Parse(Tz.Text);

                if ((Tx.Text.Count() != 0) && (Ty.Text.Count() != 0) && (Tz.Text.Count() != 0))
                {

                    CubeVisual3D c1 = new CubeVisual3D();
                    c1.SideLength = _sizeOfCube;
                    c1.Center = new Point3D(this._x, this._y, this._z);
                    c1.Visible = true;
                    if (_flagOfColor)
                        c1.Fill = new SolidColorBrush(Color.FromArgb(this._Col.A, this._Col.R, this._Col.G, this._Col.B));
                    if (_flagOfSolid)
                        c1.Fill = _Solid;
                    _viewModel._current = c1;
                    _viewModel._myCubes3D.Add(c1);
                    model.Children.Add(c1);
                    DelBut.IsEnabled = true;

                }
            }
            catch (FormatException ex)
            {
                
            }
        }



        private void Tx_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if ((Tx.Text == "") && (Ty.Text == "") && (Tz.Text == ""))
                CrBut.IsEnabled = false;
            else CrBut.IsEnabled = true;
        }


        private void UpdateSelected(object sender, RoutedEventArgs e)
        {
            
        }

        private void ChangeSizeClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Lines.Length = Lines.Width = double.Parse(SizeBox.Text);
                _sizeOfCube = double.Parse(SizeCube.Text);

                CubeVisual3D cur = _viewModel._myCubes3D.IsExist(_viewModel._current.Model);

                _x = double.Parse(Tx.Text);
                _y = double.Parse(Ty.Text);
                _z = double.Parse(Tz.Text);

               _viewModel._current.SideLength = _sizeOfCube;
               _viewModel._current.Center = new Point3D(this._x, this._y, this._z);
                if (_flagOfColor)
                    _viewModel._current.Fill = new SolidColorBrush(Color.FromArgb(this._Col.A, this._Col.R, this._Col.G, this._Col.B));
                if (_flagOfSolid)
                    _viewModel._current.Fill = _Solid;

                cur.SideLength = _sizeOfCube;
                cur.Center = new Point3D(this._x, this._y, this._z);
                if (_flagOfColor)
                    cur.Fill = new SolidColorBrush(Color.FromArgb(this._Col.A, this._Col.R, this._Col.G, this._Col.B));
                if (_flagOfSolid)
                    cur.Fill = _Solid;


            }
            catch (FormatException ex)
            {
            }

        }



        private void Solid_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.ShowDialog();
            if (of.FileName != "")
            {
                PathToSolid.Text = of.FileName;
                ImageBrush sol = new ImageBrush(new BitmapImage(new Uri(PathToSolid.Text)));
                SolRec.Fill = _Solid = sol;
                _flagOfColor = false;
                _flagOfSolid = true;
            }
        }



        private void Enable_lines_Click(object sender, RoutedEventArgs e)
        {
            var value = Lines.Visible;
            if (value) value = false; else value = true;
            Lines.Visible = value;
        }

        private void DelBut_Click(object sender, RoutedEventArgs e)
        {
            if (model.Children.Count == 1) DelBut.IsEnabled = false;
            model.Children.Remove(_viewModel._current);
            _viewModel._myCubes3D.Delete(_viewModel._current);
            GeometryModel3D _temp_mod=new GeometryModel3D();
            if (model.Children.Count <= 1) return;
            model.Children[model.Children.Count - 1].GetTransformTo(_temp_mod);
            _viewModel._current =_viewModel._myCubes3D.IsExist(_temp_mod);
        }


        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.DefaultExt = "xaml";
            sf.ShowDialog();
            if (sf.FileName.Length != 0)
            {
                Viewport3D viewport = this.myView.Viewport;
                object obj = viewport;

                FileStream fs = new FileStream(sf.FileName, FileMode.Create, FileAccess.Write);
                string savedview = System.Windows.Markup.XamlWriter.Save(obj);

                byte[] byts = System.Text.ASCIIEncoding.ASCII.GetBytes(savedview);

                fs.Write(byts, 0, byts.Length);

                fs.Close();
            }
            
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.DefaultExt = "xaml";
            of.ShowDialog();
            if (of.FileName.Length != 0)
            {
                FileStream fs = new FileStream(of.FileName, FileMode.Open, FileAccess.Read);
                Viewport3D vp = System.Windows.Markup.XamlReader.Load(fs) as Viewport3D;
                List<Visual3D> items = new List<Visual3D>();
                foreach (Visual3D visual in vp.Children)
                {
                    items.Add(visual);
                }
                vp.Children.Clear();
                foreach (var item in items)
                {
                    myView.Children.Add(item);
                }
            }
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            myView.CameraMode = CameraMode.Inspect;
        }

        private void ToggleButton_OnChecked1(object sender, RoutedEventArgs e)
        {
            myView.CameraMode = CameraMode.WalkAround;
        }

        private void X_plus_clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                _x = double.Parse(Tx.Text);
                _x += 0.5;
                Tx.Text = _x.ToString();
                this.ChangeSizeClick(sender, e);
            }
            catch (FormatException ex) { }
        }

        private void Y_plus_clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                _y = double.Parse(Ty.Text);
                _y += 0.5;
                Ty.Text = _y.ToString();
                this.ChangeSizeClick(sender, e);

            }
            catch (FormatException ex)
            {
            }
        }

        private void Z_plus_clicked(object sender, RoutedEventArgs e)
        {
            try
            {
            _z = double.Parse(Tz.Text);
            _z += 0.5;
            Tz.Text = _z.ToString();
            this.ChangeSizeClick(sender, e);
             }
            catch (FormatException ex)
            {
            }
        }

        private void X_minus_clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                _x = double.Parse(Tx.Text);
                _x -= 0.5;
                Tx.Text = _x.ToString();
                this.ChangeSizeClick(sender, e);
            }
            catch (FormatException ex)
            {
            }
        }

        private void Y_minus_clicked(object sender, RoutedEventArgs e)
        {
            try
            {
              _y = double.Parse(Ty.Text);
              _y -= 0.5;
              Ty.Text = _y.ToString();
              this.ChangeSizeClick(sender, e);
            }
            catch (FormatException ex){}
        }

        private void Z_minus_clicked(object sender, RoutedEventArgs e)
        {
             try
            {
            _z = double.Parse(Tz.Text);
            _z -= 0.5;
            Tz.Text = _z.ToString();
            this.ChangeSizeClick(sender, e);
            }
             catch (FormatException ex) { }
        }

        private void MyView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Tx.Text = _viewModel._current.Center.X.ToString();
            Ty.Text = _viewModel._current.Center.Y.ToString();
            Tz.Text = _viewModel._current.Center.Z.ToString();
            SizeCube.Text = _viewModel._current.SideLength.ToString();
            ColRec.Fill = _viewModel._current.Fill;

        }

    }
}



