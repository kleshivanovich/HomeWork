using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace HomeworkFinal
{
    public class MyCubes3D
    {
        public IList<CubeVisual3D> Container;

        public MyCubes3D()
        {
            Container=new List<CubeVisual3D>();
        }

        public void Add(CubeVisual3D item)
        {
            Container.Add(item);
        }

        public CubeVisual3D IsExist(Model3D model)
        {

            var gmodel = model as GeometryModel3D;
            foreach (CubeVisual3D cube in Container)
            {
                if (cube.Model.Equals(model))
                {
                    return cube;
                }
            }
            return new CubeVisual3D();
        }

        public void Delete(CubeVisual3D item)
        {
            Container.Remove(item);
        }

    }
}