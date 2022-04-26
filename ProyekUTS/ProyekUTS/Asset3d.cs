using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyekUTS
{
    class Asset3d
    {
        private readonly string path = "../../../Shaders/";

        public List<Vector3> vertices = new List<Vector3>();
        private List<uint> indices = new List<uint>();

        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private int _elementBufferObject;

        private Shader _shader;

        private Matrix4 model = Matrix4.Identity;   // Model Matrix      ==> Matrix ini yang akan berubah saat terjadi transformasi
        
        private Vector3 color;                      // Warna objek, dikirim ke shader lewat uniform.

        public List<Vector3> _euler = new List<Vector3>();  // Sudut lokal, relatif terhadap objek yang bersangkutan.
        public Vector3 objectCenter = Vector3.Zero;         // Titik tengah objek

        public List<Asset3d> child = new List<Asset3d>();   // Sistem Hierarchical Object ==> List untuk menampung objek - objek child.

        public Asset3d(Vector3 color)
        {
            this.color = color;
            _euler.Add(Vector3.UnitX); // Masukkan sudut Euler di Constructor.
            _euler.Add(Vector3.UnitY);
            _euler.Add(Vector3.UnitZ);
        }

        public void load(int sizeX, int sizeY)
        {
            _vertexBufferObject = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * Vector3.SizeInBytes, vertices.ToArray(), BufferUsageHint.StaticDraw);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            if (indices.Count != 0)
            {
                _elementBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
                GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * sizeof(uint), indices.ToArray(), BufferUsageHint.StaticDraw);
            }

            _shader = new Shader(path + "shader.vert", path + "shader.frag");
            _shader.Use();

            foreach (var i in child)
            {
                i.load(sizeX, sizeY);
            }
        }

        public void render(Matrix4 camera_view, Matrix4 camera_projection)
        {
            _shader.Use();
            GL.BindVertexArray(_vertexArrayObject);

            _shader.SetVector3("objColor", color);

            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", camera_view);
            _shader.SetMatrix4("projection", camera_projection);

            if (indices.Count != 0)
            {
                GL.DrawElements(PrimitiveType.Triangles, indices.Count, DrawElementsType.UnsignedInt, 0);
            }
            else
            {
                GL.DrawArrays(PrimitiveType.LineStrip, 0, vertices.Count);
            }

            foreach (var i in child)
            {
                i.render(camera_view, camera_projection);
            }
        }

        /// <summary>
        /// Berfungsi untuk me-reset sudut euler (sudut relatif terhadap objek)
        /// </summary>
        public void resetEuler()
        {
            _euler.Clear();
            _euler.Add(Vector3.UnitX);
            _euler.Add(Vector3.UnitY);
            _euler.Add(Vector3.UnitZ);
        }

        #region solidObjects

        //z nya minus artinya mejauh dari kamera
        public void createCuboid(float x_, float y_, float z_, float length)
        {
            var tempVertices = new List<Vector3>();
            Vector3 temp_vector;

            //Titik 1
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ + length / 2.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 2
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ + length / 2.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 3
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ - length / 2.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 4
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ - length / 2.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 5
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ + length / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 6
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ + length / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 7
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ - length / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 8
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ - length / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            var tempIndices = new List<uint>
            {
				//Back
				1, 2, 0,
                2, 1, 3,
				
				//Top
				5, 0, 4,
                0, 5, 1,

				//Right
				5, 3, 1,
                3, 5, 7,

				//Left
				0, 6, 4,
                6, 0, 2,

				//Front
				4, 7, 5,
                7, 4, 6,

				//Bottom
				3, 6, 2,
                6, 3, 7
            };
            vertices = tempVertices;
            indices = tempIndices;
        }

        //ini balok arena
        public void createCuboidFlat(float x_, float y_, float z_, float length)
        {
            var tempVertices = new List<Vector3>();
            Vector3 temp_vector;

            //Titik 1
            temp_vector.X = x_ - 3 * length;
            temp_vector.Y = y_ + length / 5.0f;
            temp_vector.Z = z_ - 2 * length;
            tempVertices.Add(temp_vector);

            //Titik 2
            temp_vector.X = x_ + 3 * length;
            temp_vector.Y = y_ + length / 5.0f;
            temp_vector.Z = z_ - 2 * length;
            tempVertices.Add(temp_vector);

            //Titik 3
            temp_vector.X = x_ - 3 * length;
            temp_vector.Y = y_ - length / 5.0f;
            temp_vector.Z = z_ - 2 * length;
            tempVertices.Add(temp_vector);

            //Titik 4
            temp_vector.X = x_ + 3 * length;
            temp_vector.Y = y_ - length / 5.0f;
            temp_vector.Z = z_ - 2 * length;
            tempVertices.Add(temp_vector);

            //Titik 5
            temp_vector.X = x_ - 3 * length;
            temp_vector.Y = y_ + length / 5.0f;
            temp_vector.Z = z_ + 2 * length;
            tempVertices.Add(temp_vector);

            //Titik 6
            temp_vector.X = x_ + 3 * length;
            temp_vector.Y = y_ + length / 5.0f;
            temp_vector.Z = z_ + 2 * length;
            tempVertices.Add(temp_vector);

            //Titik 7
            temp_vector.X = x_ - 3 * length;
            temp_vector.Y = y_ - length / 5.0f;
            temp_vector.Z = z_ + 2 * length;
            tempVertices.Add(temp_vector);

            //Titik 8
            temp_vector.X = x_ + 3 * length;
            temp_vector.Y = y_ - length / 5.0f;
            temp_vector.Z = z_ + 2 * length;
            tempVertices.Add(temp_vector);

            var tempIndices = new List<uint>
            {
				//Back
				1, 2, 0,
                2, 1, 3,
				
				//Top
				5, 0, 4,
                0, 5, 1,

				//Right
				5, 3, 1,
                3, 5, 7,

				//Left
				0, 6, 4,
                6, 0, 2,

				//Front
				4, 7, 5,
                7, 4, 6,

				//Bottom
				3, 6, 2,
                6, 3, 7
            };
            vertices = tempVertices;
            indices = tempIndices;
        }

        public void createEllipsoid(float x, float y, float z, float radX, float radY, float radZ, float sectorCount, float stackCount)
        {
            objectCenter = new Vector3(x, y, z);

            float pi = (float)Math.PI;
            Vector3 temp_vector;
            float sectorStep = 2 * pi / sectorCount;
            float stackStep = pi / stackCount;
            float sectorAngle, stackAngle, tempX, tempY, tempZ;

            for (int i = 0; i <= stackCount; ++i)
            {
                stackAngle = pi / 2 - i * stackStep;
                tempX = radX * (float)Math.Cos(stackAngle);
                tempY = radY * (float)Math.Sin(stackAngle);
                tempZ = radZ * (float)Math.Cos(stackAngle);

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = x + tempX * (float)Math.Cos(sectorAngle);
                    temp_vector.Y = y + tempY;
                    temp_vector.Z = z + tempZ * (float)Math.Sin(sectorAngle);

                    vertices.Add(temp_vector);
                }
            }

            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);

                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        indices.Add(k1);
                        indices.Add(k2);
                        indices.Add(k1 + 1);

                    }

                    if (i != stackCount - 1)
                    {
                        indices.Add(k1 + 1);
                        indices.Add(k2);
                        indices.Add(k2 + 1);
                    }
                }
            }
        }

        public void createTorus(float x, float y, float z, float radMajor, float radMinor, float sectorCount, float stackCount)
        {
            objectCenter = new Vector3(x, y, z);

            float pi = (float)Math.PI;
            Vector3 temp_vector;
            stackCount *= 2;
            float sectorStep = 2 * pi / sectorCount;
            float stackStep = 2 * pi / stackCount;
            float sectorAngle, stackAngle, tempX, tempY, tempZ;

            for (int i = 0; i <= stackCount; ++i)
            {
                stackAngle = pi / 2 - i * stackStep;
                tempX = radMajor + radMinor * (float)Math.Cos(stackAngle);
                tempY = radMinor * (float)Math.Sin(stackAngle);
                tempZ = radMajor + radMinor * (float)Math.Cos(stackAngle);

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = x + tempX * (float)Math.Cos(sectorAngle);
                    temp_vector.Y = y + tempY;
                    temp_vector.Z = z + tempZ * (float)Math.Sin(sectorAngle);

                    vertices.Add(temp_vector);
                }
            }

            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);

                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    indices.Add(k1);
                    indices.Add(k2);
                    indices.Add(k1 + 1);

                    indices.Add(k1 + 1);
                    indices.Add(k2);
                    indices.Add(k2 + 1);
                }
            }   
        }

        public void createCylinder(float radius, float height, float _x, float _y, float _z)
        {
            objectCenter.X = _x;
            objectCenter.Y = _y;
            objectCenter.Z = _z;
            float pi = (float)Math.PI;
            Vector3 temp_vector;

            for (float i = -pi / 2; i <= pi / 2; i += pi / 360)
            {
                for (float j = -pi; j <= pi; j += pi / 360)
                {
                    temp_vector.X = radius * (float)Math.Cos(i) * (float)Math.Cos(j) + objectCenter.X;
                    temp_vector.Y = radius * (float)Math.Cos(i) * (float)Math.Sin(j) + objectCenter.Y;
                    if (temp_vector.Y < objectCenter.Y)
                        temp_vector.Y = objectCenter.Y - height * 0.5f;
                    else
                        temp_vector.Y = objectCenter.Y + height * 0.5f;
                    temp_vector.Z = radius * (float)Math.Sin(i) + objectCenter.Z;
                    vertices.Add(temp_vector);
                }
            }
        }
        public void createEllipticParaboloid(float radiusX, float radiusY, float radiusZ, float _x, float _y, float _z)
        {

            objectCenter.X = _x;
            objectCenter.Y = _y;
            objectCenter.Z = _z;

            float pi = (float)Math.PI;
            Vector3 temp_vector;
            for (float v = -pi; v <= pi; v += pi / 300)
            {
                for (float u = 0; u <= 15; u += pi / 300)
                {
                    temp_vector.X = _x + v * (float)Math.Cos(u) * radiusX;
                    temp_vector.Y = _y + v * (float)Math.Sin(u) * radiusY;
                    temp_vector.Z = _z + (v * v) * radiusZ;
                    vertices.Add(temp_vector);

                }
            }
        }

        public void createHalfEllipsoid(float radiusX, float radiusY, float radiusZ, float _x, float _y, float _z, int sectorCount, int stackCount)
        {
            objectCenter.X = _x;
            objectCenter.Y = _y;
            objectCenter.Z = _z;

            float pi = (float)Math.PI;
            Vector3 temp_vector;
            float sectorStep = 2 * (float)Math.PI / sectorCount;
            float stackStep = (float)Math.PI / stackCount;
            float sectorAngle, StackAngle, x, y, z;

            for (int i = 0; i <= stackCount; ++i)
            {
                StackAngle = pi / 2 - i * stackStep;
                x = radiusX * (float)Math.Cos(StackAngle);
                y = radiusY * (float)Math.Cos(StackAngle);
                z = radiusZ * (float)Math.Sin(StackAngle);

                for (int j = sectorCount / 2; j >= 0; --j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = x * (float)Math.Cos(sectorAngle) + _x;
                    temp_vector.Y = y * (float)Math.Sin(sectorAngle) + _y;
                    temp_vector.Z = z + _z;
                    vertices.Add(temp_vector);
                }
            }

            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);
                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        indices.Add(k1);
                        indices.Add(k2);
                        indices.Add(k1 + 1);
                    }
                    if (i != (stackCount - 1))
                    {
                        indices.Add(k1 + 1);
                        indices.Add(k2);
                        indices.Add(k2 + 1);
                    }
                }
            }


        }
        public void createBoxVertices(float x, float y, float z, float length, float panjang, float lebar, float tinggi)
        {
            objectCenter.X = x;
            objectCenter.Y = y;
            objectCenter.Z = z;
            Vector3 temp_vector;

            //TITIK 1
            temp_vector.X = x - panjang / 2.0f;
            temp_vector.Y = y + lebar / 2.0f;
            temp_vector.Z = z - tinggi / 2.0f;
            vertices.Add(temp_vector);
            //TITIK 2
            temp_vector.X = x + panjang / 2.0f;
            temp_vector.Y = y + lebar / 2.0f;
            temp_vector.Z = z - tinggi / 2.0f;
            vertices.Add(temp_vector);
            //TITIK 3
            temp_vector.X = x - panjang / 2.0f;
            temp_vector.Y = y - lebar / 2.0f;
            temp_vector.Z = z - tinggi / 2.0f;
            vertices.Add(temp_vector);
            //TITIK 4
            temp_vector.X = x + panjang / 2.0f;
            temp_vector.Y = y - lebar / 2.0f;
            temp_vector.Z = z - tinggi / 2.0f;
            vertices.Add(temp_vector);
            //TITIK 5
            temp_vector.X = x - panjang / 2.0f;
            temp_vector.Y = y + lebar / 2.0f;
            temp_vector.Z = z + tinggi / 2.0f;
            vertices.Add(temp_vector);
            //TITIK 6
            temp_vector.X = x + panjang / 2.0f;
            temp_vector.Y = y + lebar / 2.0f;
            temp_vector.Z = z + tinggi / 2.0f;
            vertices.Add(temp_vector);
            //TITIK 7
            temp_vector.X = x - panjang / 2.0f;
            temp_vector.Y = y - lebar / 2.0f;
            temp_vector.Z = z + tinggi / 2.0f;
            vertices.Add(temp_vector);
            //TITIK 8
            temp_vector.X = x + panjang / 2.0f;
            temp_vector.Y = y - lebar / 2.0f;
            temp_vector.Z = z + tinggi / 2.0f;
            vertices.Add(temp_vector);

            indices = new List<uint>
            {
                //SEGITIGA DEPAN 1
                0,1,2,
                //SEGITIGA DEPAN 2
                1,2,3,
                //SEGITIGA ATAS 1
                0,4,5,
                //SEGITIGA ATAS 2
                0,1,5,
                //SEGITIGA KANAN 1
                1,3,5,
                //SEGITIGA KANAN 2
                3,5,7,
                //SEGITIGA KIRI 1
                0,2,4,
                //SEGITIGA KIRI 2
                2,4,6,
                //SEGITIGA BELAKANG 1
                4,5,6,
                //SEGITIGA BELAKANG 2
                5,6,7,
                //SEGITIGA BAWAH 1
                2,3,6,
                //SEGITIGA BAWAH 2
                3,6,7
            };
        }
        public void createLowerMask(float x, float y, float z, float length)
        {
            objectCenter.X = x;
            objectCenter.Y = y;
            objectCenter.Z = z;
            Vector3 temp_vector;

            //TITIK 1
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y + length / 5.0f;
            temp_vector.Z = z - length / 15.0f;
            vertices.Add(temp_vector);
            //TITIK 2
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y + length / 5.0f;
            temp_vector.Z = z - length / 15.0f;
            vertices.Add(temp_vector);
            //TITIK 3
            temp_vector.X = x - length / 5.0f;
            temp_vector.Y = y - length / 10.0f;
            temp_vector.Z = z - length / 15.0f;
            vertices.Add(temp_vector);
            //TITIK 4
            temp_vector.X = x + length / 5.0f;
            temp_vector.Y = y - length / 10.0f;
            temp_vector.Z = z - length / 15.0f;
            vertices.Add(temp_vector);
            //TITIK 5
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y + length / 5.0f;
            temp_vector.Z = z + length / 15.0f;
            vertices.Add(temp_vector);
            //TITIK 6
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y + length / 5.0f;
            temp_vector.Z = z + length / 15.0f;
            vertices.Add(temp_vector);
            //TITIK 7
            temp_vector.X = x - length / 5.0f;
            temp_vector.Y = y - length / 10.0f;
            temp_vector.Z = z + length / 15.0f;
            vertices.Add(temp_vector);
            //TITIK 8
            temp_vector.X = x + length / 5.0f;
            temp_vector.Y = y - length / 10.0f;
            temp_vector.Z = z + length / 15.0f;
            vertices.Add(temp_vector);

            indices = new List<uint>
            {
                //SEGITIGA DEPAN 1
                0,1,2,
                //SEGITIGA DEPAN 2
                1,2,3,
                //SEGITIGA ATAS 1
                0,4,5,
                //SEGITIGA ATAS 2
                0,1,5,
                //SEGITIGA KANAN 1
                1,3,5,
                //SEGITIGA KANAN 2
                3,5,7,
                //SEGITIGA KIRI 1
                0,2,4,
                //SEGITIGA KIRI 2
                2,4,6,
                //SEGITIGA BELAKANG 1
                4,5,6,
                //SEGITIGA BELAKANG 2
                5,6,7,
                //SEGITIGA BAWAH 1
                2,3,6,
                //SEGITIGA BAWAH 2
                3,6,7
            };
        }
        public void createLowerMouth(float x, float y, float z, float length)
        {
            objectCenter.X = x;
            objectCenter.Y = y;
            objectCenter.Z = z;
            Vector3 temp_vector;

            //TITIK 1
            temp_vector.X = x - length / 10.0f;            /////
            temp_vector.Y = y + length / 30.0f;
            temp_vector.Z = z - length / 14.0f;
            vertices.Add(temp_vector);
            //TITIK 2
            temp_vector.X = x + length / 10.0f;          /////
            temp_vector.Y = y + length / 30.0f;
            temp_vector.Z = z - length / 14.0f;
            vertices.Add(temp_vector);
            //TITIK 3
            temp_vector.X = x - length / 5.0f;
            temp_vector.Y = y - length / 10.0f;
            temp_vector.Z = z - length / 14.0f;
            vertices.Add(temp_vector);
            //TITIK 4
            temp_vector.X = x + length / 5.0f;
            temp_vector.Y = y - length / 10.0f;
            temp_vector.Z = z - length / 14.0f;
            vertices.Add(temp_vector);
            //TITIK 5
            temp_vector.X = x - length / 10.0f;          /////
            temp_vector.Y = y + length / 30.0f;
            temp_vector.Z = z + length / 14.0f;
            vertices.Add(temp_vector);
            //TITIK 6
            temp_vector.X = x + length / 10.0f;          /////
            temp_vector.Y = y + length / 30.0f;
            temp_vector.Z = z + length / 14.0f;
            vertices.Add(temp_vector);
            //TITIK 7
            temp_vector.X = x - length / 5.0f;
            temp_vector.Y = y - length / 10.0f;
            temp_vector.Z = z + length / 14.0f;
            vertices.Add(temp_vector);
            //TITIK 8
            temp_vector.X = x + length / 5.0f;
            temp_vector.Y = y - length / 10.0f;
            temp_vector.Z = z + length / 14.0f;
            vertices.Add(temp_vector);

            indices = new List<uint>
            {
                //SEGITIGA DEPAN 1
                0,1,2,
                //SEGITIGA DEPAN 2
                1,2,3,
                //SEGITIGA ATAS 1
                0,4,5,
                //SEGITIGA ATAS 2
                0,1,5,
                //SEGITIGA KANAN 1
                1,3,5,
                //SEGITIGA KANAN 2
                3,5,7,
                //SEGITIGA KIRI 1
                0,2,4,
                //SEGITIGA KIRI 2
                2,4,6,
                //SEGITIGA BELAKANG 1
                4,5,6,
                //SEGITIGA BELAKANG 2
                5,6,7,
                //SEGITIGA BAWAH 1
                2,3,6,
                //SEGITIGA BAWAH 2
                3,6,7
            };
        }

        public void createMiddleMask(float x, float y, float z, float length)
        {
            objectCenter.X = x;
            objectCenter.Y = y;
            objectCenter.Z = z;
            Vector3 temp_vector;

            //TITIK 1
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y + length / 5.0f;
            temp_vector.Z = z - length / 15.0f;
            vertices.Add(temp_vector);
            //TITIK 2
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y + length / 5.0f;
            temp_vector.Z = z - length / 15.0f;
            vertices.Add(temp_vector);
            //TITIK 3
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y - length / 10.0f;
            temp_vector.Z = z - length / 15.0f;
            vertices.Add(temp_vector);
            //TITIK 4
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y - length / 10.0f;
            temp_vector.Z = z - length / 15.0f;
            vertices.Add(temp_vector);
            //TITIK 5
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y + length / 5.0f;
            temp_vector.Z = z + length / 15.0f;
            vertices.Add(temp_vector);
            //TITIK 6
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y + length / 5.0f;
            temp_vector.Z = z + length / 15.0f;
            vertices.Add(temp_vector);
            //TITIK 7
            temp_vector.X = x - length / 2.0f;
            temp_vector.Y = y - length / 10.0f;
            temp_vector.Z = z + length / 15.0f;
            vertices.Add(temp_vector);
            //TITIK 8
            temp_vector.X = x + length / 2.0f;
            temp_vector.Y = y - length / 10.0f;
            temp_vector.Z = z + length / 15.0f;
            vertices.Add(temp_vector);

            indices = new List<uint>
            {
                //SEGITIGA DEPAN 1
                0,1,2,
                //SEGITIGA DEPAN 2
                1,2,3,
                //SEGITIGA ATAS 1
                0,4,5,
                //SEGITIGA ATAS 2
                0,1,5,
                //SEGITIGA KANAN 1
                1,3,5,
                //SEGITIGA KANAN 2
                3,5,7,
                //SEGITIGA KIRI 1
                0,2,4,
                //SEGITIGA KIRI 2
                2,4,6,
                //SEGITIGA BELAKANG 1
                4,5,6,
                //SEGITIGA BELAKANG 2
                5,6,7,
                //SEGITIGA BAWAH 1
                2,3,6,
                //SEGITIGA BAWAH 2
                3,6,7
            };
        }
        public void createCurveBezier(Vector3 a,Vector3 b ,Vector3 c)
        {
            //ini nyoba di tiga titik
            //vertices.Add(new Vector3(0, 0.5f,-3));
            //vertices.Add(new Vector3(0, 1f, -2));
            //vertices.Add(new Vector3(0, 1.5f, -1));
            vertices.Add(new Vector3 (a));
            vertices.Add(new Vector3(b));
            vertices.Add(new Vector3(c));

            List<Vector3> _verticesBezier = new List<Vector3>();
            List<int> pascal = new List<int>();
            if (vertices.Count > 1)
            {
                pascal = getRow(vertices.Count);
                for (float t = 0; t <= 1.0f; t += 0.005f)
                {
                    Vector3 p = getP(pascal, t);
                    _verticesBezier.Add(p);
                }
            }
            vertices = _verticesBezier;
        }

        public Vector3 getP(List<int> pascal, float t)
        {
            Vector3 p = new Vector3(0, 0, 0);
            for (int i = 0; i < vertices.Count; i++)
            {
                float temp = (float)Math.Pow((1 - t), vertices.Count - 1 - i) * (float)Math.Pow(t, i) * pascal[i];
                p += temp * vertices[i];
            }
            return p;
        }
        public List<int> getRow(int rowIndex)
        {
            List<int> currow = new List<int>();
            currow.Add(1);
            if (rowIndex == 0)
            {
                return currow;
            }

            List<int> prev = getRow(rowIndex - 1);
            for (int i = 1; i < prev.Count; i++)
            {
                int curr = prev[i - 1] + prev[i];
                currow.Add(curr);
            }
            currow.Add(1);
            return currow;
        }

        //public void createCylinder(float x, float y, float radius, float height)
        //{
        //    List<Vector3> tempVertices = new List<Vector3>();
        //    Vector3 temp_vector;
        //    //Lingkaran
        //    for (int i = 0; i < 360; i++)
        //    {
        //        double degInRad = i * Math.PI / 180;

        //        //x
        //        temp_vector.X = radius * (float)Math.Cos(degInRad) + x;
        //        //y
        //        temp_vector.Z = radius * (float)Math.Sin(degInRad) + y;
        //        //y
        //        temp_vector.Y = -2;
        //        tempVertices.Add(temp_vector);
        //    }
        //    vertices = tempVertices;
        //}

        #endregion

        #region transforms
        public void rotate(Vector3 pivot, Vector3 vector, float angle)
        {
            var radAngle = MathHelper.DegreesToRadians(angle);

            var arbRotationMatrix = new Matrix4
                (
                new Vector4((float)(Math.Cos(radAngle) + Math.Pow(vector.X, 2.0f) * (1.0f - Math.Cos(radAngle))), (float)(vector.X * vector.Y * (1.0f - Math.Cos(radAngle)) + vector.Z * Math.Sin(radAngle)), (float)(vector.X * vector.Z * (1.0f - Math.Cos(radAngle)) - vector.Y * Math.Sin(radAngle)), 0),
                new Vector4((float)(vector.X * vector.Y * (1.0f - Math.Cos(radAngle)) - vector.Z * Math.Sin(radAngle)), (float)(Math.Cos(radAngle) + Math.Pow(vector.Y, 2.0f) * (1.0f - Math.Cos(radAngle))), (float)(vector.Y * vector.Z * (1.0f - Math.Cos(radAngle)) + vector.X * Math.Sin(radAngle)), 0),
                new Vector4((float)(vector.X * vector.Z * (1.0f - Math.Cos(radAngle)) + vector.Y * Math.Sin(radAngle)), (float)(vector.Y * vector.Z * (1.0f - Math.Cos(radAngle)) - vector.X * Math.Sin(radAngle)), (float)(Math.Cos(radAngle) + Math.Pow(vector.Z, 2.0f) * (1.0f - Math.Cos(radAngle))), 0),
                Vector4.UnitW
                );

            model *= Matrix4.CreateTranslation(-pivot);
            model *= arbRotationMatrix;
            model *= Matrix4.CreateTranslation(pivot);

            for (int i = 0; i < 3; i++)
            {
                _euler[i] = Vector3.Normalize(getRotationResult(pivot, vector, radAngle, _euler[i], true));
            }

            objectCenter = getRotationResult(pivot, vector, radAngle, objectCenter);

            foreach (var i in child)
            {
                i.rotate(pivot, vector, angle);
            }
        }

        public Vector3 getRotationResult(Vector3 pivot, Vector3 vector, float angle, Vector3 point, bool isEuler = false)
        {
            Vector3 temp, newPosition;

            if (isEuler)
            {
                temp = point;
            }
            else
            {
                temp = point - pivot;
            }

            newPosition.X =
                temp.X * (float)(Math.Cos(angle) + Math.Pow(vector.X, 2.0f) * (1.0f - Math.Cos(angle))) +
                temp.Y * (float)(vector.X * vector.Y * (1.0f - Math.Cos(angle)) - vector.Z * Math.Sin(angle)) +
                temp.Z * (float)(vector.X * vector.Z * (1.0f - Math.Cos(angle)) + vector.Y * Math.Sin(angle));

            newPosition.Y =
                temp.X * (float)(vector.X * vector.Y * (1.0f - Math.Cos(angle)) + vector.Z * Math.Sin(angle)) +
                temp.Y * (float)(Math.Cos(angle) + Math.Pow(vector.Y, 2.0f) * (1.0f - Math.Cos(angle))) +
                temp.Z * (float)(vector.Y * vector.Z * (1.0f - Math.Cos(angle)) - vector.X * Math.Sin(angle));

            newPosition.Z =
                temp.X * (float)(vector.X * vector.Z * (1.0f - Math.Cos(angle)) - vector.Y * Math.Sin(angle)) +
                temp.Y * (float)(vector.Y * vector.Z * (1.0f - Math.Cos(angle)) + vector.X * Math.Sin(angle)) +
                temp.Z * (float)(Math.Cos(angle) + Math.Pow(vector.Z, 2.0f) * (1.0f - Math.Cos(angle)));

            if (isEuler)
            {
                temp = newPosition;
            }
            else
            {
                temp = newPosition + pivot;
            }
            return temp;
        }

        public void translate(float x, float y, float z)
        {
            model *= Matrix4.CreateTranslation(x, y, z);
            objectCenter.X += x;
            objectCenter.Y += y;
            objectCenter.Z += z;

            foreach (var i in child)
            {
                i.translate(x, y, z);
            }
        }
        public void jump(float x,float y, float z)
        {
            model *= Matrix4.CreateTranslation(x, y,z);
            objectCenter.X += x;
            objectCenter.Y += y;
            objectCenter.Z += z;
        
   
            foreach (var i in child)
            {
                i.jump(x, y, z);
            }
        }

        //jika ada koordinat yang tidak ingin diubah, masukan 1 di parameternya
        //jadi misal mau scaleX sebanyak 2: di parameternya -> .scale(2, 1, 1)
        public void scale(float scaleX, float scaleY, float scaleZ)
        {
            model *= Matrix4.CreateTranslation(-objectCenter);
            model *= Matrix4.CreateScale(scaleX, scaleY, scaleZ);
            model *= Matrix4.CreateTranslation(objectCenter);

            foreach (var i in child)
            {
                i.scale(scaleX, scaleY, scaleZ);
            }
        }
        #endregion
    }
}
