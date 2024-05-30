using System;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Mathematics;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using static OpenTK.Graphics.OpenGL.GL;
using static System.Formats.Asn1.AsnWriter;
using System.Diagnostics;
using System.Windows.Input;
//using System.Windows.Media.Media3D;

namespace EPQui
{
    public enum GyzmoType
    {
        translation,
        rotation,
        scale,
    }
    public class Gyzmo
    {
       public Mesh mesh;
        public Shader[] shaderProgram;
        public Shader[] clickProgram;
        public GyzmoType type = GyzmoType.translation;
        Vector3 mouseW;
        public Gyzmo()
        {
            mesh = new Mesh();
            shaderProgram = new Shader[] {
                new Shader("Res/Gyzmo.vert", "Res/light.frag", "Res/TransGyzmo.geomertry"),
                new Shader("Res/Gyzmo.vert", "Res/light.frag", "Res/RotationGyzmo .geomertry"),
                new Shader("Res/Gyzmo.vert", "Res/light.frag", "Res/ScaleGyzmo.geomertry")
            };
            clickProgram = new Shader[] { 
                new Shader("Res/Gyzmo.vert", "Res/ClicksGyz.frag", "Res/TransGyzmo.geomertry"),
                new Shader("Res/Gyzmo.vert", "Res/ClicksGyz.frag", "Res/RotationGyzmo .geomertry"),
                new Shader("Res/Gyzmo.vert", "Res/ClicksGyz.frag", "Res/ScaleGyzmo.geomertry")
            };
        }
        public void UpdateClick(Camera camera, HierObj slected)
        {
            for (int i = 0; i < clickProgram.Length; i++)
            {
                if (i == (int)type)
                {
                    clickProgram[i].Activate();
                    Matrix4 mod = slected.rotationMatrix * Matrix4.CreateTranslation(slected.Position + slected.PositionAdded);
                    GL.UniformMatrix4(GL.GetUniformLocation(clickProgram[i].ID, "model"), false, ref mod);
                    GL.Uniform3(GL.GetUniformLocation(clickProgram[i].ID, "camUp"), camera.OrientationU);
                    GL.Uniform3(GL.GetUniformLocation(clickProgram[i].ID, "camRight"), camera.OrientationR);
                    GL.Uniform3(GL.GetUniformLocation(clickProgram[i].ID, "camFr"), camera.Orientation);
                    GL.Uniform3(GL.GetUniformLocation(clickProgram[i].ID, "camP"), camera.Position);
                    mesh.DrawToClick(clickProgram[i], camera);
                }
            }
        }
        public void Update(Camera camera, HierObj slected)
        {
            for (int i = 0; i < shaderProgram.Length; i++)
            {
                if (i == (int)type)
                {
                    shaderProgram[i].Activate();
                    Matrix4 mod = slected.rotationMatrix * Matrix4.CreateTranslation(slected.Position + slected.PositionAdded);
                    GL.UniformMatrix4(GL.GetUniformLocation(clickProgram[i].ID, "model"), false, ref mod);
                    GL.Uniform3(GL.GetUniformLocation(shaderProgram[i].ID, "camUp"), camera.OrientationU);
                    GL.Uniform3(GL.GetUniformLocation(shaderProgram[i].ID, "camRight"), camera.OrientationR);
                    GL.Uniform3(GL.GetUniformLocation(shaderProgram[i].ID, "camFr"), camera.Orientation);
                    GL.Uniform3(GL.GetUniformLocation(shaderProgram[i].ID, "camP"), camera.Position);
                    mesh.DrawToClick(shaderProgram[i], camera);
                }
            }

        }
        float ray(Vector3 v1, Vector3 v2)
        {
            return MathF.Abs(Vector3.Dot(v1, v2));
        }
        public void edit(int editObj, Camera camera, HierObj slected, Vector3 point_world)
        {



            float ray_x = ray(camera.Orientation, Vector3.UnitX);
            float ray_y = ray(camera.Orientation, Vector3.UnitY);
            float ray_z = ray(camera.Orientation, Vector3.UnitZ);
            float t = 0;
            Vector3 nrm;
            if (editObj == 1)
            {
                if (ray_y > ray_x) nrm = Vector3.UnitY;
                else nrm = Vector3.UnitX;
                t = Vector3.Dot(nrm, slected.Position - camera.Position) / Vector3.Dot(nrm, point_world);
                slected.PositionAdded = new Vector3(0, 0, ((camera.Position + t * point_world) - mouseW).Z);
            }
            else if (editObj == 2)
            {
                if (ray_y > ray_z) nrm = Vector3.UnitY;
                else nrm = Vector3.UnitZ;
                t = Vector3.Dot(nrm, slected.Position - camera.Position) / Vector3.Dot(nrm, point_world);
                slected.PositionAdded = new Vector3(((camera.Position + t * point_world) - mouseW).X, 0, 0);
            }
            else if (editObj == 3)
            {
                if (ray_z > ray_x) nrm = Vector3.UnitZ;
                else nrm = Vector3.UnitX;
                t = Vector3.Dot(nrm, slected.Position - camera.Position) / Vector3.Dot(nrm, point_world);
                slected.PositionAdded = new Vector3(0, ((camera.Position + t * point_world) - mouseW).Y, 0);
            }
            else if (editObj == 4)
            {
                if (ray(camera.Orientation, (Vector4.UnitY * slected.rotationMatrix.Inverted()).Xyz) > ray(camera.Orientation, (Vector4.UnitX * slected.rotationMatrix.Inverted()).Xyz))
                {
                    nrm = (Vector4.UnitY * slected.rotationMatrix.Inverted()).Xyz;
                    t = Vector3.Dot(nrm, slected.Position - camera.Position) / Vector3.Dot(nrm, point_world);
                    slected.objectScaleAdded = new Vector3(0, 0, (new Vector4((Vector4.UnitZ * slected.rotationMatrix.Inverted()).Xyz * ((camera.Position + t * point_world) - mouseW)) * slected.rotationMatrix).Z);

                }
                else
                {
                    nrm = (Vector4.UnitX * slected.rotationMatrix.Inverted()).Xyz;
                    t = Vector3.Dot(nrm, slected.Position - camera.Position) / Vector3.Dot(nrm, point_world);
                    slected.objectScaleAdded = new Vector3(0, 0, (new Vector4((Vector4.UnitZ * slected.rotationMatrix.Inverted()).Xyz * ((camera.Position + t * point_world) - mouseW)) * slected.rotationMatrix).Z);

                }
            }
            else if (editObj == 5)
            {
                if (ray(camera.Orientation, (Vector4.UnitY * slected.rotationMatrix.Inverted()).Xyz) > ray(camera.Orientation, (Vector4.UnitZ * slected.rotationMatrix.Inverted()).Xyz))
                {
                    nrm = (Vector4.UnitY * slected.rotationMatrix.Inverted()).Xyz;
                    t = Vector3.Dot(nrm, slected.Position - camera.Position) / Vector3.Dot(nrm, point_world);
                    slected.objectScaleAdded = new Vector3((new Vector4((Vector4.UnitX * slected.rotationMatrix.Inverted()).Xyz * ((camera.Position + t * point_world) - mouseW)) * slected.rotationMatrix).X, 0, 0);

                }
                else
                {
                    nrm = (Vector4.UnitZ * slected.rotationMatrix.Inverted()).Xyz;
                    t = Vector3.Dot(nrm, slected.Position - camera.Position) / Vector3.Dot(nrm, point_world);
                    slected.objectScaleAdded = new Vector3((new Vector4((Vector4.UnitX * slected.rotationMatrix.Inverted()).Xyz * ((camera.Position + t * point_world) - mouseW)) * slected.rotationMatrix).X, 0, 0);

                }
            }
            else if (editObj == 6)
            {
                if (ray(camera.Orientation, (Vector4.UnitX * slected.rotationMatrix.Inverted()).Xyz) > ray(camera.Orientation, (Vector4.UnitZ * slected.rotationMatrix.Inverted()).Xyz))
                {
                    nrm = (Vector4.UnitX * slected.rotationMatrix.Inverted()).Xyz;
                    t = Vector3.Dot(nrm, slected.Position - camera.Position) / Vector3.Dot(nrm, point_world);
                    slected.objectScaleAdded = new Vector3(0, (new Vector4((Vector4.UnitY * slected.rotationMatrix.Inverted()).Xyz * ((camera.Position + t * point_world) - mouseW)) * slected.rotationMatrix).Y, 0);

                }
                else
                {
                    nrm = (Vector4.UnitZ * slected.rotationMatrix.Inverted()).Xyz;
                    t = Vector3.Dot(nrm, slected.Position - camera.Position) / Vector3.Dot(nrm, point_world);
                    slected.objectScaleAdded = new Vector3(0, (new Vector4((Vector4.UnitY * slected.rotationMatrix.Inverted()).Xyz * ((camera.Position + t * point_world) - mouseW)) * slected.rotationMatrix).Y, 0);

                }
            }
            else if (editObj == 7)
            {
                //slected.objectScaleAdded = new Vector3( Vector2.Distance( mouseW.Xy , mouseP));
            }
            else if (editObj == 8)
            {
                nrm = Vector3.UnitZ;
                t = Vector3.Dot(nrm, slected.Position - camera.Position) / Vector3.Dot(nrm, point_world);
                Vector3 newMouseW = (camera.Position + t * point_world);
                Vector3 v2 = newMouseW - slected.Position;
                Vector3 v1 = mouseW - slected.Position;
                float ang = (MathF.Atan2(Vector3.Dot(Vector3.Cross(v1, v2), nrm), Vector3.Dot(v1, v2)));
                slected.objectRotationAdded = Quaternion.FromAxisAngle(nrm, ang);
            }
            else if (editObj == 9)
            {
                nrm = Vector3.UnitX;
                t = Vector3.Dot(nrm, slected.Position - camera.Position) / Vector3.Dot(nrm, point_world);
                Vector3 newMouseW = (camera.Position + t * point_world);
                Vector3 v2 = newMouseW - slected.Position;
                Vector3 v1 = mouseW - slected.Position;
                float ang = (MathF.Atan2(Vector3.Dot(Vector3.Cross(v1, v2), nrm), Vector3.Dot(v1, v2)));
                slected.objectRotationAdded = Quaternion.FromAxisAngle(nrm, ang);
            }
            else if (editObj == 10)
            {
                nrm = Vector3.UnitY;
                t = Vector3.Dot(nrm, slected.Position - camera.Position) / Vector3.Dot(nrm, point_world);
                Vector3 newMouseW = (camera.Position + t * point_world);
                Vector3 v2 = newMouseW - slected.Position;
                Vector3 v1 = mouseW - slected.Position;
                float ang = (MathF.Atan2(Vector3.Dot(Vector3.Cross(v1, v2), nrm), Vector3.Dot(v1, v2)));
                slected.objectRotationAdded = Quaternion.FromAxisAngle(nrm, ang);
                Debug.WriteLine(ang * (180 / MathF.PI));
            }
            else if (editObj == 11)
            {
                nrm = camera.Orientation;
                t = Vector3.Dot(nrm, slected.Position - camera.Position) / Vector3.Dot(nrm, point_world);
                Vector3 newMouseW = (camera.Position + t * point_world);
                Vector3 v2 = newMouseW - slected.Position;
                Vector3 v1 = mouseW - slected.Position;
                float ang = (MathF.Atan2(Vector3.Dot(Vector3.Cross(v1, v2), nrm), Vector3.Dot(v1, v2)));
                slected.objectRotationAdded = Quaternion.FromAxisAngle(nrm, ang);
                Debug.WriteLine(ang * (180 / MathF.PI));
            }
            else if (editObj == 12)
            {
                t = Vector3.Dot(Vector3.UnitZ, slected.Position - camera.Position) / Vector3.Dot(Vector3.UnitZ, point_world);
                slected.PositionAdded = (camera.Position + t * point_world) - mouseW;
            }
            else if (editObj == 13)
            {
                t = Vector3.Dot(Vector3.UnitX, slected.Position - camera.Position) / Vector3.Dot(Vector3.UnitX, point_world);
                slected.PositionAdded = (camera.Position + t * point_world) - mouseW;
            }
            else if (editObj == 14)
            {
                t = Vector3.Dot(Vector3.UnitY, slected.Position - camera.Position) / Vector3.Dot(Vector3.UnitY, point_world);
                slected.PositionAdded = (camera.Position + t * point_world) - mouseW;
            }


        }
        public void set(int editObj, Camera camera, HierObj slected, Vector3 point_world)
        {

           // startRot = slected.rotationMatrix.Inverted();
            Vector3 nrm;
            float ray_x = MathF.Abs(Vector3.Dot(point_world, Vector3.UnitX) / point_world.Length);
            float ray_y = MathF.Abs(Vector3.Dot(point_world, Vector3.UnitY) / point_world.Length);
            float ray_z = MathF.Abs(Vector3.Dot(point_world, Vector3.UnitZ) / point_world.Length);

            float t = 0;
            if (editObj == 1)
            {
                if (ray_y > ray_x) nrm = Vector3.UnitY;
                else nrm = Vector3.UnitX;
                t = Vector3.Dot(nrm, slected.Position - camera.Position) / Vector3.Dot(nrm, point_world);
                mouseW = new Vector3(0, 0, ((camera.Position + t * point_world)).Z);
            }
            else if (editObj == 2)
            {
                if (ray_y > ray_z) nrm = Vector3.UnitY;
                else nrm = Vector3.UnitZ;
                t = Vector3.Dot(nrm, slected.Position - camera.Position) / Vector3.Dot(nrm, point_world);
                mouseW = new Vector3(((camera.Position + t * point_world)).X, 0, 0);
            }
            else if (editObj == 3)
            {
                if (ray_z > ray_x) nrm = Vector3.UnitZ;
                else nrm = Vector3.UnitX;
                t = Vector3.Dot(nrm, slected.Position - camera.Position) / Vector3.Dot(nrm, point_world);
                mouseW = new Vector3(0, ((camera.Position + t * point_world)).Y, 0);
            }
            else if (editObj == 4)
            {
                if (ray(camera.Orientation, (Vector4.UnitY * slected.rotationMatrix.Inverted()).Xyz) > ray(camera.Orientation, (Vector4.UnitX * slected.rotationMatrix.Inverted()).Xyz)) nrm = (Vector4.UnitY * slected.rotationMatrix.Inverted()).Xyz;
                else nrm = (Vector4.UnitX * slected.rotationMatrix.Inverted()).Xyz;
                t = Vector3.Dot(nrm, slected.Position - camera.Position) / Vector3.Dot(nrm, point_world);
                mouseW = ((camera.Position + t * point_world));
            }
            else if (editObj == 5)
            {
                if (ray(camera.Orientation, (Vector4.UnitY * slected.rotationMatrix.Inverted()).Xyz) > ray(camera.Orientation, (Vector4.UnitZ * slected.rotationMatrix.Inverted()).Xyz)) nrm = (Vector4.UnitY * slected.rotationMatrix.Inverted()).Xyz;
                else nrm = (Vector4.UnitZ * slected.rotationMatrix.Inverted()).Xyz;
                t = Vector3.Dot(nrm, slected.Position - camera.Position) / Vector3.Dot(nrm, point_world);
                mouseW = ((camera.Position + t * point_world));
            }
            else if (editObj == 6)
            {
                if (ray(camera.Orientation, (Vector4.UnitZ * slected.rotationMatrix.Inverted()).Xyz) > ray(camera.Orientation, (Vector4.UnitX * slected.rotationMatrix.Inverted()).Xyz)) nrm = (Vector4.UnitZ * slected.rotationMatrix.Inverted()).Xyz;
                else nrm = (Vector4.UnitX * slected.rotationMatrix.Inverted()).Xyz;
                t = Vector3.Dot(nrm, slected.Position - camera.Position) / Vector3.Dot(nrm, point_world);
                mouseW = ((camera.Position + t * point_world));
            }
            else if (editObj == 7)
            {
                // mouseW = new Vector3(mouseP);
            }
            else if (editObj == 8 || editObj == 12)
            {
                t = Vector3.Dot(Vector3.UnitZ, slected.Position - camera.Position) / Vector3.Dot(Vector3.UnitZ, point_world);
                mouseW = (camera.Position + t * point_world);
            }
            else if (editObj == 9 || editObj == 13)
            {
                t = Vector3.Dot(Vector3.UnitX, slected.Position - camera.Position) / Vector3.Dot(Vector3.UnitX, point_world);
                mouseW = (camera.Position + t * point_world);
            }
            else if (editObj == 10 || editObj == 14)
            {
                t = Vector3.Dot(Vector3.UnitY, slected.Position - camera.Position) / Vector3.Dot(Vector3.UnitY, point_world);
                mouseW = (camera.Position + t * point_world);
            }
            else if (editObj == 11)
            {
                t = Vector3.Dot(camera.Orientation, slected.Position - camera.Position) / Vector3.Dot(camera.Orientation, point_world);
                mouseW = (camera.Position + t * point_world);
            }

        }
    }
}
