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
using System.Drawing;
using Renderer3D.Engine;
using System.Windows.Threading;

namespace Renderer3D
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    /// [System.Runtime.InteropServices.DllImport("gdi32.dll")]
   
    public partial class MainWindow : Window
    {
        Viewport testViewport;
        Camera camera;
        Controls controls;
        Scene myScene;

        public MainWindow()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(16);
            timer.Tick += Update;
            timer.Start();

            testViewport = new Viewport((int)ViewportWindow.Width, (int)ViewportWindow.Height);
            ViewportWindow.Source = testViewport.ShowViewport();


            controls = new Controls();

            myScene = new Scene();

            SceneObject cube = new SceneObject(0, 0, 1);
            cube.mesh = Mesh.CreateCube();
            cube.material = new Material(255, 0, 0);
            myScene.AddObjectToScene(cube);

            SceneObject cube2 = new SceneObject(6, 0, 3);
            cube2.mesh = Mesh.CreateCube();
            myScene.AddObjectToScene(cube2);

            SceneObject cube3 = new SceneObject(-1.05f, 0, 1);
            cube3.mesh = Mesh.CreateCube();
            myScene.AddObjectToScene(cube3);

            SceneObject sphere1 = new SceneObject(0, 0, -10);
            sphere1.mesh = Mesh.LoadFromObj(@"Assets\icosphere.obj");
            sphere1.material = new Material(0, 255, 0);
            myScene.AddObjectToScene(sphere1);

            Light light1 = new Light(0, 0, 0, new Vector3(0, 0, -1));
            myScene.AddObjectToScene(light1);

            camera = new Camera(testViewport, myScene);
            
            
        }

        void Update(object sender, EventArgs e)
        {
            
            camera.Render();
            myScene.MoveScene(controls.GetControls());
            myScene.RotateScene(controls.GetRotation());
        }

        private void InputKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Up)
            {
                
                controls.SetVertical(-1f);
            }
            if (e.Key == Key.Down)
            {
                controls.SetVertical(1f);
            }
            if (e.Key == Key.Right)
            {
                controls.SetHorizontal(-1f);
            }
            if (e.Key == Key.Left)
            {
                controls.SetHorizontal(1f);
            }
            if(e.Key == Key.D1)
            {
                testViewport.RenderMode(1);
            }
            if (e.Key == Key.D2)
            {
                testViewport.RenderMode(2);
            }
            if (e.Key == Key.D3)
            {
                testViewport.RenderMode(3);
            }
            if (e.Key == Key.A)
            {
                controls.SetRotation(1.0f);
            }
            if (e.Key == Key.D)
            {
                controls.SetRotation(-1.0f);
            }

        }

        private void InputKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.Down)
            {

                controls.SetVertical(0f);
            }
            if (e.Key == Key.Right || e.Key == Key.Left)
            {
                controls.SetHorizontal(0f);
            }

            if (e.Key == Key.A || e.Key == Key.D)
            {
                controls.SetRotation(0f);
            }
            

        }
    }
}
