using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using System;
using OpenTK.Mathematics;
using LearnOpenTK.Common;

namespace ProyekUTS
{
    class Window : GameWindow
    {
        List<Asset3d> Kota = new List<Asset3d>();
        List<Asset3d> character = new List<Asset3d>();
        List<Asset3d> ironman = new List<Asset3d>();
        List<Asset3d> Api = new List<Asset3d>();
        List<Asset3d> nuklir = new List<Asset3d>();

        Camera _camera;

        //untuk tilt kamera
        bool _firstmove = true;
        Vector2 _lastPos;
        float timelompat = 0.1f;
        float speed = 1f;
        float speednuklir = 1f;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {

        }

        protected override void OnLoad()
        {
            base.OnLoad();

            //warna bg nya sky blue: rgb(135,206,235)
            //GL.ClearColor(0.529f, 0.807f, 0.921f, 1.0f);

            //deepsky blue: rgb(0,191,255)
            //GL.ClearColor(0, 0.749f, 1, 1.0f);

            //royal blue: rgb(65,105,225)
            GL.ClearColor(0.254f, 0.411f, 1, 1.0f);

            GL.Enable(EnableCap.DepthTest);

            //camera
            _camera = new Camera(new Vector3(0, 1, 5), Size.X / Size.Y);

            //menangkap kursor
            CursorGrabbed = true;

            

            


          
            #region Base

            var tutupbawah = new Asset3d(new Vector3(0.737f, 0.2f, 0.101f));
            tutupbawah.createHalfEllipsoid(3.0f, -3.0f, 3.0f, 0.5f, 0f, -1.0f,360,360);
            Kota.Add(tutupbawah);

            var gedung1 = new Asset3d(new Vector3(0.713f, 0.745f, 0.745f));
            gedung1.createBoxVertices(0.5f, 1.7f, -2.5f, 0.5f, 1.5f, 3.5f, 1.5f);
            Kota.Add(gedung1);
            var kacagedung1 = new Asset3d(new Vector3(0.203f, 0.972f, 0.976f));
            kacagedung1.createBoxVertices(0.5f, 1.6f, -2.49f, 0.3f, 1.3f, 3.4f, 1.5f);
            gedung1.child.Add(kacagedung1);
            var alas1 = new Asset3d(new Vector3(0.713f, 0.745f, 0.745f));
            alas1.createBoxVertices(0.5f, 1f, -2.48f, 0.5f, 1.5f, 0.1f, 1.5f);
            gedung1.child.Add(alas1);
            var alas2 = new Asset3d(new Vector3(0.713f, 0.745f, 0.745f));
            alas2.createBoxVertices(0.5f, 2.2f, -2.48f, 0.5f, 1.5f, 0.1f, 1.5f);
            gedung1.child.Add(alas2);
            var sekat1 = new Asset3d(new Vector3(0.713f, 0.745f, 0.745f));
            sekat1.createBoxVertices(0.5f, 1.6f, -2.48f, 0.5f, 0.1f, 3.4f, 1.5f);
            gedung1.child.Add(sekat1);
            var antena1 = new Asset3d(new Vector3(0f, 0f, 0f));
            antena1.createCurveBezier((0, 0.5f, -3), (0, 1f, -2), (0, 1.5f, -1));
            gedung1.child.Add((antena1));


            var gedung2 = new Asset3d(new Vector3(0.713f, 0.745f, 0.745f));
            gedung2.createBoxVertices(-1.3f, 1.2f, -2.0f, 1.5f, 1.5f, 2.5f, 1.0f);
            Kota.Add(gedung2);

            var kacagedung2 = new Asset3d(new Vector3(0.203f, 0.972f, 0.976f));
            kacagedung2.createBoxVertices(-1.3f, 1.1f, -1.99f, 1.5f, 1.3f, 2.5f, 1.0f);
            gedung2.child.Add(kacagedung2);

            var alas11 = new Asset3d(new Vector3(0.713f, 0.745f, 0.745f));
            alas11.createBoxVertices(-1.3f, 1.2f, -1.96f, 1.5f, 1.5f, 0.1f, 1.0f);
            gedung2.child.Add(alas11);
            var sekat2 = new Asset3d(new Vector3(0.713f, 0.745f, 0.745f));
            sekat2.createBoxVertices(-1.3f, 1.1f, -1.98f, 1.5f, 0.1f, 2.4f, 1f);
            gedung2.child.Add(sekat2);

            var gedung3 = new Asset3d(new Vector3(0.713f, 0.745f, 0.745f));
            gedung3.createBoxVertices(2.3f, 1.5f, -2.0f, 1.5f, 1.5f, 3.0f, 1.0f);
            Kota.Add(gedung3);
            var kacagedung3 = new Asset3d(new Vector3(0.203f, 0.972f, 0.976f));
            kacagedung3.createBoxVertices(2.3f, 1.4f, -1.99f, 1.5f, 1.3f, 3.0f, 1.0f);
            gedung3.child.Add(kacagedung3);
            var alas13 = new Asset3d(new Vector3(0.713f, 0.745f, 0.745f));
            alas13.createBoxVertices(2.3f, 1.5f, -1.96f, 1.5f, 1.5f, 0.1f, 1.0f);
            gedung3.child.Add(alas13);
            var sekat3 = new Asset3d(new Vector3(0.713f, 0.745f, 0.745f));
            sekat3.createBoxVertices(2.3f, 1.4f, -1.98f, 1.5f, 0.1f, 2.9f, 1.0f);
            gedung3.child.Add(sekat3);
            var jalan = new Asset3d(new Vector3(0.713f, 0.745f, 0.745f));
            jalan.createBoxVertices(0.5f, 0f, -0.1f, 1.5f, 5f, 0.1f, 1.4f);
            tutupbawah.child.Add(jalan);
            var garisjalan = new Asset3d(new Vector3(1f, 1f, 0f));
            garisjalan.createBoxVertices(0.5f, 0.01f, -0.1f, 1.5f, 5f, 0.1f, 0.3f);
            tutupbawah.child.Add(garisjalan);
            var sekatgaris1 = new Asset3d(new Vector3(0.713f, 0.745f, 0.745f));
            sekatgaris1.createBoxVertices(-0.5f, 0.01f, -0.1f, 1.5f, 0.3f, 0.11f, 1.4f);
            garisjalan.child.Add(sekatgaris1);
            var sekatgaris2 = new Asset3d(new Vector3(0.713f, 0.745f, 0.745f));
            sekatgaris2.createBoxVertices(1.2f, 0.01f, -0.1f, 1.5f, 0.3f, 0.11f, 1.4f);
            garisjalan.child.Add(sekatgaris2);

            var elip1 = new Asset3d(new Vector3());
            elip1.createEllipsoid(0f, 0.5f, -7.5f, 2.0f, -2.0f, 0.0f, 72, 24);
            elip1.rotate(Vector3.Zero, Vector3.UnitX, 90f);
            Kota.Add(elip1);

            var elip2 = new Asset3d(new Vector3(0.603f, 0.921f, 0.976f));
            elip2.createEllipsoid(0f, 0.5f, -7.6f, 3.0f, -3.0f, 0.0f, 72, 24);
            elip2.rotate(Vector3.Zero, Vector3.UnitX, 90f);
            Kota.Add(elip2);

            var badannuklir = new Asset3d(new Vector3(0.941f, 0.807f, 0.258f));
            badannuklir.createCylinder(0.4f, 0.7f, 0.0f, 6f, 0.0f);
            nuklir.Add(badannuklir);

            var atasnuklir = new Asset3d(new Vector3(0.941f, 0.807f, 0.258f));
            atasnuklir.createHalfEllipsoid(0.4f, 0.2f, 0.4f, 0.0f, 6.35f, 0.0f, 100, 100);
            nuklir.Add(atasnuklir);

            var bawahnuklir = new Asset3d(new Vector3(0.941f, 0.807f, 0.258f));
            bawahnuklir.createHalfEllipsoid(0.4f, -0.2f, 0.4f, 0.0f, 5.65f, 0.0f, 100, 100);
            nuklir.Add(bawahnuklir);

            var bola = new Asset3d(new Vector3(0f, 0f, 0f));
            bola.createEllipsoid(0f, 6f, 0.4f, 0.05f, 0.05f, 0.05f, 70, 70);
            nuklir.Add(bola);



            #endregion
            #region charironman
            var badanironman = new Asset3d(new Vector3(1.0f, 0.0f, 0.0f));
            badanironman.createCylinder(0.25f, 0.8f, 1.5f, 2.7f, 0.0f);
            ironman.Add(badanironman);

            var bokong = new Asset3d(new Vector3(1.0f, 0.0f, 0.0f));
            bokong.createHalfEllipsoid(0.25f, -0.15f, 0.25f, 1.5f, 2.3f, 0.0f,72,24);
            badanironman.child.Add(bokong);

            var kepala = new Asset3d(new Vector3(1.0f, 0.0f, 0.0f));
            kepala.createHalfEllipsoid(0.25f, 0.15f, 0.25f, 1.5f, 3.1f, 0.0f,72,24);
            badanironman.child.Add(kepala);

            var matakiri = new Asset3d(new Vector3(0.203f, 0.972f, 0.976f));
            matakiri.createHalfEllipsoid(0.05f, -0.025f, 0.015f, 1.425f, 3.0f, 0.3f,72,24);
            badanironman.child.Add(matakiri);

            var matakanan = new Asset3d(new Vector3(0.203f, 0.972f, 0.976f));
            matakanan.createHalfEllipsoid(0.05f, -0.025f, 0.015f, 1.575f, 3.0f, 0.3f,72,24);
            badanironman.child.Add(matakanan);

            var middlemask = new Asset3d(new Vector3(0.901f, 0.584f, 0.215f));
            middlemask.createMiddleMask(1.5f, 3.0f, 0.27f, 0.4f);
            badanironman.child.Add(middlemask);

            var lowermask = new Asset3d(new Vector3(0.901f, 0.584f, 0.215f));
            lowermask.createLowerMask(1.5f, 2.885f, 0.27f, 0.4f);
            badanironman.child.Add(lowermask);
            var lowermouth = new Asset3d(new Vector3(0.796f, 0.494f, 0.145f));
            lowermouth.createLowerMouth(1.5f, 2.885f, 0.27f, 0.4f);
            badanironman.child.Add(lowermouth);

            var core = new Asset3d(new Vector3(0.203f, 0.972f, 0.976f));
            core.createEllipticParaboloid(0.02f, 0.01f, 0.012f, 1.5f, 2.75f, 0.15f);
            badanironman.child.Add(core);

            var api = new Asset3d(new Vector3(0.203f, 0.972f, 0.976f));
            api.createCylinder(0.1f, 1.0f, 1.5f, 2.45f, 0.0f);
            Api.Add(api);
            var apichild = new Asset3d(new Vector3(0.203f, 0.972f, 0.976f));
            apichild.createEllipsoid( 1.5f, 1.8f, 0.0f,0.3f, 0.3f, 0.3f,72,24);
            Api.Add(apichild);

            var apicurve = new Asset3d(new Vector3(0.203f, 0.972f, 0.976f));
            apicurve.createCurveBezier((0.1f, 0.9f, -0.2f), (0.4f, 0.9f, -0.1f), (0.2f, 0.9f, -0.0f));
            apicurve.translate(1f, 0f, 0f);
            Api.Add(apicurve);

            #endregion









            #region character
            var badan = new Asset3d(new Vector3(0.070f, 0.631f, 0.070f));
            badan.createEllipticParaboloid(0.08f, 0.08f, 0.08f, 0.0f, 0.0f, 0.0f);
            badan.rotate(Vector3.Zero, Vector3.UnitX, -90f);
            character.Add(badan);
            var badanatas = new Asset3d(new Vector3(0.070f, 0.631f, 0.070f));
            badanatas.createHalfEllipsoid(0.17f, 0.28f, 0.17f, 0.0f, 0.85f, 0.0f,72,24);
            badan.child.Add(badanatas);
            var pipi = new Asset3d(new Vector3(0.070f, 0.631f, 0.070f));
            pipi.createEllipsoid(0.0f, 0.8f, 0.0f, 0.28f, 0.15f, 0.28f,72,24);
            badan.child.Add(pipi);
            var celana = new Asset3d(new Vector3(0.7f, 0.4f, 0.0f));
            celana.createCylinder(0.17f, 0.15f, 0.0f, 0.25f, 0.0f);
            badan.child.Add(celana);
            var bawah = new Asset3d(new Vector3(0.070f, 0.631f, 0.070f));
            bawah.createHalfEllipsoid(0.15f, -0.2f, 0.15f, 0.0f, 0.2f, 0.0f,72,24);
            badan.child.Add(bawah);
            var mata = new Asset3d(new Vector3(1.0f, 1.0f, 1.0f));
            mata.createEllipsoid(0.03f, 1.0f, 0.13f, 0.03f, 0.05f, 0.03f, 100, 100);
            badan.child.Add(mata);
            var mata2 = new Asset3d(new Vector3(1.0f, 1.0f, 1.0f));
            mata2.createEllipsoid(-0.03f, 1.0f, 0.13f, 0.03f, 0.05f, 0.03f, 100, 100);
            badan.child.Add(mata2);
            var kelopakmata = new Asset3d(new Vector3(0.070f, 0.509f, 0.070f));
            kelopakmata.createHalfEllipsoid(0.03f, 0.05f, 0.03f, -0.03f, 1.02f, 0.13f,72,24);
            badan.child.Add(kelopakmata);
            var kelopakmata2 = new Asset3d(new Vector3(0.070f, 0.509f, 0.070f));
            kelopakmata2.createHalfEllipsoid(0.03f, 0.05f, 0.03f, 0.03f, 1.02f, 0.13f,72,24);
            badan.child.Add(kelopakmata2);
            var hitammata = new Asset3d(new Vector3(0.0f, 0.0f, 0.0f));
            hitammata.createEllipsoid(-0.03f, 0.99f, 0.15f, 0.015f, 0.015f, 0.015f, 100, 100);
            badan.child.Add(hitammata);
            var hitammata2 = new Asset3d(new Vector3(0.0f, 0.0f, 0.0f));
            hitammata2.createEllipsoid(0.03f, 0.99f, 0.15f, 0.015f, 0.015f, 0.015f, 100, 100);
            badan.child.Add(hitammata2);
            var lobanghidung = new Asset3d(new Vector3(0.0f, 0.0f, 0.0f));
            lobanghidung.createEllipsoid(-0.03f, 0.9f, 0.23f, 0.013f, 0.013f, 0.013f, 100, 100);
            badan.child.Add(lobanghidung);
            var lobanghidung2 = new Asset3d(new Vector3(0.0f, 0.0f, 0.0f));
            lobanghidung2.createEllipsoid(0.03f, 0.9f, 0.23f, 0.013f, 0.013f, 0.013f, 100, 100);
            badan.child.Add(lobanghidung2);
            var mulut = new Asset3d(new Vector3(1.0f, 0.0f, 0.0f));
            mulut.createHalfEllipsoid(0.15f, 0.1f, 0.15f, 0.0f, 0.77f, 0.15f,72,24);
            badan.child.Add(mulut);
            var otot = new Asset3d(new Vector3(0.070f, 0.509f, 0.070f));
            otot.createEllipsoid(-0.05f, 0.6f, 0.2f, 0.07f, 0.07f, 0.07f, 100, 100);
            badan.child.Add(otot);
            var otot2 = new Asset3d(new Vector3(0.070f, 0.509f, 0.070f));
            otot2.createEllipsoid(0.05f, 0.6f, 0.2f, 0.07f, 0.07f, 0.07f, 100, 100);
            badan.child.Add(otot2);
            var otot3 = new Asset3d(new Vector3(0.070f, 0.509f, 0.070f));
            otot3.createEllipsoid(-0.04f, 0.5f, 0.18f, 0.06f, 0.06f, 0.06f, 100, 100);
            badan.child.Add(otot3);
            var otot4 = new Asset3d(new Vector3(0.070f, 0.509f, 0.070f));
            otot4.createEllipsoid(0.04f, 0.5f, 0.18f, 0.06f, 0.06f, 0.06f, 100, 100);
            badan.child.Add(otot4);
            var otot5 = new Asset3d(new Vector3(0.070f, 0.509f, 0.070f));
            otot5.createEllipsoid(0.033f, 0.41f, 0.18f, 0.04f, 0.04f, 0.04f, 100, 100);
            badan.child.Add(otot5);
            var otot6 = new Asset3d(new Vector3(0.070f, 0.509f, 0.070f));
            otot6.createEllipsoid(-0.033f, 0.41f, 0.18f, 0.04f, 0.04f, 0.04f, 100, 100);
            badan.child.Add(otot6);
            var rambut = new Asset3d(new Vector3());
            rambut.createCurveBezier((0,1f, 0), (0f, 0.6f, 0), (0.1f, 0.3f, 0));
            rambut.translate(-0.2f, 0.25f, 0);
            badan.child.Add(rambut);
            var rambut2 = new Asset3d(new Vector3());
            rambut2.createCurveBezier((0, 1f, 0), (0f, 0.6f, 0), (0, 0.3f, -0.1f));
            rambut2.translate(0f, 0.25f, 0.2f);
            badan.child.Add(rambut2);
            var rambut3 = new Asset3d(new Vector3());
            rambut3.createCurveBezier((0, 1f, 0), (0f, 0.6f, 0), (-0.1f, 0.3f, 0));
            rambut3.translate(0.2f, 0.25f, 0);
            badan.child.Add(rambut3);
            var rambut4 = new Asset3d(new Vector3());
            rambut4.createCurveBezier((0, 1f, 0), (0f, 0.6f, 0), (0f, 0.3f, 0.1f));
            rambut4.translate(0f, 0.25f, -0.2f);
            badan.child.Add(rambut4);


            #endregion

            foreach (Asset3d i in Kota)
            {
                i.load(Size.X, Size.Y);
            }
            foreach(Asset3d i in ironman)
            {
                i.load(Size.X,Size.Y);
            }
            foreach (Asset3d i in character)
            {
                i.load(Size.X, Size.Y);
            }
            foreach (Asset3d i in Api)
            {
                i.load(Size.X, Size.Y);

            }
            foreach (Asset3d i in nuklir)
            {
                i.load(Size.X, Size.Y);
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            float time = (float)args.Time; //Deltatime ==> waktu antara frame sebelumnya ke frame berikutnya, gunakan untuk animasi

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); // DepthBufferBit juga harus di clear karena kita memakai depth testing.

            foreach (Asset3d i in Kota)
            {
                i.render(_camera.GetViewMatrix(), _camera.GetProjectionMatrix());
                for (int j = 0; j < 50; j++)
                {
                    Kota[4].rotate(Vector3.Zero, Vector3.UnitY, 45 * time);
                    Kota[5].rotate(Vector3.Zero, Vector3.UnitY, -45 * time);
                }

            }
            foreach (Asset3d i in nuklir)
            {
                i.render(_camera.GetViewMatrix(), _camera.GetProjectionMatrix());
               
                i.translate(0.0f, 0.005f * speednuklir, 0.0f);
                Console.WriteLine("X: " + i.objectCenter.Y);
            }
            if (nuklir[0].objectCenter.Y >= 6.5f || nuklir[0].objectCenter.Y <= 6f)
            {
                speednuklir *= -1f;
            }
        
            foreach (Asset3d k in ironman)
            {
                k.render(_camera.GetViewMatrix(), _camera.GetProjectionMatrix());
                
                k.translate(0.0f, 0.01f * speed, 0.0f);
                Console.WriteLine("X: " + k.objectCenter.Y);
            }
            if (ironman[0].objectCenter.Y >= 3.5f || ironman[0].objectCenter.Y <= 2.5f)
            {
                speed *= -1f;
            }


            foreach (Asset3d i in Api)
            {
                Console.WriteLine("y: " + i.objectCenter.Y);
                i.render(_camera.GetViewMatrix(), _camera.GetProjectionMatrix());

                
            }
            

            foreach (Asset3d i in character)
            {
                Console.WriteLine("y: " + i.objectCenter.Y);
                i.render(_camera.GetViewMatrix(), _camera.GetProjectionMatrix());
                
                
            }

            foreach (Asset3d i in character)
            {

                for (int j = 0; j < 8; j++)
                {

                    i.translate(0.0f, 0.05f * timelompat, 0.1f * time);

                    if (i.objectCenter.Y >= 1f)
                    {
                        timelompat = timelompat * -1;
                    }
                    else if (i.objectCenter.Y <= 0.1)
                    {
                        timelompat = 0.1f;
                    }
                    if (j == 2)
                        i.rotate(Vector3.Zero, Vector3.UnitY, -60 * time);
                }


            }



            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            float time = (float)args.Time; //Deltatime ==> waktu antara frame sebelumnya ke frame berikutnya, gunakan untuk animasi

            if (!IsFocused)
            {
                return; //Reject semua input saat window bukan focus.
            }

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            float cameraSpeed = 5.0f;

            if (KeyboardState.IsKeyDown(Keys.W))
            {
                _camera.Position += _camera.Front * cameraSpeed * (float)args.Time;
            }
            if (KeyboardState.IsKeyDown(Keys.S))
            {
                _camera.Position -= _camera.Front * cameraSpeed * (float)args.Time;
            }
            if (KeyboardState.IsKeyDown(Keys.A))
            {
                _camera.Position -= _camera.Right * cameraSpeed * (float)args.Time;
            }
            if (KeyboardState.IsKeyDown(Keys.D))
            {
                _camera.Position += _camera.Right * cameraSpeed * (float)args.Time;
            }
            if (KeyboardState.IsKeyDown(Keys.Space))
            {
                _camera.Position += _camera.Up * cameraSpeed * (float)args.Time;
            }
            if (KeyboardState.IsKeyDown(Keys.LeftControl))
            {
                _camera.Position -= _camera.Up * cameraSpeed * (float)args.Time;
            }
            if (KeyboardState.IsKeyDown(Keys.J))
            {
                foreach(Asset3d i in character)
                {
                    i.translate(-0.4f * time, 0.0f, 0.0f);
                }
            }
            if (KeyboardState.IsKeyDown(Keys.I))
            {
                foreach (Asset3d i in character)
                {
                    i.translate(0.0f, 0.0f, 0.4f*time);
                }
            }
            if (KeyboardState.IsKeyDown(Keys.L))
            {
                foreach (Asset3d i in character)
                {
                    i.translate(0.4f * time, 0.0f, 0.0f);
                }
            }
            if (KeyboardState.IsKeyDown(Keys.K))
            {
                foreach (Asset3d i in character)
                {
                    i.translate(0.0f, 0.0f, -0.4f*time);
                }
            }

            var mouse = MouseState;
            var sensitivity = 0.1f;

            //yaw menggerakan kamera ke kanan kiri
            //pitch menggerakan kamera ke atas bawah
            if (_firstmove)
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstmove = false;
            }
            else
            {
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _camera.Yaw += deltaX * sensitivity;
                _camera.Pitch -= deltaY * sensitivity;
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            _camera.Fov -= e.OffsetY;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
            _camera.AspectRatio = Size.X / (float) Size.Y;
        }
    }
}
