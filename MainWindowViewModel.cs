using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using HomeWorkFinal;

namespace HomeworkFinal
{
    public partial class MainWindowViewModel
    {
        private IList<Model3D> _selectedModels;

        public CubeVisual3D _current { get; set; }
        public MyCubes3D _myCubes3D;

        public MainWindowViewModel(Viewport3D viewport)
        {
            _myCubes3D = new MyCubes3D();
            this.PointSelectionCommand = new PointSelectionCommand(viewport, this.HandleSelectionEvent);
        }


        public PointSelectionCommand PointSelectionCommand { get; private set; }

        public SelectionHitMode SelectionMode
        {
            get
            {
                return this.PointSelectionCommand.SelectionHitMode;
            }

            set
            {
                this.PointSelectionCommand.SelectionHitMode = value;
            }
        }

        private void HandleSelectionEvent(object sender, ModelsSelectedEventArgs args)
        {
            if (args.SelectedModels.Count == 0) return;
            this._selectedModels = args.SelectedModels;
            var pointSelectionArgs = args as ModelsSelectedByPointEventArgs;
            if (pointSelectionArgs != null)
            {



                this.Select(this._selectedModels.First());

            }
            else return;
        
        }

        private void Select(Model3D model)
        {
            var geometryModel = model as GeometryModel3D;
            if (geometryModel != null)
            {
                _current = _myCubes3D.IsExist(model);
            }
        
            
        }
    }
}
